using MediatR;
using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;

namespace MajlesMefa.Back.UseCases.Queries.GetKhadamatQuery
{
    public class GetKhadamatDataQuery :IRequest<TableModel<MokatebeDto>>
    {
        public DataEntryTypeEnum DataEntryType { get; set; }

        public string Title { get; set; }

        public Guid? SenatorIdId { get; set; }

        public bool IsBelongCurrentUser { get; set; }


        public Guid? DataEntryId { get; set; }

        public TableRequestModel Filter { get; set; } = new TableRequestModel()
        {
            Skip = 0,
            Take = 10
        };

    }
}
