using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IdentityContext.Entities
{
    public class ApplicationUserEntity : IdentityUser<Guid>
    {
        public bool ChangePassword { get; set; } = false;

        [StringLength(8)]
        public string OtpCode { get; set; }

        public DateTime OtpExpiersOn { get; set; } = DateTime.Now.AddSeconds(180);

        public ICollection<RefreshTokenEntity> RefreshTokens { get; set; } = new HashSet<RefreshTokenEntity>();

    }
}
