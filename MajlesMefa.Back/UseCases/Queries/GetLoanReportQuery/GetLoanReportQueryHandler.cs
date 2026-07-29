using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Services.Abstractioin;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq.Dynamic.Core;

namespace MajlesMefa.Back.UseCases.Queries.GetLoanReportQuery
{
    public class GetLoanReportQueryHandler : IRequestHandler<GetLoanReportQuery, TableModel<DataEntryDto>>
    {
        private readonly RefahMajlesDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetLoanReportQueryHandler(
            RefahMajlesDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<TableModel<DataEntryDto>> Handle(GetLoanReportQuery request, CancellationToken cancellationToken)
        {
            _context.Database.SetCommandTimeout(120);
            var cuser = _currentUserService.GetCurrentUser();

            var query = _context.DataEntries
                .AsNoTracking()
                .Where(x => x.DataEntryType == DataEntryTypeEnum.Loan && x.Loan != null);

            if (cuser.Roles.Any(e => e == RoleTypeEnum.Organization))
            {
                query = query.Where(q => q.ActionReferences.Any(ar => ar.ToUserId == cuser.BussinessUserId));
            }

            if (cuser.Roles.Any(e => e == RoleTypeEnum.Senator))
            {
                query = query.Where(x => x.SenatorId == cuser.BussinessUserId);
            }

            if (request.Year.HasValue)
            {
                var pc = new PersianCalendar();

                var fromDate = pc.ToDateTime(request.Year.Value, 1, 1, 0, 0, 0, 0);
                var toDate = pc.ToDateTime(request.Year.Value + 1, 1, 1, 0, 0, 0, 0);

                query = query.Where(x => x.Created >= fromDate && x.Created < toDate);
            }

            if (request.LoanTypeFilter.HasValue)
            {
                query = query.Where(x => x.Loan.LoanType == request.LoanTypeFilter.Value);
            }

            var baseQuery = query
                .Select(x => new
                {
                    Amount = x.Loan.Amount,
                    LoanType = x.Loan.LoanType,
                    VaziatPasokh = x.Loan.VaziatPasokh,
                    CurrentUserId = x.ActionReferences
                        .Where(a => a.ActRefType == ActRefTypeEnum.Refer)
                        .OrderByDescending(a => a.Created)
                        .Select(a => a.ToUserId)
                        .FirstOrDefault()
                });

            var bankRows =
                from x in baseQuery
                join u in _context.Users.AsNoTracking() on x.CurrentUserId equals u.Id
                where u.Name.Contains("بان") || u.Name.StartsWith("صندوق")
                select new
                {
                    BankName = u.Name,
                    x.LoanType,
                    x.Amount,
                    x.VaziatPasokh
                };

            var resultQuery = bankRows
                .GroupBy(x => new { x.BankName, x.LoanType })
                .Select(g => new
                {
                    Name = g.Key.BankName,
                    LoanType = g.Key.LoanType,

                    TotalAmount = g.Sum(x => x.Amount),
                    TotalCount = g.Count(),

                    PaidAmount = g.Sum(x =>
                        x.VaziatPasokh == ResponseStatusEnum.Mosbat
                            ? x.Amount
                            : 0),

                    PaidCount = g.Sum(x =>
                        x.VaziatPasokh == ResponseStatusEnum.Mosbat
                            ? 1
                            : 0),

                    UnpaidAmount = g.Sum(x =>
                        x.VaziatPasokh == ResponseStatusEnum.Manfi || x.VaziatPasokh == null
                            ? x.Amount
                            : 0),

                    UnpaidCount = g.Sum(x =>
                        x.VaziatPasokh == ResponseStatusEnum.Manfi || x.VaziatPasokh == null
                            ? 1
                            : 0),

                    InBranchAmount = g.Sum(x =>
                        x.VaziatPasokh == ResponseStatusEnum.Shobe
                            ? x.Amount
                            : 0),

                    InBranchCount = g.Sum(x =>
                        x.VaziatPasokh == ResponseStatusEnum.Shobe
                            ? 1
                            : 0)
                });

            var result = await resultQuery
                .OrderBy(x => x.Name)
                .Select(x => new DataEntryDto()
                {
                    MyData = new GetLoanReportQueryResponse()
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
                        BankFullName = x.Name
                    }
                })
                .ToTableResultAsync(request.Filter);

            return result;
        }

    }
}
