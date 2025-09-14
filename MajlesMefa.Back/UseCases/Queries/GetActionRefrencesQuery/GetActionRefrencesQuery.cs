using MediatR;
using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetActionRefrencesQuery
{
    public class GetActionRefrencesQuery: IRequest<TableModel<ActionRefrenceDto>>
    {
        public TableRequestModel Filter { get; set; }
    }
}
