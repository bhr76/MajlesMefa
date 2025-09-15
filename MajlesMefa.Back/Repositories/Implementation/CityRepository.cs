using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Repositories.Implementation
{
    public class CityRepository : BaseRepository<CityEntity>, ICityRepository
    {
        public CityRepository(RefahMajlesDbContext context) : base(context)
        {
        }

        public async Task<bool> HasUserAsync(Guid cityId)
        {
            var rslt = await _context.Cities.Where(x => x.Id == cityId)
                .AnyAsync(x => x.Users.Any());
            return rslt;
        }
    }
}
