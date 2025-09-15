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
    public class TahghighTafahosRepository : BaseRepository<TahghighTafahosEntity>, ITahghighTafahosRepository
    {
        public TahghighTafahosRepository(RefahMajlesDbContext context) : base(context)
        {
        }

        public async Task AddSenatorAsync(TahghighTafahosEntity tahghighTafahos, Guid senatorId)
        {
            await _context.TahghighTafahosSenators.AddAsync(new TahghighTafahosSenatorEntity()
            {
                TahghighTafahos = tahghighTafahos,
                SenatorId = senatorId
            });
        }

        public async Task RemoveSenatorAsync(Guid tahghighTafahosId, Guid senatorId)
        {

            var entity = await _context.TahghighTafahosSenators.SingleOrDefaultAsync(x => x.TahghighTafahosId == tahghighTafahosId && x.SenatorId == senatorId);
            _context.TahghighTafahosSenators.Remove(entity);
        }
    }
}
