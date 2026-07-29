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
                .Where(x => x.DataEntryType == request.DataEntryType);

            #region permission
            if (cuser.Roles.Any(u => u == RoleTypeEnum.MinistryMember))
            {
                if (request.DataEntryType != DataEntryTypeEnum.DastoorJalasatComission)
                    query = query.Where(q =>
                        q.ActionReferences.Any(ar =>
                            ar.FromUserId == cuser.BussinessUserId ||
                            ar.ToUserId == cuser.BussinessUserId) ||
                        q.ActionReferences.Select(a => a.FromUser.OrganizationId)
                            .FirstOrDefault() != null);
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
                query = query.Where(q => q.ActionReferences.Any(ar => ar.ToUserId == cuser.BussinessUserId));
            }

            var loansQuery = _context.Loans
                .AsNoTracking()
                .AsQueryable();

            if (request.ResponseStatuses != null && request.ResponseStatuses.Any())
            {
                loansQuery = loansQuery.Where(l => request.ResponseStatuses.Contains(l.VaziatPasokh));
            }

            if (request.LoanType.HasValue && request.LoanType.Value != 0)
            {
                loansQuery = loansQuery.Where(l => l.LoanType == request.LoanType.Value);
            }

            var joinedQuery =
                from l in loansQuery
                join d in query on l.DataEntryId equals d.Id
                join p in _context.SenatorProfiles on d.SenatorId equals p.UserId
                select new
                {
                    LoanType = l.LoanType,
                    Amount = l.Amount,
                    Created = d.Created,
                    Name = p.Name,
                    FullName = p.Name
                };

            if (request.Year.HasValue)
            {
                var pc = new PersianCalendar();

                var fromDate = pc.ToDateTime(request.Year.Value, 1, 1, 0, 0, 0, 0);
                var toDate = pc.ToDateTime(request.Year.Value + 1, 1, 1, 0, 0, 0, 0);

                joinedQuery = joinedQuery.Where(x => x.Created >= fromDate && x.Created < toDate);
            }

            if (!string.IsNullOrWhiteSpace(request.SenatorName))
            {
                joinedQuery = joinedQuery.Where(x =>
                    x.Name.Contains(request.SenatorName) ||
                    x.FullName.Contains(request.SenatorName));
            }

            var result1 = joinedQuery
                .GroupBy(x => new { x.FullName, x.LoanType })
                .Select(g => new
                {
                    Name = g.Key.FullName,
                    LoanType = g.Key.LoanType,
                    Cnt = g.Count(),
                    Amount = g.Sum(x => x.Amount)
                });

            var result = await result1
                .Select(x => new DataEntryDto()
                {
                    MyData = new GetLoanSenatorReportQueryResponse()
                    {
                        Amount = x.Amount,
                        count = x.Cnt,
                        LoanType = x.LoanType,
                        SenatorFullName = x.Name
                    }
                })
                .ToTableResultAsync(request.Filter);

            return result;
        }

    }
}