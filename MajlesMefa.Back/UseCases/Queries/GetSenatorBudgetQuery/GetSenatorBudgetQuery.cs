using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Dtos.Common;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MediatR;

namespace MajlesMefa.Back.UseCases.Queries.GetSenatorBudgetQuery
{

    public class GetSenatorBudgetQuery: IRequest<TableModel<SenatorBudgetDto>>
    {
        public TableRequestModel Filter { get; set; } = new TableRequestModel();
    }
}
