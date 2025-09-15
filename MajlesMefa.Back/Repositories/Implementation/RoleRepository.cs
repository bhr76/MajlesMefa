using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Repositories.Implementation
{
    public class RoleRepository : BaseRepository<BussinessRoleEntity>, IRoleRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoleRepository(RefahMajlesDbContext context) : base(context)
        {
        }
    }
}
