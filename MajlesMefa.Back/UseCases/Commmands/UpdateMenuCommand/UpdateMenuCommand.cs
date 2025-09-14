using MediatR;
using MajlesMefa.Back.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.UpdateMenuCommand
{
    public class UpdateMenuCommand: IRequest
    {
        public Guid PageId { get; set; }

        public List<RoleTypeEnum> Roles { get; set; }
    }
}
