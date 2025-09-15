using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Repositories.Implementation
{
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        public UserRepository(RefahMajlesDbContext context) : base(context)
        {
        }

        public void AddToRole(UserRoleEntity userRole)
        {
            _context.UserRoles.Add(userRole);
        }
    }
}
