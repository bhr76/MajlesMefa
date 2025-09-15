using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Utilities.Db.Pagination
{
    public interface IDatedEntity
    {
        public DateTime Created { get; set; }
    }
}
