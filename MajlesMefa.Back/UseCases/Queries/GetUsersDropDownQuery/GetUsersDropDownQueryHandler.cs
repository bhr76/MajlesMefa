using MediatR;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Dtos.UserDtos;
using MajlesMefa.Back.Repositories;
using MajlesMefa.Back.Repositories.Abstraction;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetUsersDropDownQuery
{
    public class GetUsersDropDownQueryHandler : IRequestHandler<GetUsersDropDownQuery, List<UserDropDownDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUsersDropDownQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<UserDropDownDto>> Handle(GetUsersDropDownQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.UserRepository.NoTracking
                .AsQueryable();
            if (request.IsActive.HasValue)
            {
                query = query.Where(x => x.IsActive == request.IsActive);
            }
            if (request.Role.HasValue)
            {
                query = query.Where(x => x.UserRoles.Any(xx => xx.Role.RoleType == request.Role));
            }
            if (request.OrganizationId.HasValue)
            {
                query = query.Where(x => x.OrganizationId == request.OrganizationId);
            }
            if (request.CityId.HasValue)
            {
                query = query.Where(x => x.CityId == request.CityId);
            }

            var rslt = await query
                .Select(x => new UserDropDownDto()
                {
                    Name = x.Name,
                    Id = x.Id
                })
                .ToListAsync();

            return rslt;
        }
    }
}
