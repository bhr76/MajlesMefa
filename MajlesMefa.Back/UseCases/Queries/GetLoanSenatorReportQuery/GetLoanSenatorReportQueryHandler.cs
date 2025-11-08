using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Services.Abstractioin;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace MajlesMefa.Back.UseCases.Queries.GetLoanSenatorReportQuery
{
    public class GetLoanSenatorReportQueryHandler : IRequestHandler<GetLoanSenatorReportQuery, TableModel<DataEntryDto>>
    {
        private readonly RefahMajlesDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetLoanSenatorReportQueryHandler(RefahMajlesDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<TableModel<DataEntryDto>> Handle(GetLoanSenatorReportQuery request, CancellationToken cancellationToken)
        {
            var cuser = _currentUserService.GetCurrentUser();

            var query = _context.DataEntries
                .AsNoTracking()
                .Include(x => x.ActionReferences).ThenInclude(x => x.FromUser)
                .Include(x => x.ActionReferences).ThenInclude(x => x.ToUser)
                .Where(x => x.DataEntryType == request.DataEntryType);

            #region permission
            if (cuser.Roles.Any(u => u == RoleTypeEnum.MinistryMember))
            {
                if (request.DataEntryType != DataEntryTypeEnum.DastoorJalasatComission)
                    query = query.Where(q => (q.ActionReferences.Any(ar =>
                    ar.FromUserId == cuser.BussinessUserId ||
                    ar.ToUserId == cuser.BussinessUserId)) ||
                    q.ActionReferences.FirstOrDefault().FromUser.OrganizationId != null);
            }
            #endregion
            if (request.DataEntryId.HasValue)
            {
                query = query.Where(x => x.Id == request.DataEntryId);
            }

            if (cuser.Roles.Any(e => e == RoleTypeEnum.Senator))
            {
                query = query.Where(x => x.SenatorId == cuser.BussinessUserId);
            }
            if (cuser.Roles.Any(e => e == RoleTypeEnum.Organization))
            {
                query = query.Where(q => (q.ActionReferences.Any(ar =>
                    ar.ToUserId == cuser.BussinessUserId)));
            }

            var tempTable12 = _context.Loans
                .Where(l => l.VaziatPasokh == request.ResponseStatus)
                .Join(_context.DataEntries,
                      l => l.DataEntryId,
                      sp => sp.Id,
                      (l, sp) => new { Loan = l, DataEntry = sp })
                .Join(_context.SenatorProfiles,
                      x => x.DataEntry.SenatorId,
                      p => p.UserId,
                      (x, p) => new
                      {
                          Loan = x.Loan,
                          Name = p.Name
                      });

            var result1 = tempTable12
                .GroupBy(x => new { x.Name, x.Loan.LoanType })
                .Select(g => new
                {
                    Name = g.Key.Name,
                    LoanType = g.Key.LoanType,
                    Cnt = g.Count(),
                    Amount = g.Sum(x => x.Loan.Amount)
                });

            var result = await result1.Select(x => new DataEntryDto()
            {
                MyData = new GetLoanSenatorReportQueryResponse()
                {
                    Amount=x.Amount,
                    count = x.Cnt,
                    LoanType = x.LoanType,
                    SenatorFullName = x.Name
                }
            }).ToTableResultAsync(request.Filter);

            return result;
        }

    }

}
