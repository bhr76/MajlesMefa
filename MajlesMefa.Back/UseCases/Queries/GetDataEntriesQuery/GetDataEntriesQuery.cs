using MediatR;
using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;

namespace MajlesMefa.Back.UseCases.Queries.GetDataEntriesQuery
{

    /// <summary>
    /// var json = DataSourceResult.GetFromTable(list);
    /// </summary>
    public class GetDataEntriesQuery : IRequest<TableModel<DataEntryDto>>
    {
        public DataEntryTypeEnum DataEntryType { get; set; }

        public string Title { get; set; }

        public Guid? CategoryId { get; set; }
        public Guid? SenatorIdId { get; set; }

        public List<Guid> Moavenats { get; set; }

        public bool IsBelongCurrentUser { get; set; }

        public bool IsNeedUserAction { get; set; }

        public Guid? RelatedSenatorId { get; set; }

        public Guid? DataEntryId { get; set; }
        public Guid? CurrentUserId { get; set; }

        public TableRequestModel Filter {get;set;} = new TableRequestModel()
        {
            Skip = 0,
            Take = 10
        };
    }

    //input: string models
    //var Filter = JsonConvert.DeserializeObject<TableRequestModel>(models);
    //create 
    //var list =  mediator.ExecuteAsync(request);
    //var rslt = DataSourceResult.GetFromTable(list);
    //return Json(rslt);



}
