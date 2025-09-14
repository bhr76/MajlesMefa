using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.DeleteMokatebePeygiryCommand
{
    public class DeleteMokatebePeygiryCommand: IRequest
    {
        public long Id { get; set; }
    }
}
