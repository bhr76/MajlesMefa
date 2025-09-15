using MediatR;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetCategoriesQuery
{
    public class GetCategoriesQuery: IRequest<TableModel<CategoryDto>>
    {
        public Guid? ParentId { get; set; }
        
        public Guid? Id { get; set; }

        public DataEntryTypeEnum? DataEntryType { get; set; }

        public TableRequestModel Filter { get; set; } = new TableRequestModel();
    }
}
