using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.DeleteOrganizationCommand
{
    public class DeleteOrganizationCommand: IRequest
    {
        public Guid Id { get; set; }
    }
}
