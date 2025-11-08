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
            
                    var tempTable1100 = tempTable10
                        .Where(x => x.Loan.VaziatPasokh == request.ResponseStatus)
                        .GroupBy(x => new { x.CurrentUserId, x.Loan.LoanType })
                        .Select(g => new
                        {
                            CurrentUserId = g.Key.CurrentUserId,
                            LoanType = g.Key.LoanType,
                            Amount = g.Sum(x => x.Loan.Amount),
                            Cnt = g.Count()
                        });  

            var result1 = (from t in tempTable1100
                          join u in _context.Users on t.CurrentUserId equals u.Id
                          where u.Name.Contains("بان")
                          select new
                          {
                              Name = u.Name,
                              Amount = t.Amount,
                              LoanType = t.LoanType,
                              Cnt = t.Cnt
                          });

            var result = await result1.Select(x => new DataEntryDto()
            {
                MyData = new GetLoanReportQueryResponse()
                {
                    Amount=x.Amount,
                    count = x.Cnt,
                    LoanType = x.LoanType,
                    BankFullName = x.Name
                }
            }).ToTableResultAsync(request.Filter);

            return result;
        }

    }

}
