using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using AutoMapper;
using IdentityContext.Services.Abstraction;
using IdentityContext.Entities;
using IdentityContext.Extensions;
using IdentityContext.Dtos;

namespace CreamFramework.Infrastructure.Identity
{
    public class RoleManagerService : IRoleManagerService
    {
        private readonly RoleManager<ApplicationRoleEntity> _roleManager;
        private readonly IUserManagerService _userManager;
        private readonly IMapper _mapper;

        public RoleManagerService(RoleManager<ApplicationRoleEntity> roleManager, IMapper mapper, IUserManagerService userManager)
        {
            _roleManager = roleManager;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<Guid> CreateRoleAsync(string roleName)
        {
            var role = new ApplicationRoleEntity
            {
                Name = roleName
            };

            var result = await _roleManager.CreateAsync(role);
            result.ThrowIfFail();
            return role.Id;
        }

        public async Task DeleteRoleAsync(Guid userId)
        {
            var role = await _roleManager.Roles.SingleOrDefaultAsync(u => u.Id == userId);

            if (role != null)
            {
                await DeleteRoleAsync(role);
            }
        }

        public async Task DeleteRoleAsync(ApplicationRoleEntity role)
        {
            var rslt = await _roleManager.DeleteAsync(role);
            rslt.ThrowIfFail();
        }
        public async Task<IQueryable<RoleDto>> GetApplicationRolesAsync()
        {
            return (await _roleManager.Roles
                .Select(c => new RoleDto
                {
                    Name = c.Name,
                    Id = c.Id,
                    NormalizedName = c.NormalizedName,
                    FaName = c.FaName
                }).ToListAsync()).AsQueryable();
        }

        public async Task UpdateRole(RoleUpdateDto request)
        {
            var role = await _roleManager.FindByIdAsync(request.Id.ToString());

            if (role != null)
            {
                role.Name = request.Name;
                role.FaName = request.FaName;
            }
            var result = await _roleManager.UpdateAsync(role);
            result.ThrowIfFail();

        }

        public async Task<RoleDto> FindByIdAsync(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);           
            return new RoleDto
            {
                Name = role.Name,
                Id = role.Id,
                NormalizedName = role.NormalizedName,
                FaName = role.FaName
            };
        }
        public async Task<RoleDto> FindByNameAsync(string name)
        {
			var user = await _userManager.FindByNameAsync(name);
			var roleName = await _userManager.GetUserRolesAsync(user.Id.ToString());
			var role = await _roleManager.FindByNameAsync(roleName.FirstOrDefault());
            return new RoleDto
            {
                Name = role.Name,
                Id = role.Id,
                NormalizedName = role.NormalizedName,
                FaName = role.FaName
            };
        }
        public async Task<IList<RoleDto>> FindByNamesAsync(IList<string> names)
        {
            var roles = await _roleManager.Roles
                .Where(r => names.Contains(r.Name))
                .Select(role => new RoleDto
                {
                    Name = role.Name,
                    Id = role.Id,
                    NormalizedName = role.NormalizedName,
                    FaName = role.FaName
                })
                .ToListAsync();
            return roles;
        }
        public async Task<int> GetRoleCount()
        {
            return await _roleManager.Roles.CountAsync();
        }

        //public async Task<RoleResponse> UpdateRoleDetail(UpdateRoleDetailCommand request)
        //{
        //    var role = new RoleDto(request.RoleId, request.Name);
        //    var appRole = await _roleManager.FindByIdAsync(role.Id.ToString());
        //    return await _roleManager.UpdateAsync(appRole);

        //}

        public async Task<Guid> CreateAsync(RoleDto role)
        {
            var appRole = new ApplicationRoleEntity
            {
                Id = role.Id,
                Name = role.Name,
                NormalizedName = role.NormalizedName,
                FaName = role.FaName
            };

            var identityResult = await _roleManager.CreateAsync(appRole);
            identityResult.ThrowIfFail();

            return appRole.Id;
        }

        public async Task<Guid> Update(RoleDto role)
        {
            var appRole = await _roleManager.FindByIdAsync(role.Id.ToString());            
            var identityResult = await _roleManager.UpdateAsync(appRole);
            identityResult.ThrowIfFail();
            return appRole.Id;
        }

        //public async Task<RoleDto> FindById(string roleId)
        //{
        //    var data = await _roleManager.FindByIdAsync(roleId);
        //    return _mapper.Map<RoleDto>(data);
        //}       

        public async Task AddClaims(RoleDto role, List<string> claims)
        {
            var appRole = await _roleManager.FindByIdAsync(role.Id.ToString());
            foreach (var item in claims)
            {
                await _roleManager.AddClaimAsync(appRole, new Claim("Permission", item));
            }
        }

        public async Task RemoveClaims(RoleDto role, IList<ClaimDto> claims)
        {
            var appRole = await _roleManager.FindByIdAsync(role.Id.ToString());
            foreach (var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(appRole, new Claim(claim.Type, claim.Value));
            }
        }


        //public async Task RemoveRoleById(Guid id)
        //{
        //    await _roleManager.DeleteAsync(new IdentityRole<Guid> { Id = id });
        //}

        public async Task<List<ClaimDto>> GetClaims(RoleDto role)
        {
            //var appRole = _mapper.Map<IdentityRole<Guid>>(role);

            var appRole = new ApplicationRoleEntity
            {
                Id = role.Id,
                Name = role.Name,
                NormalizedName = role.NormalizedName,
                FaName = role.FaName,
            };

            return (await _roleManager.GetClaimsAsync(appRole))
                .Select(o => new ClaimDto(o.Type, o.Value))
                .ToList();
        }

        public async Task<List<ClaimDto>> GetClaims(Guid roleId) =>
            (await _roleManager.GetClaimsAsync(new  ApplicationRoleEntity { Id = roleId }))
            .Select(o => new ClaimDto(o.Type, o.Value))
            .ToList();
    }
}
