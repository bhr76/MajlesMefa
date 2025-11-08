using FluentFTP.Helpers;
using IdentityContext.Dtos;
using IdentityContext.Entities;
using IdentityContext.Extensions;
using IdentityContext.Models.Settings;
using IdentityContext.Services.Abstraction;
using IdentityContext.Services.Abstraction.External;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace CreamFramework.Infrastructure.Identity
{
    public class UserManagerService : IUserManagerService
    {

        private readonly UserManager<ApplicationUserEntity> _userManager;
        private readonly SignInManager<ApplicationUserEntity> _signInManager;
        private readonly IOptionService _optionService;
        private readonly AuthDbContext _context;
        private readonly IPasswordHasher<ApplicationUserEntity> hasher;

        public UserManagerService(
            UserManager<ApplicationUserEntity> userManager,
            SignInManager<ApplicationUserEntity> signInManager,
            IOptionService optionService,
            AuthDbContext context,
            IPasswordHasher<ApplicationUserEntity> hasher)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _optionService = optionService;
            _context = context;
            this.hasher = hasher;
        }

        public async Task<Guid> CreateUserAsync(string userName, string password)
        {
            var user = new ApplicationUserEntity
            {
                UserName = userName,
                Email = userName,
            };

            var result = await _userManager.CreateAsync(user, password);
            result.ThrowIfFail();
            return user.Id;
        }

        public async Task<Guid> CreateUserAsync(string userName, string password, string email)
        {
            var user = new ApplicationUserEntity
            {
                UserName = userName,
                Email = email,
            };

            var result = await _userManager.CreateAsync(user, password);
            result.ThrowIfFail();
            return user.Id;
        }

        public async Task<Guid> CreateUserAsync(string userName, string password, string email, string name, string phoneNumber, bool mustAddClaims)
        {
            var user = new ApplicationUserEntity
            {
                UserName = userName,
                PasswordHash = password,
                Email = email,
                PhoneNumber = phoneNumber
            };

            var result = await _userManager.CreateAsync(user, password);
            result.ThrowIfFail();
            if (mustAddClaims)
            {
                var claimsToAdd = new List<Claim>() {
                new Claim(ClaimTypes.GivenName, name),
            };
                await _userManager.AddClaimsAsync(user, claimsToAdd);
            }
            return  user.Id;
        }

        public async Task<Guid> CreateUserWithOtpAsync(string mobile, string NationalCode, string passcode, string Otp = null)
        {
            var user = new ApplicationUserEntity
            {
                UserName = NationalCode,
                PhoneNumber = mobile,
                OtpCode = Otp,
                PasswordHash = passcode,
                LockoutEnabled = false
            };

            var result = await _userManager.CreateAsync(user);


            if (result.Succeeded)
            {
                var claimsToAdd = new List<Claim>() {
                    new Claim(nameof(user.UserName), user.UserName),
                    new Claim(nameof(user.PhoneNumber), user.PhoneNumber),
                };
                await _userManager.AddClaimsAsync(user, claimsToAdd);
            }

            return user.Id;
        }

        public async Task UpdatePassCodeAsync(Guid userId, string password)
        {
            
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, password);
            result.ThrowIfFail();
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            if (user != null)
            {
                await DeleteUserAsync(user);
            }
        }


        public async Task DeleteUserAsync(ApplicationUserEntity user)
        {
            var result = await _userManager.DeleteAsync(user);
            result.ThrowIfFail();
        }

        public IQueryable<UserDto> GetApplicationUsers()
        {
            var query = from q in (from user in _context.Users
                                   join claim in _context.UserClaims on user.Id equals claim.UserId
                                   where claim.ClaimType == ClaimTypes.GivenName || claim.ClaimType == ClaimTypes.Surname
                                   select new
                                   {
                                       user.Id,
                                       user.UserName,
                                       user.Email,
                                       user.PhoneNumber,
                                       Name = claim.ClaimType == ClaimTypes.GivenName ? claim.ClaimValue : null,
                                   }
                    )
                        group q by new
                        {
                            q.Id,
                            q.UserName,
                            q.Email,
                            q.PhoneNumber
                        }
                    into grouping
                        select new UserDto()
                        {
                            Id = grouping.Key.Id,
                            Email = grouping.Key.Email,
                            PhoneNumber = grouping.Key.PhoneNumber,
                            Name = grouping.Max(k => k.Name),
                            UserName = grouping.Key.UserName
                        };

            return query;

        }

        public async Task<IEnumerable<ClaimDto>> GetClaimsAsync(Guid userId)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == userId);
            var rslt = await _userManager.GetClaimsAsync(user);
            return rslt.Select(x => new ClaimDto(x.Type, x.Value));
        }

        public async Task UpdateUserAsync(UpdateUserDto request)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            var claims = await _userManager.GetClaimsAsync(user);

            if (user != null)
            {
                user.Email = request.Email;
                user.PhoneNumber = request.PhoneNumber;
            }

            var claimsToAdd = new List<Claim>() {
                new Claim(ClaimTypes.Surname, request.Name)
            };

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                await _userManager.RemoveClaimsAsync(user, claims);
                await _userManager.AddClaimsAsync(user, claimsToAdd);
                if(request.ManageUserRoles != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var removeRoles = roles.Where(r => !request.ManageUserRoles.Any(x => x.RoleName == r));
                    var addedRoles = request.ManageUserRoles.Where(r => !roles.Any(x => x == r.RoleName))
                        .Select(x => x.RoleName);
                    var r = await _userManager.RemoveFromRolesAsync(user ,removeRoles);
                    if (r.Succeeded)
                    {
                        var addRslt = await _userManager.AddToRolesAsync(user, addedRoles);
                        addRslt.ThrowIfFail();
                    }
                    r.ThrowIfFail();
                }
            }

            result.ThrowIfFail();
        }

        public async Task<UserDto> FindByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            var claims = await _userManager.GetClaimsAsync(user);


            return new UserDto
            {
                UserName = user.UserName,
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Name = claims.Where(c => c.Type == ClaimTypes.Surname)
                .Select(c => c.Value).DefaultIfEmpty("").FirstOrDefault(),
            };

        }


        public async Task<UserDto> FindByMobileAsync(string mobile)
        {
            var user = await _userManager.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.PhoneNumber == mobile);

            if (user == null) { return null; }

            var claims = await _userManager.GetClaimsAsync(user);

            return new UserDto
            {
                UserName = user.UserName,
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                OtpCode = user.OtpCode,
                OtpExpiresIn = (int)(user.OtpExpiersOn - DateTime.Now).TotalSeconds,
                Name = claims.Where(c => c.Type == ClaimTypes.Surname)
                .Select(c => c.Value).DefaultIfEmpty("").FirstOrDefault(),
            };
        }


        public async Task UpdateOtpByMobileAsync(string mobile, string otp)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.PhoneNumber == mobile);
            if (user == null)
            {
                throw new NullReferenceException("عدم تطابق کد otp");
            }
            user.OtpCode = otp;
            user.OtpExpiersOn = DateTime.Now.AddSeconds(180);
            await _userManager.UpdateAsync(user);
        }



        public async Task<int> GetUserCountAsync()
        {
            return await _userManager.Users.CountAsync();
        }


        public async Task<UserDto> FindByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return null;
            }

            var claims = await _userManager.GetClaimsAsync(user);

            return new UserDto
            {
                UserName = user.UserName,
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Name = claims.Where(c => c.Type == ClaimTypes.Surname)
                .Select(c => c.Value).DefaultIfEmpty("").FirstOrDefault(),
            };

        }

        public async Task<bool> CheckUserNameExistsAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return (user != null);

        }

        public async Task AddToRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new NullReferenceException("کابر یافت نشد");
            }

            var result = await _userManager.AddToRoleAsync(user, role);

            result.ThrowIfFail();
        }
        public async Task AddToRolesAsync(string userId, List<string> role)
        {

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new NullReferenceException("کابر یافت نشد");
            }

            var result = await _userManager.AddToRolesAsync(user, role);

            result.ThrowIfFail();
        }

        public async Task RemoveFromRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);


            if (user == null)
            {
                throw new NullReferenceException("کابر یافت نشد");
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, userRoles);

            result.ThrowIfFail();
        }
        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return null;

            return await _userManager.GetRolesAsync(user);
        }
        public async Task<UserDto> CheckPasswordSignInAsync(string userName, string passcode)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new UnauthorizedAccessException("اطلاعات ورود نامعتبر است");
            }
            if (user.LockoutEnabled)
            {
                throw new NullReferenceException("مسدودید");
            }
            if (user.TwoFactorEnabled)
            {
                throw new NullReferenceException("ورود دو مرحله ای فعال شده است");
            }

            if (user.ChangePassword)
            {
                throw new UnauthorizedAccessException("اطلاعات ورود نامعتبر است");
            }
            if (user == null)
            {
                throw new UnauthorizedAccessException("اطلاعات ورود نامعتبر است");
            }
            
            var hashedPass = hasher.HashPassword(user, passcode);
           

            var loginRslt = await _signInManager.CheckPasswordSignInAsync(user, passcode, false);
            if (!loginRslt.Succeeded)
            {
                

                var problemDetails = new ProblemDetails
                {
                    Title = "اطلاعات ورود نامعتبر است",
                    Status = StatusCodes.Status401Unauthorized, // یا 400 اگر ترجیح می‌دهید
                };

                //throw new Exception("اطلاعات ورود نامعتبر است");
                throw new UnauthorizedAccessException("اطلاعات ورود نامعتبر است");
            }
            

            var claims = await _userManager.GetClaimsAsync(user);
            var rslt = new UserDto
            {
                UserName = user.UserName,
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Name = claims.Where(c => c.Type == ClaimTypes.Surname)
                .Select(c => c.Value).DefaultIfEmpty("").FirstOrDefault(),
            };
            return rslt;
        }

        public async Task<TokenDto> GetTokensAsync(UserDto user, Func<List<ClaimDto>> bussinessClaims)
        {
            var config = _optionService.GetConfig<JwtSetting>();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Secret));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var userData = await _userManager.FindByNameAsync(user.UserName);

            var claims = new List<Claim> {
                new Claim(nameof(user.UserName), user.UserName),
                //new Claim(nameof(user.PhoneNumber), user.PhoneNumber),
                new Claim(ClaimTypes.NameIdentifier, userData.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            if(bussinessClaims != null)
            {
                var k = bussinessClaims();
                var x = k.Select(x => new Claim(x.Type, x.Value)).ToList();
                claims.AddRange(x);
            }

            var token = new JwtSecurityToken(config.ValidIssuer,
                config.ValidIssuer,
                claims,
                expires: DateTime.Now.AddSeconds(config.AccessTokenExpirationTimeSeconds),
                signingCredentials: credentials);


            var tokenRslt = new JwtSecurityTokenHandler().WriteToken(token);
            var refreshToken = new RefreshTokenEntity()
            {
                JwtId = token.Id,
                IsUsed = false,
                ApplicationUserId = user.Id,
                AddedDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddSeconds(config.RefreshTokenExpirationTimeSeconds),
                IsRevoked = false,
                Token = RandomString(25) + Guid.NewGuid()

            };

            _context.Entry(refreshToken).State = EntityState.Added;
            await _context.SaveChangesAsync();
            byte[] encodedText = new UTF8Encoding().GetBytes(DateTime.Now.ToString("HH:mm:ss tt") + config.Secret);
            var hashData = MD5.Create().ComputeHash(encodedText);
            var hexData = Convert.ToHexString(hashData);
            return new TokenDto()
            {
                AccessToken = tokenRslt,
                RefreshToken = refreshToken.Token,
                SessionToken = hexData,
                AccessTokenExpiresInSeconds = config.AccessTokenExpirationTimeSeconds,
                RefresshTokenExpiresInSeconds = config.RefreshTokenExpirationTimeSeconds,
            };

        }


        public async Task<TokenDto> GetTokensAsync(string refreshToken, Func<List<ClaimDto>> bussinessClaims)
        {
            var config = _optionService.GetConfig<JwtSetting>();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Secret));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var refreshTokenEntity = await _context.RefreshTokens
                .Include(r => r.ApplicationUser)
                .AsNoTracking()
                .SingleOrDefaultAsync(r =>
                    r.Token == refreshToken
                    && r.ExpiryDate > DateTime.Now);

            var oldClaims = await _userManager.GetClaimsAsync(refreshTokenEntity.ApplicationUser);

            if (refreshTokenEntity == null)
            {
                throw new NullReferenceException("اطلاعات ورود نامعتبر است");
            }

            var claims = new List<Claim> {
                new Claim(nameof(refreshTokenEntity.ApplicationUser.UserName), refreshTokenEntity.ApplicationUser.UserName),
                new Claim(nameof(refreshTokenEntity.ApplicationUser.PhoneNumber), refreshTokenEntity.ApplicationUser.PhoneNumber),
                new Claim(ClaimTypes.NameIdentifier, refreshTokenEntity.ApplicationUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (bussinessClaims != null)
            {
                claims.AddRange(bussinessClaims().Select(x => new Claim(x.Type, x.Value)));
            }


            var token = new JwtSecurityToken(config.ValidIssuer,
                config.ValidIssuer,
                claims,
                expires: DateTime.Now.AddSeconds(config.AccessTokenExpirationTimeSeconds),
                signingCredentials: credentials);


            var tokenRslt = new JwtSecurityTokenHandler().WriteToken(token);

            refreshTokenEntity.JwtId = token.Id;
            _context.Entry(refreshTokenEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return new TokenDto()
            {
                AccessToken = tokenRslt,
                RefreshToken = refreshTokenEntity.Token,
                AccessTokenExpiresInSeconds = config.AccessTokenExpirationTimeSeconds,
                RefresshTokenExpiresInSeconds = (refreshTokenEntity.ExpiryDate - DateTime.Now).Seconds,
                UserId = refreshTokenEntity.ApplicationUserId
            };


        }

        public string GetRandomOtp(int digits)
        {
            Random r = new Random();
            var x = r.Next((int)Math.Pow(10, digits - 1), (int)Math.Pow(10, digits) - 1);
            return x.ToString();
        }

        #region helper
        private string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<ApplicationUserEntity> GetUserAsync(UserDto user)
        {
            var rslt = await _userManager.Users.AsNoTracking()
                .Where(x => x.Id == user.Id)
                .SingleOrDefaultAsync();
            return rslt;
        }

        public async Task<Guid> GetUserIdAsync(string refreshToken)
        {
            var rslt = await _context.RefreshTokens
                .Where(x => x.Token == refreshToken)
                .Select(x => x.ApplicationUserId).SingleOrDefaultAsync();
            return rslt;
        }

        #endregion
    }
}
