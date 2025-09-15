using MediatR;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Repositories;

namespace MajlesMefa.Back.UseCases.Queries.GetCitiesQuery
{
    public class GetCityByIdQueryHandler : IRequestHandler<GetCityByIdQuery, CityDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCityByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CityDto> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.CityRepository
                .NoTracking
                .Where(x => x.Id == request.Id);

                var rslt = await query.Select(s => new CityDto()
                {
                    Id = s.Id,
                    Name = s.Name,
                })
                .SingleOrDefaultAsync(cancellationToken);
            return rslt;
        }
    }
}
