using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Dtos.Common;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MajlesMefa.Back.Utilities.EnumHelper;
using MediatR;

namespace MajlesMefa.Back.UseCases.Queries.GetSenatorBudgetQuery
{
    public class GetSenatorBudgetQueryHandler : IRequestHandler<Queries.GetSenatorBudgetQuery.GetSenatorBudgetQuery, TableModel<SenatorBudgetDto>>
    {
        private readonly RefahMajlesDbContext _context;

        public GetSenatorBudgetQueryHandler(RefahMajlesDbContext context)
        {
            _context = context;
        }

        public async Task<TableModel<SenatorBudgetDto>> Handle(Queries.GetSenatorBudgetQuery.GetSenatorBudgetQuery request, CancellationToken   cancellationToken)
        {
            var query = await _context.SenatorBudgets
                .Select(b => new SenatorBudgetDto()
                {

                    Id = b.Id,
                    Amount = b.Amount,
                    ExecDate = b.ExecDate,
                    SenatorRequestLoanType = b.LoanType,
                    SenatorId = b.SenatorId,
                    UserId = b.UserId,
                    SenatorName = b.Senator.Name , 
                    UserName = b.User.Name,
                    LoanTypeName = b.LoanType.GetDisplayName(),
                }).ToTableResultAsync(request.Filter);
            return query;
        }
    }
}
