using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Repositories.Abstraction
{
    public interface ITahghighTafahosRepository : IRepository<TahghighTafahosEntity>
    {
        Task RemoveSenatorAsync(Guid tahghighTafahosId, Guid senatorId);


        Task AddSenatorAsync(TahghighTafahosEntity tahghighTafahos, Guid senatorId);
    }
}
