using IdentityContext.Dtos;
using IdentityContext.Services.Abstraction;
using MediatR;
using MajlesMefa.Back.Dtos.Common;
using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Utilities.EnumHelper;
using System.Security.Claims;

namespace MajlesMefa.Back.UseCases.Commmands.LoginByRTokenCommand
{
    public class LoginByRTokenCommandHandler : IRequestHandler<LoginByRTokenCommand, TokenDto>
    {
        private readonly IUserManagerService _userManagerService;
        private readonly IUnitOfWork _unitOfWork;

        public LoginByRTokenCommandHandler(IUserManagerService userManagerService,
            IUnitOfWork unitOfWork)
        {
            _userManagerService = userManagerService;
            _unitOfWork = unitOfWork;
        }

        public async Task<TokenDto> Handle(LoginByRTokenCommand request, CancellationToken cancellationToken)
        {
            var userId = await _userManagerService.GetUserIdAsync(request.RefreshToken);
            var extraData = await _unitOfWork.UserRepository
                .NoTracking
                .Where(x => x.UserId == userId)
                .Select(x => new CurrentUserDto()
                {
                    Name = x.Name,
                    City = x.CityId.HasValue? new CityDto()
                    {
                        Name = x.City.Name,
                        Id = x.Id,
                        ParentId = x.City.ParentCityId,
                        ParentName = x.City.Parent.Name,
                    }: null,
                    Organization = x.OrganizationId.HasValue? new OrganizationDto()
                    {
                        Name = x.Organization.Name,
                        ParentName = x.Organization.Parent.Name,
                        Id = x.Organization.Id,
                        ParentId = x.Organization.ParentId,
                    }: null,
                    UserId = x.UserId,
                    BussinessUserId = x.Id
                })
                .SingleOrDefaultAsync();
            var roles = (await _userManagerService.GetUserRolesAsync(userId.ToString())
                )
                .ToList();
            var roleNames = roles
                .Select(x => Enum.Parse<RoleTypeEnum>(x).GetDisplayName())
                .ToList();

            var token = await _userManagerService.GetTokensAsync(request.RefreshToken, () =>
            {
                var bsClaims = new List<ClaimDto>(12)
                {
                    new ClaimDto(ClaimTypes.Role, string.Join(',', roles)),
                    new ClaimDto("RoleNames", string.Join(',', roleNames)),
                    new ClaimDto("Fullname", extraData.Name),
                    new ClaimDto("BussinessUserId", extraData.BussinessUserId.ToString()),
                };
                if (extraData.City != null)
                {
                    bsClaims.Add(new ClaimDto("CityId", extraData.City.Id.ToString()));
                    if (extraData.City.Name != null)
                        bsClaims.Add(new ClaimDto("CityName", extraData.City.Name));
                    if (extraData.City.ParentId != null)
                        bsClaims.Add(new ClaimDto("CityParentId", extraData.City.ParentId.ToString()));
                    if (extraData.City.ParentName != null)
                        bsClaims.Add(new ClaimDto("CityParentName", extraData.City.ParentName));
                }
                if (extraData.Organization != null )
                {
                    bsClaims.Add(new ClaimDto("OrganizationId", extraData.Organization.Id.ToString()));
                    if (extraData.Organization.Name != null)
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
