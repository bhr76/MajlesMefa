using MediatR;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.UseCases.Queries.GetPeygiriByIdQuery
{
    public class GetPeygiriByIdQuery: IRequest<PeygiryDto>
    {
        public long Id {get;set; }
    }
}
