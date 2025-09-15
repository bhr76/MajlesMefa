using MediatR;
using MajlesMefa.Back.Dtos.Common.Details;
using MajlesMefa.Back.Migrations;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MajlesMefa.Back.Dtos.Common;

namespace MajlesMefa.Back.UseCases.Queries.GetBanksQuery
{

    public class GetBanksQuery: IRequest<TableModel<BankDto>>
    {
        public TableRequestModel Filter { get; set; } = new TableRequestModel();
    }
}
