using MediatR;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.UseCases.Commmands.SeedCitiesCommand;
using MajlesMefa.Back.UseCases.Commmands.SeedPageRoleCommand;
using MajlesMefa.Back.UseCases.Commmands.SeedRolesAndAdminsCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Seeder
{
    public static class PageRoleSeeder
    {
        public static async Task AddPagesWithRoleAccessAsync(this IServiceProvider mainServices, Assembly assembly)
        {
            using var scope = mainServices.CreateScope();
            var services = scope.ServiceProvider;
            try
            {
                var refahMajlesContext = services.GetRequiredService<RefahMajlesDbContext>();
                //var hasPageRoles = await refahMajlesContext.PageRoles.AnyAsync();
                var hasPageRoles = await refahMajlesContext.PageRoles.CountAsync() > 10;
                if (!hasPageRoles)
                {
                    var mediator = services.GetRequiredService<IMediator>();
                    var server = mainServices.GetService<IServer>();
                    var baseUrl = "";
                    await mediator.Send(new SeedPageRoleCommand() { Assembly = assembly, BaseUrl = baseUrl });
                }
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<SeedPageRoleCommand>>();
                logger.LogError(ex, "An error occurred while migrating or initializing the database.");
            }
        }

    }
}
