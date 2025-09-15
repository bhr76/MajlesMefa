using MediatR;
using Microsoft.EntityFrameworkCore;
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
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCategoryByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var query =  _unitOfWork.CategoryRepository
                .NoTracking
                .Where(x => x.Id == request.Id)
                .Where(x => x.DataEntryType == request.DataEntryType);
                var rslt = await query.Select(s => new CategoryDto()
                {
                    Id = s.Id,
                    Name = s.Name,
                    IsCentralOffice = s.IsCentralOffice
                })
                .SingleOrDefaultAsync(cancellationToken);
            return rslt;
        }
    }
}
