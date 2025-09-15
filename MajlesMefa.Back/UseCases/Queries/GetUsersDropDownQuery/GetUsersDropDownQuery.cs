using MediatR;
using MajlesMefa.Back.Dtos.UserDtos;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetUsersDropDownQuery
{
    public class GetUsersDropDownQuery: IRequest<List<UserDropDownDto>>
    {

        public bool? IsActive { get;set; }
        public Guid? CityId { get; set; }
        public Guid? OrganizationId { get; set; }

        public RoleTypeEnum? Role { get; set; }
    }
}
