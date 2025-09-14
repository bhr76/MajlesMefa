using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityContext.Dtos
{
    public class UpdateUserDto
    {
        public string Email { get; set; }

        public Guid Id { get; set; }

        public string PhoneNumber { get; set; }

        public string Name { get; set; }
        public string UserName { get; set; }

        public List<ManageUserRolesDto> ManageUserRoles { get; set; } 
    }
}
