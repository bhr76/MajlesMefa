using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityContext.Entities
{
    public class ApplicationRoleEntity : IdentityRole<Guid>
    {
        [StringLength(64)]
        public string FaName { get; set; }
    }
}
