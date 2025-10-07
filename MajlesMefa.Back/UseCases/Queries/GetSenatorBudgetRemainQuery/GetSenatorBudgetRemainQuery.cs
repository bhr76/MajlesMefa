using MajlesMefa.Back.Dtos;
using MediatR;

namespace MajlesMefa.Back.UseCases.Queries.GetSenatorBudgetRemainQuery
{

    public class GetSenatorBudgetRemainQuery: IRequest<GetSenatorBudgetRemainResponse>
    {
        public Guid SenatorId { get; set; }
        public Guid UserId { get; set; }
    }
}
