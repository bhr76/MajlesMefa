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

namespace MajlesMefa.Back.UseCases.Commmands.LoginCommand
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, TokenDto>
    {
        private readonly IUserManagerService _userManagerService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AuthDbContext authDbContext;

        public LoginCommandHandler(IUserManagerService userManagerService,
            IUnitOfWork unitOfWork,
            AuthDbContext authDbContext)
        {
            _userManagerService = userManagerService;
            _unitOfWork = unitOfWork;
            this.authDbContext = authDbContext;
        }

        public async Task<TokenDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
    //        var users = authDbContext.Users
    //.FromSqlRaw("SELECT * FROM AspNetUsers WHERE UserName BETWEEN '1000' AND '1300' ORDER BY UserName")
    //.ToList();
    //        foreach (var usr in users)
    //        {
    //            try
    //            {
    //                await _userManagerService.UpdatePassCodeAsync(usr.Id, usr.UserName + "Aa@1234");
    //            }
    //            catch (Exception ex)
    //            {

    //                throw;
    //            }
             
                
    //        }
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
