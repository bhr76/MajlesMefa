using MediatR;
using MajlesMefa.Back.Dtos.UserDtos;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetUserByIdQuery
{
    public class GetUserByIdQuery: IRequest<UserDetailDto>
    {
        public Guid UserId { get; set; }
    }
}
