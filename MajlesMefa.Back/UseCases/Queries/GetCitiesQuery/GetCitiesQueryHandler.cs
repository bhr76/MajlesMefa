using MediatR;
using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Repositories;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetCitiesQuery
{
    public class GetCitiesQueryHandler : IRequestHandler<GetCitiesQuery, TableModel<CityDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCitiesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TableModel<CityDto>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.CityRepository
                .NoTracking
                .Where(x => x.ParentCityId == request.ParentId);

                if (request.Id.HasValue)
                {
                    query = query.Where(x => x.Id == request.Id);
                }

                var rslt = await query.Select(s => new CityDto()
                {
                    Id = s.Id,
                    Name = s.Name,
                    //SubCategoriesCount = s.Categories.Count + s.Categories.Sum(ss => ss.Categories.Count),
                })
                .ToTableResultAsync(request.Filter);
            return rslt;
        }
    }
}
