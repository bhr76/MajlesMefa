using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Repositories.Abstraction; // changed from .Implementation
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetSenatorBudgetRemainQuery
{
    public class GetSenatorBudgetRemainQueryHandler : IRequestHandler<GetSenatorBudgetRemainQuery, GetSenatorBudgetRemainResponse>
    {
        private readonly RefahMajlesDbContext _context;
        private readonly ISenatorBudgetRepository _budgetRepository; // use interface

        public GetSenatorBudgetRemainQueryHandler(
            RefahMajlesDbContext context,
            ISenatorBudgetRepository budgetRepository) // use interface
        {
            _context = context;
            _budgetRepository = budgetRepository;
        }

        public async Task<GetSenatorBudgetRemainResponse> Handle(GetSenatorBudgetRemainQuery request, CancellationToken cancellationToken)
        {
            var isDefine = await _budgetRepository.HasDefineBudget(request.SenatorId, request.UserId);
            if (!isDefine)
            {
                return new GetSenatorBudgetRemainResponse
                {
                    IsDefined = false,
                    RemainAmount = 0,
                    Dsc = "بودجه مربوط به این نماینده در این بانک مشخص نشده است!"
                };
            }

            var defineBudget = await _budgetRepository.GetDefineBudget(request.SenatorId, request.UserId);

            var usedBudget = await _context.Loans
                .Where(x => x.DataEntry.SenatorId == request.SenatorId && x.UserId == request.UserId)
                .SumAsync(x => (long?)x.Amount, cancellationToken) ?? 0L;

            if (defineBudget.Amount - usedBudget < 0)
            {
                return new GetSenatorBudgetRemainResponse
                {
                    IsDefined = false,
                    RemainAmount = 0,
                    Dsc = "مقدار وارد شده بیشتر از بودجه است!"
                };
            }

            return new GetSenatorBudgetRemainResponse
            {
                SenatorBudgetId = defineBudget.Id,
                IsDefined = true,
                RemainAmount = defineBudget.Amount - usedBudget
            };
        }
    }
}