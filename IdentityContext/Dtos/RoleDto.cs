using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityContext.Dtos
{
    public class RoleDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string FaName { get; set; }

        public string NormalizedName { get; set; }


        public RoleDto() { }
        public RoleDto(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
        public static RoleDto Create(string name)
        {
            return new RoleDto
            {
                Name = name,
            };
        }

        public static RoleDto Update(Guid id, string title)
        {
            return new RoleDto
            {
                Id = id,
                Name = title
            };
        }
        public void SetId(Guid id)
        {
            Id = id;
        }
    }
}
