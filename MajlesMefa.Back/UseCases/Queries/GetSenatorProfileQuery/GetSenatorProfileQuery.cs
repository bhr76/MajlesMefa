using MediatR;
using MajlesMefa.Back.Dtos.Common.Details;
using MajlesMefa.Back.Migrations;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetSenatorProfileQuery
{

    public class GetSenatorProfileQuery: IRequest<TableModel<SenatorDetailDto>>
    {
        public Guid? UserId { get; set; }
        public Guid? CommissionId { get; set; }

        public TableRequestModel Filter { get; set; } = new TableRequestModel();
    }
}
