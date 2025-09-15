using IdentityContext.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.UseCases.Commmands.SeedCitiesCommand;
using MajlesMefa.Back.UseCases.Commmands.SeedRolesAndAdminsCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Seeder
{
    public static class CitySeeder
    {
        public static async Task AddCitiesSeed(this IServiceProvider mainServices)
        {
            using var scope = mainServices.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var refahMajlesContext = services.GetRequiredService<RefahMajlesDbContext>();

                //var hasCities = await refahMajlesContext.Cities.AnyAsync();
                var hasCities = await refahMajlesContext.Cities.CountAsync() > 10;
                if (!hasCities)
                {
                    var mediator = services.GetRequiredService<IMediator>();
                    await mediator.Send(new SeedCitiesCommand());
                }
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedCitiesCommand>>();
                logger.LogError(ex, "An error occurred while migrating or initializing the database.");
            }
        }


    }
}
