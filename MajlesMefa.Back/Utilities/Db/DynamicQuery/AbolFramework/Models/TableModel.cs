using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models
{
    public class TableModel<TOutput>
    {
        public List<TOutput> Items { get; set; }

        public int Count { get; set; }

        public object Aggregates { get; set; }

        //public DataSourceResult
    }
}
