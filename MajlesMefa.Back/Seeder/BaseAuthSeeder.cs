using IdentityContext.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.UseCases.Commmands.SeedRolesAndAdminsCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Seeder
{
    public static class BaseAuthSeeder
    {
        public static async Task AddBaseUserSeed(this IServiceProvider mainServices)
        {
            using var scope = mainServices.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var identityContext = services.GetRequiredService<AuthDbContext>();
                identityContext.Database.Migrate();

                var creamContext = services.GetRequiredService<RefahMajlesDbContext>();
                creamContext.Database.SetCommandTimeout(180);
                creamContext.Database.Migrate();

                var hasUser = await identityContext.Users.AnyAsync();
                if (!hasUser)
                {
                    var mediator = services.GetRequiredService<IMediator>();
                    await mediator.Send(new SeedRolesAndAdminsCommand());
                }
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedRolesAndAdminsCommand>>();
                logger.LogError(ex, "An error occurred while migrating or initializing the database.");
            }
        }
    }
}
