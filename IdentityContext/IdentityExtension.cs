using CreamFramework.Infrastructure.Identity;
using IdentityContext.Describers;
using IdentityContext.Entities;
using IdentityContext.Services.Abstraction;
using IdentityContext.Services.Abstraction.External;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityContext
{
    public static class IdentityExtension
    {
        public static IServiceCollection AddCustomIdentity<TOptionService>(this IServiceCollection services, IConfiguration configuration,  string connectionString) where TOptionService : class, IOptionService
        {
            services.AddSingleton<IOptionService, TOptionService>();
            services.AddScoped<IUserManagerService, UserManagerService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IRoleManagerService, RoleManagerService>();

            services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(
                configuration.GetConnectionString(connectionString),
                   x => x.EnableRetryOnFailure()));
            services.AddIdentity<ApplicationUserEntity, ApplicationRoleEntity>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = false;
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
            })
                 .AddEntityFrameworkStores<AuthDbContext>()
                 .AddRoles<ApplicationRoleEntity>()
                  .AddErrorDescriber<PersianIdentityErrorDescriber>()
                 .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 0;
                options.Lockout.AllowedForNewUsers = false;
            });
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUserEntity>, UserClaimsPrincipalFactory<ApplicationUserEntity>>();

            return services;
        }
    }
}
