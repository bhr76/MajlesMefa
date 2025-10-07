using MediatR;
using System;

namespace MajlesMefa.Back.UseCases.Commmands.DeleteSenatorBudgetCommand
{
    public class DeleteSenatorBudgetCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}