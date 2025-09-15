using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Dtos
{
    public class CityDto
    {
        public string Name { get; set; }

        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        public string ParentName { get; set; }
    }
}
