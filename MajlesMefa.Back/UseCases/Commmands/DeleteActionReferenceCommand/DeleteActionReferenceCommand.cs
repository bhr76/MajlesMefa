using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.DeleteActionReferenceCommand
{
    public class DeleteActionReferenceCommand: IRequest
    {
        public Guid Id { get; set; }
    }
}
