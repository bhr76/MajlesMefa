using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.IdentityModel.Tokens;
using MajlesMefa.Back.Dtos.Common;
using MajlesMefa.Back.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Utilities
{
    public static class DbContextExtension
    {
        public static void RegisterConfigurations(this Type TContextType, ModelBuilder modelBuilder, params Assembly[] assemblies) 
        {
            var context = (DbContext)Activator.CreateInstance(TContextType, new DbContextOptions<RefahMajlesDbContext>());
            var dbContexType = TContextType;
            MethodInfo applyGenericMethod = typeof(ModelBuilder).GetMethods().First(m => m.Name == nameof(ModelBuilder.ApplyConfiguration));

            IEnumerable<Type> types = assemblies.SelectMany(a => a.GetExportedTypes())
                .Where(c => c.IsClass && !c.IsAbstract && c.IsPublic);

            types = types.Where(x => context.Model.FindEntityType(x.FullName) != null);

            foreach (Type type in types)
            {

                foreach (Type iface in type.GetInterfaces())
                {
                    if (iface.IsConstructedGenericType && iface.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                    {
                        MethodInfo applyConcreteMethod = applyGenericMethod.MakeGenericMethod(iface.GenericTypeArguments[0]);
                        applyConcreteMethod.Invoke(modelBuilder, new object[] { Activator.CreateInstance(type) });
                    }
                }
            }
        }

        public static void AddCurrentUserData(this ChangeTracker changeTracker, CurrentUserDto currentUser)
        {
            foreach (var entry in changeTracker.Entries<IAuditEntity>())
            {
                if (entry.State == EntityState.Added &&
                    entry.Entity.Created == DateTime.MinValue &&
                    entry.Entity.CreatorUserId == Guid.Empty)
                {
                    entry.Entity.Created = DateTime.Now;
                    entry.Entity.CreatorUserId = currentUser.UserId;
                }
            }
        }
    
    }
}
