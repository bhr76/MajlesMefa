using MediatR;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Repositories;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetCategoriesQuery
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, TableModel<CategoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCategoriesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TableModel<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            var query =  _unitOfWork.CategoryRepository
                .NoTracking
                .Where(x => x.ParentId == request.ParentId)
                
                .Where(x => x.DataEntryType == request.DataEntryType);

                if (request.Id.HasValue)
                {
                    query = query.Where(x => x.Id == request.Id);
                }

                var rslt = await query.Select(s => new CategoryDto()
                {
                    Id = s.Id,
                    Name = s.Name,
                    IsCentralOffice = s.IsCentralOffice,
                    //SubCategoriesCount = s.Categories.Count + s.Categories.Sum(ss => ss.Categories.Count),
                })
                .ToTableResultAsync(request.Filter);
            return rslt;
        }
    }
}
