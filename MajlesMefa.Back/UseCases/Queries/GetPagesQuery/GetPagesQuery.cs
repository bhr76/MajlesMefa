using MediatR;
using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Dtos.UserDtos;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetPagesQuery
{
    public class GetPagesQuery: IRequest<TableModel<PageWithRolesDto>>
    {
        public Guid? Id { get; set; }

        public TableRequestModel Filter {get;set; } = new TableRequestModel();
    }
}
