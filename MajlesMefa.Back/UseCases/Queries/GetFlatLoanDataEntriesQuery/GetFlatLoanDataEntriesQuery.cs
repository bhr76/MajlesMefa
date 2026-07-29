using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MediatR;

namespace MajlesMefa.Back.UseCases.Queries.GetFlatLoanDataEntriesQuery
{
    public class GetFlatLoanDataEntriesQuery : IRequest<TableModel<FlatDataEntryDto>>
    {
        public DataEntryTypeEnum DataEntryType { get; } = DataEntryTypeEnum.Loan;

        public string Title { get; set; }

        public Guid? CategoryId { get; set; }
        public Guid? SenatorId { get; set; }

        public List<Guid> Moavenats { get; set; }

        public bool IsBelongCurrentUser { get; set; }

        public bool IsNeedUserAction { get; set; }

        public Guid? RelatedSenatorId { get; set; }

        public Guid? DataEntryId { get; set; }
        public Guid? CurrentUserId { get; set; }

        public int? Year { get; set; }
        public TableRequestModel Filter { get; set; } = new TableRequestModel()
        {
            Skip = 0,
            Take = 10
        };
    }
}