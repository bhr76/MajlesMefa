using IdentityContext.Dtos;
using IdentityContext.Services.Abstraction;
using MediatR;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Repositories;
using MajlesMefa.Back.Utilities.EnumHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.SeedRolesAndAdminsCommand
{
    public class SeedRolesAndAdminsCommandHandler : IRequestHandler<SeedRolesAndAdminsCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserManagerService _userManagerService;
        private readonly IRoleManagerService _roleManagerService;

        public SeedRolesAndAdminsCommandHandler(IUserManagerService userManagerService,
            IUnitOfWork unitOfWork,
            IRoleManagerService roleManagerService)
        {
            _userManagerService = userManagerService;
            _unitOfWork = unitOfWork;
            _roleManagerService = roleManagerService;
        }

        public async Task Handle(SeedRolesAndAdminsCommand request, CancellationToken cancellationToken)
        {
            await _roleManagerService.CreateAsync(new RoleDto()
            {
                FaName = RoleTypeEnum.Admin.GetDisplayName(),
                Name = RoleTypeEnum.Admin.ToString(),
            });
            await _roleManagerService.CreateAsync(new RoleDto()
            {
                FaName = RoleTypeEnum.MinistryAdmin.GetDisplayName(),
                Name = RoleTypeEnum.MinistryAdmin.ToString(),
            });
            await _roleManagerService.CreateAsync(new RoleDto()
            {
                FaName = RoleTypeEnum.MinistryMember.GetDisplayName(),
                Name = RoleTypeEnum.MinistryMember.ToString(),
            });
            await _roleManagerService.CreateAsync(new RoleDto()
            {
                FaName = RoleTypeEnum.Senator.GetDisplayName(),
                Name = RoleTypeEnum.Senator.ToString(),
            });
            await _roleManagerService.CreateAsync(new RoleDto()
            {
                FaName = RoleTypeEnum.Organization.GetDisplayName(),
                Name = RoleTypeEnum.Organization.ToString(),
            });

            var mainAdminUser = await _userManagerService.CreateUserAsync("admin", "P@ssw0rd", "", "کاربر ادمین", "09164911098");
            var ministryAdminUser = await _userManagerService.CreateUserAsync("vezarat", "V3z@r@tKh@n#h", "", "ادمین وزارت خانه", "09114911098");

            await _userManagerService.AddToRoleAsync(mainAdminUser.ToString(), RoleTypeEnum.Admin.ToString());
            await _userManagerService.AddToRoleAsync(ministryAdminUser.ToString(), RoleTypeEnum.MinistryAdmin.ToString());

            _unitOfWork.RoleRepository.Add(new BussinessRoleEntity()
            {
                RoleType = RoleTypeEnum.Admin,
            });
            _unitOfWork.RoleRepository.Add(new BussinessRoleEntity()
            {
                RoleType = RoleTypeEnum.MinistryAdmin,
            });
            _unitOfWork.RoleRepository.Add(new BussinessRoleEntity()
            {
                RoleType = RoleTypeEnum.MinistryMember,
            });
            _unitOfWork.RoleRepository.Add(new BussinessRoleEntity()
            {
                RoleType = RoleTypeEnum.Organization,
            });
            _unitOfWork.RoleRepository.Add(new BussinessRoleEntity()
            {
                RoleType = RoleTypeEnum.Senator,
            });

            _unitOfWork.UserRepository.Add(new UserEntity()
            {
                CreatorUserId = Guid.Empty,
                IsActive = true,
                Mobile = "09164911098",
                Name = "کاربر ادمین",
                UserId = mainAdminUser,
            });

            _unitOfWork.UserRepository.Add(new UserEntity()
            {
                CreatorUserId = Guid.Empty,
                IsActive = true,
                Mobile = "09164911098",
                Name = "ادمین وزارت خانه",
                UserId = ministryAdminUser,
            });

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
