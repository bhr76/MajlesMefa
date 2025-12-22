using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MajlesMefa.Back.Utilities.Mapping
{
    public static class MapperExtension
    {
        public static IServiceCollection AddMapper(this IServiceCollection services, Assembly assembly = null)
        {
            var targetAssembly = assembly ?? Assembly.GetCallingAssembly();

            services.AddAutoMapper(config =>
            {
                config.AddProfile(new MappingProfile(targetAssembly));
            }, targetAssembly);

            return services;
        }
    }
}
