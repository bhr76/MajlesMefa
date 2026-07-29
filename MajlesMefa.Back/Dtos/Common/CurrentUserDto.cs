using IdentityContext.Dtos;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Dtos.Common
{
    public class CurrentUserDto
    {
        public Guid BussinessUserId { get; set; }

        public Guid UserId { get; set; }
        
        public string IpAddress { get; set; }

        public string Name { get; set; }
        public string UserName { get; set; }

        public List<RoleTypeEnum> Roles { get; set; } = new List<RoleTypeEnum>();

        public List<string> RoleNames { get; set; } = new List<string>();

        public OrganizationDto Organization { get; set; } = new OrganizationDto();

        public CityDto City { get; set; } = new CityDto();
    }
}
