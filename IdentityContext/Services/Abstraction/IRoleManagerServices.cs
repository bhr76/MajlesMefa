using IdentityContext.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityContext.Services.Abstraction
{
    public interface IRoleManagerService
    {
        Task<Guid> CreateRoleAsync(string roleName);
        Task DeleteRoleAsync(Guid userId);
        Task<IQueryable<RoleDto>> GetApplicationRolesAsync();
        Task UpdateRole(RoleUpdateDto request);
        Task<RoleDto> FindByIdAsync(string id);
        Task<RoleDto> FindByNameAsync(string name);
        Task<int> GetRoleCount();
        Task<Guid> CreateAsync(RoleDto role);
        Task<Guid> Update(RoleDto role);
        // Task<bool> Validate(RoleDto role);
        //Task<RoleDto> FindById(string roleId);

        Task<IList<RoleDto>> FindByNamesAsync(IList<string> names);

        Task AddClaims(RoleDto role, List<string> claims);
        Task RemoveClaims(RoleDto role, IList<ClaimDto> claims);
        Task<List<ClaimDto>> GetClaims(RoleDto role);

        //Task RemoveRoleById(int id);
        Task<List<ClaimDto>> GetClaims(Guid roleId);
    }
}
