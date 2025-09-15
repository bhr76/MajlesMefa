using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Entities
{
    public abstract class AuditEntity<TId>: IAuditEntity
    {
        public TId Id { get; set; }

        public DateTime Created { get; set; }
        
        public Guid CreatorUserId { get; set; }
    }
}