using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Services.Abstractioin;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace MajlesMefa.Back.UseCases.Queries.GetLoanReportQuery
{
    public class GetLoanReportQueryHandler : IRequestHandler<GetLoanReportQuery, TableModel<DataEntryDto>>
    {
        private readonly RefahMajlesDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetLoanReportQueryHandler(RefahMajlesDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<TableModel<DataEntryDto>> Handle(GetLoanReportQuery request, CancellationToken cancellationToken)
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

            var tempTable10 = query
                  .Select(l => new
                  {
                      Loan = l.Loan,
                      CurrentUserId = l.ActionReferences.Where(a => a.ActRefType == ActRefTypeEnum.Refer)
                          .OrderByDescending(a => a.Created)
                          .Select(a => a.ToUserId)
                          .FirstOrDefault()
                  });

            // فیلتر VaziatPasokh حذف شد و VaziatPasokh به GroupBy اضافه نشد
            var tempTable1100 = tempTable10
                .GroupBy(x => new { x.CurrentUserId, x.Loan.LoanType })
                .Select(g => new
                {
                    CurrentUserId = g.Key.CurrentUserId,
                    LoanType = g.Key.LoanType,

                    // کل
                    TotalAmount = g.Sum(x => x.Loan.Amount),
                    TotalCount = g.Count(),

                    // پرداخت شده
                    PaidAmount = g.Where(x => x.Loan.VaziatPasokh == ResponseStatusEnum.Mosbat).Sum(x => x.Loan.Amount ),
                    PaidCount = g.Count(x => x.Loan.VaziatPasokh == ResponseStatusEnum.Mosbat),

                    // پرداخت نشده
                    UnpaidAmount = g.Where(x => x.Loan.VaziatPasokh == ResponseStatusEnum.Manfi || x.Loan.VaziatPasokh == null).Sum(x => x.Loan.Amount),
                    UnpaidCount = g.Count(x => x.Loan.VaziatPasokh == ResponseStatusEnum.Manfi || x.Loan.VaziatPasokh == null),

                    //بانک
                     InBranchAmount = g.Where(x => x.Loan.VaziatPasokh == ResponseStatusEnum.Shobe).Sum(x => x.Loan.Amount),
                    InBranchCount = g.Count(x => x.Loan.VaziatPasokh == ResponseStatusEnum.Shobe),

                });

            var result1 = (from t in tempTable1100
                           join u in _context.Users on t.CurrentUserId equals u.Id
                           where u.Name.Contains("بان")
                           select new
                           {
                               Name = u.Name,
                               LoanType = t.LoanType,

                               // کل
                               TotalAmount = t.TotalAmount,
                               TotalCount = t.TotalCount,

                               // پرداخت شده
                               PaidAmount = t.PaidAmount,
                               PaidCount = t.PaidCount,

                               // پرداخت نشده
                               UnpaidAmount = t.UnpaidAmount,
                               UnpaidCount = t.UnpaidCount,

                               //بانک
                               InBranchAmount = t.InBranchAmount,
                               InBranchCount = t.InBranchCount
                           });

            var result = await result1.Select(x => new DataEntryDto()
            {
                MyData = new GetLoanReportQueryResponse()
                {
                    // کل
                    TotalAmount = x.TotalAmount,
                    TotalCount = x.TotalCount,

                    // پرداخت شده
                    PaidAmount = x.PaidAmount,
                    PaidCount = x.PaidCount,

                    // پرداخت نشده
                    UnPaidAmount = x.UnpaidAmount,
                    UnPaidCount = x.UnpaidCount,

                    InBranchAmount = x.InBranchAmount,
                    InBranchCount = x.InBranchCount,

                    LoanType = x.LoanType,
                    BankFullName = x.Name
                }
            }).ToTableResultAsync(request.Filter);

            return result;
        }
    }
}