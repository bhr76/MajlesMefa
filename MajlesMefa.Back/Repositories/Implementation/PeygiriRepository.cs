using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using MajlesMefa.Back.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Repositories.Implementation
{
    public class PeygiriRepository : BaseRepository<PeygiriEntity>, IPeygiriRepository
    {
        public PeygiriRepository(RefahMajlesDbContext context) : base(context)
        {
        }
    }
}
