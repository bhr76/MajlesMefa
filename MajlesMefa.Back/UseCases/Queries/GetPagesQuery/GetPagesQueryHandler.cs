using MediatR;
using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Dtos.UserDtos;
using MajlesMefa.Back.Repositories;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetPagesQuery
{
    public class GetPagesQueryHandler : IRequestHandler<GetPagesQuery, TableModel<PageWithRolesDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPagesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TableModel<PageWithRolesDto>> Handle(GetPagesQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.PageRepository
                .NoTracking;
            if(request.Id.HasValue)
            {
                query = query
                    .Where(x => x.Id == request.Id);
            }

            var rslt = await query 
                .Select(x => new PageWithRolesDto()
                {
                    Id = x.Id,
                    Action = x.Action,
                    Controller = x.Controller,
                    Url = x.Url,
                    Title= x.Title,
                    Roles = x.Roles.Select(x => x.Role.RoleType).ToList(),
                }).ToTableResultAsync(request.Filter);
            return rslt;
        }
    }
}
