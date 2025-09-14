using MediatR;
using MajlesMefa.Back.Dtos;

namespace MajlesMefa.Back.UseCases.Queries.GetCitiesQuery
{
    public class GetCityByIdQuery: IRequest<CityDto>
    {
        
        public Guid Id { get; set; }
    }
}
