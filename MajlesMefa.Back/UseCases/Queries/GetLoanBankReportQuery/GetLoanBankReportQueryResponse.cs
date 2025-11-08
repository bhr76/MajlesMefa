using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MediatR;

namespace MajlesMefa.Back.UseCases.Queries.GetLoanBankReportQuery
{

    /// <summary>
    /// var json = DataSourceResult.GetFromTable(list);
    /// </summary>
    public class GetLoanBankReportQueryResponse
    {
        public Int64 Amount{ get; set; }
        public int count{ get; set; }
        public LoanTypeEnum LoanType { get; set; }
        public Guid CurrentUserId { get; set; }
        public string BankFullName { get; set; }
        public ResponseStatusEnum ResponseStatus { get; set; }
        public TableRequestModel Filter {get;set;} = new TableRequestModel()
        {
            Skip = 0,
            Take = 10
        };
    }
}
