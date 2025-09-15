using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Dtos.Common.Grid
{
    public class SenatorDto
    {

        public Guid UserId { get; set; }
        public Guid Id => UserId;

        public string Name { get; set; }

        public string Mobile { get; set; }
        
        public string HozeEntekhabi { get; set; }
        
        public string City { get; set; }
    }
}
