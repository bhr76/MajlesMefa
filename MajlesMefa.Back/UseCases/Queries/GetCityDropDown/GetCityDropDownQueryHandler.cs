using MediatR;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Repositories;


namespace MajlesMefa.Back.UseCases.Queries.GetCityDropDown
{
    public class GetCityDropDownQueryHandler : IRequestHandler<GetCityDropDownQuery, List<GetCityDropDownDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCityDropDownQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<GetCityDropDownDto>> Handle(GetCityDropDownQuery request, CancellationToken cancellationToken)
        {
            var parent = new CityEntity();
            var query = _unitOfWork.CityRepository
                .NoTracking.AsQueryable();
            if (request.ParentId.HasValue)
            {
                parent = await _unitOfWork.CityRepository
              .NoTracking.Where(o => o.Id == request.ParentId).FirstOrDefaultAsync();
                query = query.Where(x => x.ParentCityId == request.ParentId);
            }
            else
            {
                query = query.Where(x => !x.ParentCityId.HasValue);
            }
            var result=await query.Select(o=>new GetCityDropDownDto
                {
                    Id = o.Id,
                    Name = o.Name,
                }).ToListAsync();
            if (!string.IsNullOrEmpty(parent.Name)) { result.Add(new GetCityDropDownDto { Id = parent.Id, Name = $"تمام شهرهای {parent.Name}" }); }

            return result;
        }
    }
}