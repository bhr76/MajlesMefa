using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.DeleteUseCommand
{
    public class DeleteUseCommand: IRequest
    {
        public Guid UserId { get; set; }
    }
}
