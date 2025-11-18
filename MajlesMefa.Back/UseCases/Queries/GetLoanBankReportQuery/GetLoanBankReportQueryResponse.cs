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
        public Int64 TotalAmount{ get; set; }
        public int TotalCount{ get; set; }

        public Int64 PaidAmount { get; set; }
        public int PaidCount { get; set; }

        public Int64 UnPaidAmount { get; set; }
        public int UnPaidCount { get; set; }

        public Int64 InBranchAmount { get; set; }
        public int InBranchCount { get; set; }

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
