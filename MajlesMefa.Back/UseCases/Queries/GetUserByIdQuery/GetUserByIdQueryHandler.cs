using IdentityContext.Services.Abstraction;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Dtos.UserDtos;
using MajlesMefa.Back.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetUserByIdQuery
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDetailDto>
    {
        private readonly IUserManagerService _userManagerService;
        private readonly IUnitOfWork _unitOfWork;

        public GetUserByIdQueryHandler(IUserManagerService userManagerService, 
            IUnitOfWork unitOfWork)
        {
            _userManagerService = userManagerService;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserDetailDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
          
            var rslt = await _unitOfWork.UserRepository
                .NoTracking
                .Where(x => x.Id == request.UserId)
                .Select(x => new UserDetailDto()
                {
                    CityId = x.CityId,
                    CityName = x.City.Name,
                    OrganizationName = x.Organization.Name,
                    OrganizationParentName = x.Organization.Parent.Name,
                    Mobile = x.Mobile,
                    OrganizationId = x.OrganizationId,
                    OrganizationParentId = x.Organization.ParentId,
                    IsActive = x.IsActive,
                    AvatarFileName = x.AvatarFileName,
                    Name = x.Name,
                    Role = x.UserRoles.FirstOrDefault().Role.RoleType,
                    Id = x.Id,
                    UserId = x.UserId,
                })
                .SingleOrDefaultAsync(cancellationToken);

            var identity = await _userManagerService.FindByIdAsync(rslt.UserId.ToString());

            rslt.Username = identity.UserName;
            rslt.Email = identity.Email;
            return rslt;
        }
    }
}
