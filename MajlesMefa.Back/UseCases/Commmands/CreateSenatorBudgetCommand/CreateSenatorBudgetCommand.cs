using MediatR;
using System;
using MajlesMefa.Back.Enums.Senator;

namespace MajlesMefa.Back.UseCases.Commmands.CreateSenatorBudgetCommand
{
    public class CreateSenatorBudgetCommand : IRequest<Guid>
    {
        public Guid SenatorId { get; set; }
        public Guid UserId { get; set; }
        public long Amount { get; set; }
        public string ExecDate { get; set; }
        public SenatorRequestLoanTypeEnum LoanType { get; set; }
    }
}