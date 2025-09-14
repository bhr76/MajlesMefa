using MediatR;
using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Dtos.Common.Details;
using MajlesMefa.Back.Enums.Senator;
using MajlesMefa.Back.Repositories;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MajlesMefa.Back.Dtos.Common;
using MajlesMefa.Back.Entities;

namespace MajlesMefa.Back.UseCases.Queries.GetBanksQuery
{
    public class GetBanksQueryHandler : IRequestHandler<GetBanksQuery, TableModel<BankDto>>
    {
        private readonly RefahMajlesDbContext _context;

        public GetBanksQueryHandler(RefahMajlesDbContext context)
        {
            _context = context;
        }

        public async Task<TableModel<BankDto>> Handle(GetBanksQuery request, CancellationToken cancellationToken)
        {
            var query = await _context.Banks
                .Select(b => new BankDto()
                {

                    Id = b.Id,
                    Name = b.Name,

                }).ToTableResultAsync(request.Filter);
            return query;
        }
    }
}
