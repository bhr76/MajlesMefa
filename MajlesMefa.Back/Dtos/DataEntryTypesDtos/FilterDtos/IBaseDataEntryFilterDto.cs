using MajlesMefa.Back.Utilities.Db.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Dtos.DataEntryTypesDtos.FilterDtos
{
    public interface IBaseDataEntryFilterDto
    {
        public int Page { get; set; }
        
        public int PageSize { get; set; }
        
        public int Skip { get; set; }
        
        public int Take { get; set; }
    }
}
