using MajlesMefa.Back.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Repositories.Abstraction
{
    public interface ICityRepository:IRepository<CityEntity>
    {
        Task<bool> HasUserAsync(Guid cityId);
    }
}
