using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Dtos
{
    public class KeywordDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int RepeatCount { get; set; }
    }
}
