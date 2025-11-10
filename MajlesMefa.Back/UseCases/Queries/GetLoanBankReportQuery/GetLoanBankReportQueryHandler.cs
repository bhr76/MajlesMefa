using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Services.Abstractioin;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace MajlesMefa.Back.UseCases.Queries.GetLoanBankReportQuery
{
    public class GetLoanBankReportQueryHandler : IRequestHandler<GetLoanBankReportQuery, TableModel<DataEntryDto>>
    {
        private readonly RefahMajlesDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetLoanBankReportQueryHandler(RefahMajlesDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<TableModel<DataEntryDto>> Handle(GetLoanBankReportQuery request, CancellationToken cancellationToken)
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

            var tempTable17 = query
                .Where(l=>l.Loan.VaziatPasokh == request.ResponseStatus)
                .Select(l => new
                {
                    Amount = l.Loan.Amount,
                    LoanType = l.Loan.LoanType,
                    BankUserId = l.ActionReferences
                     .Where(a => a.ActRefType == ActRefTypeEnum.Refer
                     )
                        .OrderByDescending(a => a.Created)
                        .Select(a => a.ToUserId)
                        .FirstOrDefault(),
                    DataEntryId = l.Loan.DataEntryId 
                }); 

            var tempTable19 = tempTable17
                .Where(t => t.BankUserId == request.BankUserId )
                .Join(_context.Users,
                      t => t.BankUserId,
                      u => u.Id,
                      (t, u) => new
                      {
                          t.Amount,
                          t.LoanType,
                          t.BankUserId,
                          BankName = u.Name
                      }); 

            var result1 = tempTable19
                .GroupBy(x => new { x.BankName, x.LoanType })
                .Select(g => new
                {
                    BankName = g.Key.BankName,
                    LoanType = g.Key.LoanType,
                    Cnt = g.Count(),
                    Amount = g.Sum(x => x.Amount)
                });

            var result = await result1.Select(x => new DataEntryDto()
            {
                MyData = new GetLoanBankReportQueryResponse()
                {
                    Amount=x.Amount,
                    count = x.Cnt,
                    LoanType = x.LoanType,
                    BankFullName = x.BankName
                }
            }).ToTableResultAsync(request.Filter);

            return result;
        }

    }

}
