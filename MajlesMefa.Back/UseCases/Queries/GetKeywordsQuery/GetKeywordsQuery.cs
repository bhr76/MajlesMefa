using MediatR;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetKeywordsQuery
{
    public class GetKeywordsQuery: IRequest<GetKeywordsQueryResult>
    {
        public Guid DataEntryId { get;set; }

        public TableRequestModel Filter { get;set; }
    }
}
