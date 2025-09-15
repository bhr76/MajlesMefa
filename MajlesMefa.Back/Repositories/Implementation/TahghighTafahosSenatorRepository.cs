using Microsoft.EntityFrameworkCore;
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
    public class TahghighTafahosSenatorRepository : BaseRepository<TahghighTafahosSenatorEntity>, ITahghighTafahosSenatorRepository
    {
        public TahghighTafahosSenatorRepository(RefahMajlesDbContext context) : base(context)
        {
        }


    }
}
