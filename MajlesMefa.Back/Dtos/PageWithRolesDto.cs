using MajlesMefa.Back.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Dtos
{
    public class PageWithRolesDto
    {
        public Guid Id { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string Url { get; set; }

        public string Title { get; set; }

        public List<RoleTypeEnum> Roles { get; set; }

        public string RolesStr => string.Join(" - ", Roles.Select(i => i .GetPersianName() ));
    }
}
