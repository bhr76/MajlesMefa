using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MajlesMefa.Back.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Extensions
{
    public static class DbContextExtension
    {
        public static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RefahMajlesDbContext>(options => {

                options.UseSqlServer(configuration.GetConnectionString("AppDb"),
                   x => x.EnableRetryOnFailure());
                options.EnableSensitiveDataLogging();
            });
            return services;
        }

        public static void AddBaseConfig<TEntity>(this EntityTypeBuilder<TEntity> builder) where TEntity : class, IAuditEntity
        {
            builder.Property(x => x.Created)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("getdate()");
            builder.HasIndex(x => x.Created);
        }

    }
}
