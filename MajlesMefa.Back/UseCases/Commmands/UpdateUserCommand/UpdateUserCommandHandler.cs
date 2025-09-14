using IdentityContext.Dtos;
using IdentityContext.Services.Abstraction;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Repositories;
using MajlesMefa.Back.Services.Abstractioin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.UpdateUserCommand
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManagerService _userManagerService;
        private readonly ICurrentUserService _currentUserService;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork,
            IUserManagerService userManagerService,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _userManagerService = userManagerService;
            _currentUserService = currentUserService;
        }

        public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var cuser = _currentUserService.GetCurrentUser();
            var role = await _unitOfWork.RoleRepository
                .NoTracking
                .Where(x => x.RoleType == request.Role)
                .SingleOrDefaultAsync();
            var user = await _unitOfWork.UserRepository
                .Tracking
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .Where(x => x.Id ==  request.UserId)
                .SingleOrDefaultAsync();

            var mustRemoveSenatorProfile = request.Role != RoleTypeEnum.Senator && user.UserRoles.Any(x => x.Role.RoleType == RoleTypeEnum.Senator);

            if (!cuser.Roles.Any(x => x == RoleTypeEnum.Admin))
            {
                if (!cuser.Roles.Any(x => x == RoleTypeEnum.MinistryAdmin))
                {
                    if (!cuser.Roles.Any(x => x == RoleTypeEnum.MinistryMember))
                    {
                        request.OrganizationId = cuser.Organization.Id == Guid.Empty ? null : cuser.Organization.Id;
                        if (!cuser.Roles.Contains(request.Role))
                        {
                            throw new AccessViolationException("اجازه ی اعطای دسترسی سطح بالا تر ندارید");
                        }
                    }
                    else
                    {
                        if (!cuser.Roles.Contains(request.Role))
                        {
                            if (request.Role == (RoleTypeEnum.Admin | RoleTypeEnum.MinistryAdmin))
                            {
                                throw new AccessViolationException("اجازه ی اعطای دسترسی سطح بالا تر ندارید");
                            }
                        }
                    }
                }
                else
                {
                    if (!cuser.Roles.Contains(request.Role))
                    {
                        if (request.Role == RoleTypeEnum.Admin)
                        {
                            throw new AccessViolationException("اجازه ی اعطای دسترسی سطح بالا تر ندارید");
                        }
                    }
                }
            }
            if(user.CreatorUserId != cuser.UserId && user.Id != cuser.UserId)
            {
                if(cuser.Roles.Any(x => x != RoleTypeEnum.Admin))
                {
                    if (cuser.Roles.Any(x => x != RoleTypeEnum.MinistryAdmin))
                    {
                        throw new AccessViolationException("تنها اجازه ی ویرایش کاربری که تعریف نموده اید را دارید");
                    }
                    else
                    {
                        var roles = await _userManagerService.GetUserRolesAsync(user.UserId.ToString());
                        if (roles.Contains(RoleTypeEnum.Admin.ToString()))
                        {
                            throw new AccessViolationException("اجاره ی ویرایش کاربر سطح بالاتر را ندارید");
                        }
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(request.Password))
            {
                await _userManagerService.UpdatePassCodeAsync(user.UserId, request.Password);
            }
            var userDto = new UpdateUserDto()
            {
                Email = request.Email,
                Id = user.UserId,
                ManageUserRoles = new List<ManageUserRolesDto>() {
                    new ManageUserRolesDto() {
                        RoleName = request.Role.ToString(),
                    }
                },
                Name = request.Name,
                PhoneNumber = request.Mobile
            };

            await _userManagerService.UpdateUserAsync(userDto);
            user.CityId = request.CityId;
            user.IsActive = request.IsActive;
            user.Mobile = request.Mobile;
            user.Name = request.Name;
            user.OrganizationId = request.OrganizationId;
            if(user.UserRoles.Any(x => x.RoleId != role.Id))
            {
                user.UserRoles.FirstOrDefault().RoleId = role.Id;
            }

            if (mustRemoveSenatorProfile)
            {
                var senator = await _unitOfWork.SenatorRepository.FindAsync(user.Id);
                _unitOfWork.SenatorRepository.Delete(senator);
            }
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
