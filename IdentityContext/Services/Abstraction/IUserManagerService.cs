using IdentityContext.Dtos;
using IdentityContext.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityContext.Services.Abstraction
{
    public interface IUserManagerService
    {
        Task<Guid> CreateUserAsync(string userName, string password);

        Task UpdatePassCodeAsync(Guid userId, string passwordhash);

        Task<Guid> CreateUserAsync(string userName, string password, string email);

        Task<Guid> CreateUserAsync(string userName, string password, string email, string name, string phoneNumber, bool mustAddClaims= false);

        Task<Guid> CreateUserWithOtpAsync(string mobile, string NationalCode, string passcode, string Otp = null);

        Task DeleteUserAsync(Guid userId);

        IQueryable<UserDto> GetApplicationUsers();

        Task<UserDto> FindByIdAsync(string id);

        Task<UserDto> FindByMobileAsync(string mobile);

        Task<int> GetUserCountAsync();

        Task<UserDto> FindByNameAsync(string userName);

        Task<bool> CheckUserNameExistsAsync(string userName);

        Task AddToRoleAsync(string userId, string role);
        Task AddToRolesAsync(string userId, List<string> role);
        Task RemoveFromRolesAsync(string userId);
        Task<IList<string>> GetUserRolesAsync(string userId);

        Task UpdateUserAsync(UpdateUserDto request);

        Task<UserDto> CheckPasswordSignInAsync(string userName, string passcode);

        Task<TokenDto> GetTokensAsync(UserDto user, Func<List<ClaimDto>> bussinessClaims = null);

        Task<TokenDto> GetTokensAsync(string refreshToken, Func<List<ClaimDto>> bussinessClaims = null);
        
        Task<Guid> GetUserIdAsync(string refreshToken);

        Task<IEnumerable<ClaimDto>> GetClaimsAsync(Guid userId);

        Task UpdateOtpByMobileAsync(string mobile, string otp);

        string GetRandomOtp(int digits = 6);

        Task<ApplicationUserEntity> GetUserAsync(UserDto user);

    }
}
