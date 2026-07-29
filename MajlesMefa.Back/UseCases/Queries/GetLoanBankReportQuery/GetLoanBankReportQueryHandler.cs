using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Services.Abstractioin;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Dynamic.Core;

namespace MajlesMefa.Back.UseCases.Queries.GetLoanBankReportQuery
{
    public class GetLoanBankReportQueryHandler : IRequestHandler<GetLoanBankReportQuery, TableModel<DataEntryDto>>
    {
        private readonly RefahMajlesDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetLoanBankReportQueryHandler(
            RefahMajlesDbContext context,
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
                .Where(x => x.DataEntryType == request.DataEntryType && x.Loan != null);

            #region permission
            if (cuser.Roles.Any(u => u == RoleTypeEnum.MinistryMember))
            {
                if (request.DataEntryType != DataEntryTypeEnum.DastoorJalasatComission)
                {
                    query = query.Where(q =>
                        q.ActionReferences.Any(ar =>
                            ar.FromUserId == cuser.BussinessUserId ||
                            ar.ToUserId == cuser.BussinessUserId) ||
                        q.ActionReferences
                            .Select(ar => ar.FromUser.OrganizationId)
                            .FirstOrDefault() != null);
                }
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
                query = query.Where(q => q.ActionReferences.Any(ar =>
                    ar.ToUserId == cuser.BussinessUserId));
            }

            if (request.Year.HasValue)
            {
                var pc = new PersianCalendar();

                var fromDate = pc.ToDateTime(request.Year.Value, 1, 1, 0, 0, 0, 0);
                var toDate = pc.ToDateTime(request.Year.Value + 1, 1, 1, 0, 0, 0, 0);

                query = query.Where(x => x.Created >= fromDate && x.Created < toDate);
            }

            var tempTable17 = query
                .Select(l => new
                {
                    Amount = l.Loan.Amount,
                    LoanType = l.Loan.LoanType,
                    VaziatPasokh = l.Loan.VaziatPasokh,
                    BankUserId = l.ActionReferences
                        .Where(a => a.ActRefType == ActRefTypeEnum.Refer)
                        .OrderByDescending(a => a.Created)
                        .Select(a => a.ToUserId)
                        .FirstOrDefault()
                });

            var tempTable19 = tempTable17
                .Where(t => t.BankUserId == request.BankUserId)
                .Join(_context.Users.AsNoTracking(),
                    t => t.BankUserId,
                    u => u.Id,
                    (t, u) => new
                    {
                        t.Amount,
                        t.LoanType,
                        t.VaziatPasokh,
                        t.BankUserId,
                        BankName = u.Name
                    });

            var result1 = tempTable19
                .GroupBy(x => new { x.BankName, x.LoanType })
                .Select(g => new
                {
                    BankName = g.Key.BankName,
                    LoanType = g.Key.LoanType,

                    TotalCount = g.Count(),
                    TotalAmount = g.Sum(x => x.Amount),

                    PaidCount = g.Count(x => x.VaziatPasokh == ResponseStatusEnum.Mosbat),
                    PaidAmount = g.Where(x => x.VaziatPasokh == ResponseStatusEnum.Mosbat).Sum(x => x.Amount),

                    UnpaidCount = g.Count(x => x.VaziatPasokh == ResponseStatusEnum.Manfi || x.VaziatPasokh == null),
                    UnpaidAmount = g.Where(x => x.VaziatPasokh == ResponseStatusEnum.Manfi || x.VaziatPasokh == null).Sum(x => x.Amount),

                    InBranchCount = g.Count(x => x.VaziatPasokh == ResponseStatusEnum.Shobe),
                    InBranchAmount = g.Where(x => x.VaziatPasokh == ResponseStatusEnum.Shobe).Sum(x => x.Amount)
                });

            var result = await result1
                .OrderBy(x => x.BankName)
                .Select(x => new DataEntryDto()
                {
                    MyData = new GetLoanBankReportQueryResponse()
                    {
                        TotalAmount = x.TotalAmount,
                        TotalCount = x.TotalCount,

                        PaidAmount = x.PaidAmount,
                        PaidCount = x.PaidCount,

                        UnPaidAmount = x.UnpaidAmount,
                        UnPaidCount = x.UnpaidCount,

                        InBranchAmount = x.InBranchAmount,
                        InBranchCount = x.InBranchCount,

                        LoanType = x.LoanType,
                        BankFullName = x.BankName
                    }
                })
                .ToTableResultAsync(request.Filter);

            return result;
        }
    }
}
