using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Commmands.DeleteCategoryCommand
{
    public class DeleteCategoryCommand: IRequest
    {
        public Guid Id { get; set; }
    }
}
