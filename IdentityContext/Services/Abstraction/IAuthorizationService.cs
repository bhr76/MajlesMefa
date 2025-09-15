using IdentityContext.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityContext.Services.Abstraction
{
    public interface IAuthorizationService
    {
        (string Token, int ExpireIn) GenerateAccessToken(ApplicationUserEntity user);

        Task<ApplicationUserEntity> GetUserAsync(string mobile);

    }
}
