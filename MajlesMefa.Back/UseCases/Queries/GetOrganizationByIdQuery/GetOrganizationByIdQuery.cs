using MediatR;
using MajlesMefa.Back.Dtos;

namespace MajlesMefa.Back.UseCases.Queries.GetOrganizationsQuery
{
    public class GetOrganizationByIdQuery: IRequest<OrganizationDto>
    {
        
        public Guid Id { get; set; }
    }
}
