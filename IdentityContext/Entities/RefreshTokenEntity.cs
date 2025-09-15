using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityContext.Entities
{
    public class RefreshTokenEntity
    {
        public long Id { get; set; }

        public Guid ApplicationUserId { get; set; }

        [StringLength(2048)]
        public string Token { get; set; }

        [StringLength(2048)] 
        public string JwtId { get; set; }

        public bool IsUsed { get; set; }

        public bool IsRevoked { get; set; }

        public DateTime AddedDate { get; set; }

        public DateTime ExpiryDate { get; set; }

        public ApplicationUserEntity ApplicationUser { get; set; }
    }
}
