using IdentityContext.Entities;
using IdentityContext.Models.Settings;
using IdentityContext.Services.Abstraction;
using IdentityContext.Services.Abstraction.External;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CreamFramework.Infrastructure.Identity
{
    public class AuthorizationService: IAuthorizationService
    {
        private readonly IOptionService _optionService;
        private readonly AuthDbContext _context;

        public AuthorizationService(AuthDbContext context, IOptionService optionservice)
        {
            _context = context;
            _optionService = optionservice;
        }

        public AuthorizationService()
        {
        }

        public async Task<ApplicationUserEntity> GetUserAsync(string mobile)
        {
            var rslt = await _context.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(u => u.PhoneNumber == mobile);
            return rslt;
        }

        public (string Token, int ExpireIn) GenerateAccessToken(ApplicationUserEntity user)
        {
            var config = _optionService.GetConfig<JwtSetting>();

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Secret));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(nameof(user.UserName), user.UserName),
                new Claim(nameof(user.SecurityStamp), user.SecurityStamp),
                //new Claim(nameof(user.PhoneNumber), user.PhoneNumber),
                new Claim(nameof(user.NormalizedUserName), user.NormalizedUserName),
                //new Claim(nameof(user.NormalizedEmail), user.NormalizedEmail),
                //new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(config.ValidIssuer,
                config.ValidIssuer,
                claims,
                expires: DateTime.Now.AddSeconds(config.AccessTokenExpirationTimeSeconds),
                signingCredentials: credentials);

            var tokenRslt =  new JwtSecurityTokenHandler().WriteToken(token);
            return (tokenRslt, config.AccessTokenExpirationTimeSeconds);
        }
    }
}
