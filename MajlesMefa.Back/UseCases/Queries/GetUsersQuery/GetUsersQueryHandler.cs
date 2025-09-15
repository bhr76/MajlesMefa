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

namespace MajlesMefa.Back.UseCases.Queries.GetUsersQuery
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, TableModel<BsUserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUsersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TableModel<BsUserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.UserRepository
                .NoTracking;
            if (request.Roles?.Count > 0)
            {
                query = query.Where(x => x.UserRoles.Any(x => request.Roles.Contains(x.Role.RoleType)));
            }
            if (request.CityId.HasValue)
            {
                query = query.Where(x => x.CityId == request.CityId);
            }
            if (request.OrganizationId.HasValue)
            {
                query = query.Where(x => x.OrganizationId == request.OrganizationId);
            }
            var rslt = await query.Select(x => new BsUserDto()
            {
                AvatarFileName = x.AvatarFileName,
                City = x.CityId.HasValue ? new CityDto()
                {
                    Id = x.CityId.Value,
                    Name = x.City.Name,
                    ParentId = x.City.ParentCityId,
                    ParentName = x.City.Parent.Name,
                } : null,
                Id = x.Id,
                IsActive = x.IsActive,
                Mobile = x.Mobile,
                Role = x.UserRoles.FirstOrDefault().Role.RoleType,
                Name =x.Name,
                Organization = x.OrganizationId.HasValue? new OrganizationDto()
                {
                    Id = x.OrganizationId.Value,
                    Name = x.Organization.Name,
                    ParentId = x.Organization.ParentId,
                    ParentName = x.Organization.Parent.Name,
                } : null,

            }).ToTableResultAsync(request.Filter);
            return rslt;
        }
    }
}
