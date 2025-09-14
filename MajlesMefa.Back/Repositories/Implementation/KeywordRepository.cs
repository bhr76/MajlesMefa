using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Repositories.Implementation
{
    public class KeywordRepository : BaseRepository<KeywordEntity>, IKeywordRepository
    {
        public KeywordRepository(RefahMajlesDbContext context) : base(context)
        {
        }
    }
}
