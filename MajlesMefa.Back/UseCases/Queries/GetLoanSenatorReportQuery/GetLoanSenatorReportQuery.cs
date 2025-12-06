using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MediatR;

namespace MajlesMefa.Back.UseCases.Queries.GetLoanSenatorReportQuery
{

    /// <summary>
    /// var json = DataSourceResult.GetFromTable(list);
    /// </summary>
    public class GetLoanSenatorReportQuery : IRequest<TableModel<DataEntryDto>>
    {
        public DataEntryTypeEnum DataEntryType { get; set; }
        public string Title { get; set; }
        public Guid? UserId { get; set; }
        public Guid? DataEntryId { get; set; }
        public Guid? CurrentUserId { get; set; }
        public List<ResponseStatusEnum> ResponseStatuses { get; set; }
        public string SenatorName { get; set; }
        public LoanTypeEnum? LoanType { get; set; }

        public ResponseStatusEnum ResponseStatus { get; set; }
        public TableRequestModel Filter {get;set;} = new TableRequestModel()
        {
            Skip = 0,
            Take = 10
        };
    }
}
