using IdentityContext.Dtos;
using IdentityContext.Services.Abstraction;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Dtos.Common;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Repositories;
using MajlesMefa.Back.Utilities.EnumHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DocumentFormat.OpenXml.InkML;
using IdentityContext.Entities;
using Newtonsoft.Json;
using MajlesMefa.Back.Entities;
using Dapper;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Grid;

namespace MajlesMefa.Back.UseCases.Commmands.LoginCommand
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenDto>
    {
        private readonly IUserManagerService _userManagerService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AuthDbContext authDbContext;
        private readonly DapperContext _dapper;

        public LoginCommandHandler(IUserManagerService userManagerService,
            IUnitOfWork unitOfWork,
            AuthDbContext authDbContext,
            DapperContext dapper)
        {
            _userManagerService = userManagerService;
            _unitOfWork = unitOfWork;
            this.authDbContext = authDbContext;
            _dapper = dapper;
        }

        public class NamayandeganPassword
        {
            public string UserCode { get; set; }
            public string  NewPassword { get; set; }
        }

        public async Task<TokenDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
    //        var users = authDbContext.Users
    //.FromSqlRaw("SELECT * FROM AspNetUsers WHERE UserName BETWEEN '1000' AND '1300' ORDER BY UserName")
    //.ToList();

    //        using (var connection = _dapper.CreateConnection())
    //        {
    //            var query = "select * from NamayandeganPassword";
    //            try
    //            {
    //                var namayandeganPassword = await connection.QueryAsync<NamayandeganPassword>(query);

    //                foreach (var usr in users)
    //                {

    //                    var namayande = namayandeganPassword.FirstOrDefault(n => n.UserCode == usr.UserName);
    //                    if (namayande != null && namayande.NewPassword != string.Empty)
    //                    {
    //                        await _userManagerService.UpdatePassCodeAsync(usr.Id, namayande.NewPassword);
    //                    }
    //                    //var newPass = _unitOfWork.
    //                }
    //            }
    //            catch (Exception ex)
    //            {

    //                throw;
    //            }


    //        }
            //        System.IO.File.AppendAllText(@"c:\test\index.txt", $"before check pass - {DateTime.Now.ToString()}");


            //var sdqw = JsonConvert.SerializeObject(await _unitOfWork.UserRepository.Tracking.FirstOrDefaultAsync(c=>c.UserId==new Guid("4196f437-8580-48b0-b6a6-a61f794f4991")));


            var user = await _userManagerService.CheckPasswordSignInAsync(request.Username, request.Password);
            var extraData = await _unitOfWork.UserRepository
                .NoTracking
                .Where(x => x.UserId == user.Id)
                .Select(x => new CurrentUserDto()
                {
                    Name = x.Name,
                    City = x.CityId.HasValue? new CityDto() {
                        Name = x.City.Name,
                        Id = x.Id,
                        ParentId= x.City.ParentCityId,
                        ParentName= x.City.Parent.Name,
                    }: null,
                    Organization = x.OrganizationId.HasValue? new OrganizationDto()
                    {
                        Name = x.Organization.Name,
                        ParentName = x.Organization.Parent.Name,
                        Id= x.Organization.Id,
                        ParentId = x.Organization.ParentId,
                    }: null,
                    UserId = user.Id,
                    BussinessUserId = x.Id
                })
                .SingleOrDefaultAsync();
            var roles = (await _userManagerService.GetUserRolesAsync(user.Id.ToString())
                )
                .ToList();
            var roleNames = roles
                .Select(x => Enum.Parse<RoleTypeEnum>(x).GetDisplayName())
                .ToList();

            var token = await _userManagerService.GetTokensAsync(user, () =>
            {
                var bsClaims = new List<ClaimDto>(12)
                {
                    new ClaimDto(ClaimTypes.Role, string.Join(',', roles)),
                    new ClaimDto("RoleNames", string.Join(',', roleNames)),
                    new ClaimDto("Fullname", extraData.Name),
                    new ClaimDto("BussinessUserId", extraData.BussinessUserId.ToString()),
                };
                if (extraData.City!= null)
                {
                    bsClaims.Add(new ClaimDto("CityId", extraData.City.Id.ToString()));
                    if(extraData.City.Name != null)
                        bsClaims.Add(new ClaimDto("CityName", extraData.City.Name));
                    if (extraData.City.ParentId != null)
                        bsClaims.Add(new ClaimDto("CityParentId", extraData.City.ParentId.ToString()));
                    if (extraData.City.ParentName != null) 
                        bsClaims.Add(new ClaimDto("CityParentName", extraData.City.ParentName));
                }
                if (extraData.Organization!= null)
                {
                    bsClaims.Add(new ClaimDto("OrganizationId", extraData.Organization.Id.ToString()));
                    if(extraData.Organization.Name != null)
                        bsClaims.Add(new ClaimDto("OrganizationName", extraData.Organization.Name));
                    if (extraData.Organization.ParentId != null)
                        bsClaims.Add(new ClaimDto("OrganizationParentId", extraData.Organization.ParentId.ToString()));
                    if (extraData.Organization.ParentName != null) 
                        bsClaims.Add(new ClaimDto("OrganizationParentName", extraData.Organization.ParentName));
                }
                return bsClaims;
            });
            return token;

        }
    }
}
