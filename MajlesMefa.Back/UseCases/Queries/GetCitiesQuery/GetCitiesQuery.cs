using MediatR;
using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;

namespace MajlesMefa.Back.UseCases.Queries.GetCitiesQuery
{
    public class GetCitiesQuery: IRequest<TableModel<CityDto>>
    {
        public Guid? ParentId { get; set; }
        
        public Guid? Id { get; set; }


        public TableRequestModel Filter { get; set; } = new TableRequestModel();
    }
}
