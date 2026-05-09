using MediatR;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Grid;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Repositories;
using MajlesMefa.Back.Services.Abstractioin;
using MajlesMefa.Back.Utilities.Date;
using MD.PersianDateTime.Standard;

namespace MajlesMefa.Back.UseCases.Queries.GetDashboardQuery
{
    public class GetDashboardQueryHandler :
         IRequestHandler<GetDashboardLoanByStatusQuery, DashboardLoanDto>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RefahMajlesDbContext _context;
        private readonly DapperContext _dapperContext;

        public GetDashboardQueryHandler(ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            RefahMajlesDbContext context, DapperContext dapperContext)
        {
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _context = context;
            _dapperContext = dapperContext;
        }



        public async Task<DashboardLoanDto> Handle(GetDashboardLoanByStatusQuery request, CancellationToken cancellationToken)
        {
            string[] months = { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };

            var cuser = _currentUserService.GetCurrentUser();

            // Base query for loans
            var query = _context.DataEntries
                .AsNoTracking()
                .Include(x => x.Loan)
                .Include(x => x.ActionReferences).ThenInclude(x => x.FromUser)
                .Include(x => x.ActionReferences).ThenInclude(x => x.ToUser)
                .Where(x => x.DataEntryType == DataEntryTypeEnum.Loan);

            // Query for Shora references - used for countOfShoraRefrence
            //var dataEntryIds = await _context.ActionReferences
            //    .AsNoTracking()
            //    .Where(a => a.FromUserId == request.ShoraUserId && a.ActRefType == ActRefTypeEnum.Refer)
            //    .GroupBy(a => a.DataEntryId)
            //    .Select(g => g.Key)
            //    .ToListAsync();

            //var queryShora = _context.DataEntries
            //    .AsNoTracking()
            //    .Include(x => x.Loan)
            //    .Include(x => x.ActionReferences).ThenInclude(x => x.FromUser)
            //    .Include(x => x.ActionReferences).ThenInclude(x => x.ToUser)
            //    .Where(x => dataEntryIds.Contains(x.Id) && x.DataEntryType == DataEntryTypeEnum.Loan);

            // Apply role-based filters to both queries
            if (cuser.Roles.Any(e => e == RoleTypeEnum.Organization))
            {
                query = query.Where(q => q.ActionReferences.Any(ar => ar.ToUserId == cuser.BussinessUserId));
                //queryShora = queryShora.Where(q => q.ActionReferences.Any(ar => ar.ToUserId == cuser.BussinessUserId));
            }

            if (cuser.Roles.Any(e => e == RoleTypeEnum.Senator))
            {
                query = query.Where(x => x.SenatorId == cuser.BussinessUserId);
                //queryShora = queryShora.Where(x => x.SenatorId == cuser.BussinessUserId);
            }

            try
            {
                // CountOfShoraRefrence - using queryShora with role filters applied
                //var tempTable10 = queryShora
                //    .Select(l => new
                //    {
                //        Loan = l.Loan,
                //        CurrentUserId = l.ActionReferences
                //            .Where(a => a.ActRefType == ActRefTypeEnum.Refer)
                //            .OrderByDescending(a => a.Created)
                //            .Select(a => a.ToUserId)
                //            .FirstOrDefault()
                //    });

                //var tempTable1100 = tempTable10
                //    .GroupBy(x => new { x.CurrentUserId, x.Loan.LoanType })
                //    .Select(g => new
                //    {
                //        CurrentUserId = g.Key.CurrentUserId,
                //        LoanType = g.Key.LoanType,
                //        TotalCount = g.Count()
                //    });

                //var countOfShoraRefrence = await (
                //    from t in tempTable1100
                //    join u in _context.Users on t.CurrentUserId equals u.Id
                //    where u.Name.Contains("بان") || u.Name.Contains("صندوق")
                //    select t.TotalCount
                //).SumAsync();

                // Total counts from main query
                var countOfAllLoans = await query.CountAsync();
                var countOfApprovedLoan = await query.Where(de => de.Loan.VaziatPasokh == ResponseStatusEnum.Mosbat).CountAsync();
                var countOfDeniedLoan = await query.Where(de => de.Loan.VaziatPasokh == ResponseStatusEnum.Manfi).CountAsync();
                var countOfInProgressLoan = await query.Where(de => de.Loan.VaziatPasokh == ResponseStatusEnum.Inprogress).CountAsync();
                var countOfInBranchRequest = await query.Where(de => de.Loan.VaziatPasokh == ResponseStatusEnum.Shobe).CountAsync();

                // Rejected by branch count
                var tempTableForRejected = query
                    .Select(l => new
                    {
                        Loan = l.Loan,
                        CurrentUserId = l.ActionReferences
                            .Where(a => a.ActRefType == ActRefTypeEnum.Refer)
                            .OrderByDescending(a => a.Created)
                            .Select(a => a.ToUserId)
                            .FirstOrDefault()
                    });

                var rejectedByBranchCount = await tempTableForRejected
                    .Join(_context.Users,
                        t => t.CurrentUserId,
                        u => u.Id,
                        (t, u) => new { t.Loan, BankName = u.Name })
                    .Where(x => (x.BankName.Contains("بان") || x.BankName.Contains("صندوق"))
                                && (x.Loan.VaziatPasokh == ResponseStatusEnum.Manfi || x.Loan.VaziatPasokh == null))
                    .CountAsync();

                // Monthly chart data
                if (request.Status != null)
                {
                    query = query.Where(x => x.Loan.VaziatPasokh == request.Status);
                }

                var dataList = await query
                    .Select(de => new { CreatedDate = de.Created, Amount = de.Loan.Amount })
                    .ToListAsync();

                var dashboardData = dataList
                    .Select(x => new
                    {
                        PersianDate = PersianDateTime.Parse(x.CreatedDate.ToPersianDate(), "/"),
                        Amount = x.Amount
                    })
                    .GroupBy(x => new { x.PersianDate.Year, x.PersianDate.Month })
                    .Select(g => new
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        TotalRequests = g.Count(),
                        TotalAmount = g.Sum(x => x.Amount),
                    })
                    .ToList();

                var currentPersianDate = PersianDateTime.Now;
                var currentYear = currentPersianDate.Year;
                var currentMonth = currentPersianDate.Month;

                var dashboardFinalData = new List<DashboardLoanItemDto>();

                for (int i = 0; i < 12; i++)
                {
                    var targetMonth = currentMonth - i;
                    var targetYear = currentYear;

                    if (targetMonth <= 0)
                    {
                        targetMonth += 12;
                        targetYear--;
                    }

                    var monthData = dashboardData.FirstOrDefault(x => x.Year == targetYear && x.Month == targetMonth);
                    var monthName = months[targetMonth - 1];

                    dashboardFinalData.Add(new DashboardLoanItemDto
                    {
                        ItemName = monthName,
                        Count = monthData?.TotalRequests ?? 0,
                        ItemType = DataEntryTypeEnum.Loan,
                        TotalAmount = monthData?.TotalAmount ?? 0,
                    });
                }

                dashboardFinalData.Reverse();

                DashboardLoanDto dto = new()
                {
                    CountAllLoan = countOfAllLoans,
                    CountOfApprovedLoan = countOfApprovedLoan,
                    CountOfDeniedLoan = countOfDeniedLoan,
                    CountOfInProgressLoan = countOfInProgressLoan,
                    CountOfInBranchLoan = countOfInBranchRequest,
                    CountOfRejectedByBranch = rejectedByBranchCount,
                    DashboardChartData = dashboardFinalData,
                    //CountShoraRefrences = countOfShoraRefrence
                };

                return dto;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
