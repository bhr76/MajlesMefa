using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Utilities.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile(Assembly assembly)
        {
            if (assembly == null)
            {
                assembly = Assembly.GetExecutingAssembly();
            }
            ApplyMappingsFromAssembly(assembly);
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var allTypesInAllAssemblies = GetAllTypes(assembly);

            var types = allTypesInAllAssemblies
                .Where(t => t.GetInterfaces().Any(i =>
                    i == typeof(IMapping)))
                .ToList();

            foreach (var type in types)
            {
                if (type.IsGenericType)
                {
                    continue;
                }
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }

        private List<Type> GetAllTypes(Assembly assembly)
        {
            var hasChilds = assembly.GetReferencedAssemblies().Any();
            var thisTypes = assembly.GetExportedTypes().ToList();
            if (hasChilds)
            {
                var subs = assembly.GetReferencedAssemblies()
                    .Select(Assembly.Load)
                    .Where(x => x.GetExportedTypes()
                        .Any(exp => exp.GetInterfaces()
                        .Any(i => i == typeof(IMapping)))
                    );
                foreach (var s in subs)
                {
                    thisTypes.AddRange(GetAllTypes(s));
                }
            }
            return thisTypes;
        }
    }

}
