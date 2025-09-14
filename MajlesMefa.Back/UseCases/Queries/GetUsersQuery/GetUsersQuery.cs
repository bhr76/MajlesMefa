using MediatR;
using MajlesMefa.Back.Dtos.UserDtos;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetUsersQuery
{
    public class GetUsersQuery: IRequest<TableModel<BsUserDto>>
    {
        public List<RoleTypeEnum> Roles { get; set; }
        
        public Guid? CityId { get;set; }
        
        public Guid? OrganizationId { get;set; }

        public TableRequestModel Filter { get; set; }
    }
}
