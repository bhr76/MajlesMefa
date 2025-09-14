using MajlesMefa.Back.Utilities.Db.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Entities
{
    public interface IAuditEntity: IDatedEntity
    {
        public DateTime Created { get; set; }

        public Guid CreatorUserId { get; set; }
    }
}
