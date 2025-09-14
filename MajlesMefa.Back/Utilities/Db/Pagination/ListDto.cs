using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Utilities.Db.Pagination
{
    public class ListDto<TEntity>
    {
        public int Count { get; set; }

        public List<TEntity> Items { get; set; } = new List<TEntity>();
    }
}
