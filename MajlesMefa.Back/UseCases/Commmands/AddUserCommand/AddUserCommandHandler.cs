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
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.AddUserCommand
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManagerService _userManagerService;
        private readonly ICurrentUserService _currentUserService;

        public AddUserCommandHandler(IUnitOfWork unitOfWork,
            IUserManagerService userManagerService,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _userManagerService = userManagerService;
            _currentUserService = currentUserService;
        }

        public async Task<Guid> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                request.Name = request.Name;
            }            var cuser = _currentUserService.GetCurrentUser();
            var bRole = await _unitOfWork.RoleRepository
                .NoTracking
                .Where(x => x.RoleType == request.Role)
                .Select(x => x.Id)
                .SingleOrDefaultAsync();

            var authUseer = await  _userManagerService.CreateUserAsync(request.Username, request.Password, request.Email, request.Name, request.Mobile);
            if(!cuser.Roles.Any(x=> x == RoleTypeEnum.Admin))
            {
                if(!cuser.Roles.Any(x => x == RoleTypeEnum.MinistryAdmin))
                {
                    if(!cuser.Roles.Any(x => x == RoleTypeEnum.MinistryMember))
                    {
                        request.OrganizationId = cuser.Organization.Id == Guid.Empty? null: cuser.Organization.Id;
                        if (!cuser.Roles.Contains(request.Role))
                        {
                            throw new AccessViolationException("اجازه ی اعطای دسترسی سطح بالا تر ندارید");
                        }
                    }
                    else
                    {
                        if (!cuser.Roles.Contains(request.Role))
                        {
                            if(request.Role == (RoleTypeEnum.Admin | RoleTypeEnum.MinistryAdmin))
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

            if (cuser.Roles.Count == 1 && cuser.Roles.Single() == RoleTypeEnum.Senator)
            {
                request.CityId = cuser.City.Id == Guid.Empty? null : cuser.City.Id;
            }

            await _userManagerService.AddToRoleAsync(authUseer.ToString(), request.Role.ToString());


            var user = new UserEntity()
            {
                CityId = request.CityId,
                IsActive = request.IsActive,
                Mobile = request.Mobile,
                Name = request.Name,
                OrganizationId = request.OrganizationId,
                UserId = authUseer,
            };
            
            var userRole = new UserRoleEntity()
            {
                RoleId = bRole,
                User = user
            };

            _unitOfWork.UserRepository.Add(user);
            _unitOfWork.UserRepository.AddToRole(userRole);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return user.Id;
        }
    }
}
