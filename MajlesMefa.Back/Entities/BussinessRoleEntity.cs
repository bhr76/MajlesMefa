using MajlesMefa.Back.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Entities
{
    public class BussinessRoleEntity
    {
        public Guid Id { get; set; }

        public RoleTypeEnum RoleType { get; set; }

        public virtual ICollection<PageRoleEntity> PageRoles { get; set; } = new HashSet<PageRoleEntity>();
        
        public virtual ICollection<UserRoleEntity> UserRoles { get; set; } = new HashSet<UserRoleEntity>();
    }
}
