using MediatR;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MajlesMefa.Back.UseCases.Queries.GetOrganizationDropDown
{
    public class GetOrganizationDropDownQueryHandler : IRequestHandler<GetOrganizationDropDownQuery, List<GetOrganizationDropDownDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOrganizationDropDownQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<GetOrganizationDropDownDto>> Handle(GetOrganizationDropDownQuery request, CancellationToken cancellationToken)
        {
            var parent = new OrganizationEntity();
            var query=  _unitOfWork.OrganizationRepositroy
                .NoTracking.AsQueryable();
            if (request.ParentId.HasValue)
            {
                parent = await _unitOfWork.OrganizationRepositroy
              .NoTracking.Where(o => o.Id == request.ParentId).FirstOrDefaultAsync();
                query = query.Where(x => x.ParentId == request.ParentId);
            }
            else
            {
                query = query.Where(x => !x.ParentId.HasValue);
            }
           
            var result =await query.Select(o => new GetOrganizationDropDownDto
            {
                    Id = o.Id,
                    Name = o.Name,
                }).ToListAsync();
            if (!string.IsNullOrEmpty(parent.Name)) { result.Add(new GetOrganizationDropDownDto { Id = parent.Id, Name = $" تمام زیرمجموعه های {parent.Name}" }); }
           
            return result;
        }
    }
}