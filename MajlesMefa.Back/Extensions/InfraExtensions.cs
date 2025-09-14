using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MajlesMefa.Back.Services.Abstractioin;
using MajlesMefa.Back.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Extensions
{
    public static class InfraExtensions
    {
        public static IServiceCollection AddCurrentUserService(this IServiceCollection services)
        {
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            return services;

        }
    }
}
