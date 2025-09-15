using MediatR;
using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;

namespace MajlesMefa.Back.UseCases.Queries.GetOrganizationsQuery
{
    public class GetOrganizationsQuery: IRequest<TableModel<OrganizationDto>>
    {
        public Guid? ParentId { get; set; }
        
        public Guid? Id { get; set; }


        public TableRequestModel Filter { get; set; } = new TableRequestModel();
    }
}
