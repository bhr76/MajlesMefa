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
    public class GetCategoryByIdQuery: IRequest<CategoryDto>
    {
        
        public Guid Id { get; set; }
        public DataEntryTypeEnum? DataEntryType { get; set; }
    }
}
