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

namespace MajlesMefa.Back.UseCases.Queries.GetOrganizationsQuery
{
    public class GetOrganizationsQueryHandler : IRequestHandler<GetOrganizationsQuery, TableModel<OrganizationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOrganizationsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TableModel<OrganizationDto>> Handle(GetOrganizationsQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.OrganizationRepositroy
                .NoTracking
                .Where(x => x.ParentId == request.ParentId);
                

                if (request.Id.HasValue)
                {
                    query = query.Where(x => x.Id == request.Id);
                }

                var rslt = await query.Select(s => new OrganizationDto()
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
