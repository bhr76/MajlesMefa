using MediatR;
using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetActionRefrencesByDataEntryId
{
    public class GetActionRefrencesByDataEntryIdQuery : IRequest<TableModel<ActionRefrenceDto>>
    {
        public Guid DataEntryId { get; set; }
        public TableRequestModel Filter { get; set; }
        public ActRefTypeEnum ActRefType { get; set; }
        
    }
}

