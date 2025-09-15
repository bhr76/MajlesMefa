using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityContext.Entities
{
    public class AuthDbContext : IdentityDbContext<ApplicationUserEntity, ApplicationRoleEntity, Guid, IdentityUserClaim<Guid>,
        ApplicationUserRole, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {

        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }

        public AuthDbContext(DbContextOptions<AuthDbContext> options)
              : base(options)
        {
        }
    }
}
