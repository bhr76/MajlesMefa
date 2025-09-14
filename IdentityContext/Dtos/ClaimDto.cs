using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityContext.Dtos
{
    public class ClaimDto
    {
        public string Type { get; set; }
        public string Value { get; set; }

        public ClaimDto(string type, string value)
        {
            Type = type;
            Value = value;
        }
    }
}
