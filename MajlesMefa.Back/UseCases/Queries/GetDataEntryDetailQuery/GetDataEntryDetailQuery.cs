using MediatR;
using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetDataEntryDetailQuery
{
    public class GetDataEntryDetailQuery: IRequest<DataEntryDto>
    {
        public Guid DataEntryId { get; set; }

        public DataEntryTypeEnum DataEntryType { get; set; }
    }
}
