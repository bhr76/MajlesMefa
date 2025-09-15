using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetKeywordsQuery
{
    public class GetKeywordsQueryResult
    {
        public TableModel<KeywordDto> Keywords { get; set; }

        public int Sum { get; set; }
    }
}
