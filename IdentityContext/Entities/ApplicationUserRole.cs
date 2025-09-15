using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityContext.Entities
{
    public class ApplicationUserRole : IdentityUserRole<Guid>
    {
        public virtual ApplicationUserEntity ApplicationUser { get; set; }
        public virtual ApplicationRoleEntity ApplicationRole { get; set; }
    }
}
