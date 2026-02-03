using MediatR;
using Microsoft.EntityFrameworkCore;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Grid;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Enums.Soval;
using MajlesMefa.Back.Repositories;
using MajlesMefa.Back.Services.Abstractioin;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.EnumHelper;
using Dapper;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Collections;
using MD.PersianDateTime.Standard;
using MajlesMefa.Back.Migrations;

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

        //public async Task<List<MokatebeDashboardItemDto>> Handle(GetDashboardMokatebeByOrganizationQuery request, CancellationToken cancellationToken)
        //{

        //    //var query = "select Id , ItemName, sum(Count) Count from(\r\nselect dbo.fn_getParentId(CategoryId) Id , dbo.fn_getParentName(CategoryId) ItemName ,count(*) Count from DataEntries  where CategoryId is not null and DataEntryType = '4'\r\ngroup by dbo.fn_getParentId(CategoryId), dbo.fn_getParentName(CategoryId)\r\n union all\r\nselect tblComma.value Id , dbo.fn_getParentName(value) ItemName, count(*) Count from (\r\nSELECT value , dbo.fn_getParentName(value) ItemName\r\nFROM DataEntries\r\nCROSS APPLY STRING_SPLIT(Moavenats, ',')\r\nwhere Moavenats is not null and DataEntryType = '4'\r\n) tblComma GROUP BY value , dbo.fn_getParentName(value)\r\n) tblSumm\r\nGROUP BY Id , ItemName\r\norder by Count desc";
        //    var query = "select Id , ItemName, sum(Count) Count, IsCentralOffice from(\r\nselect dbo.fn_getParentId(CategoryId) Id , dbo.fn_getParentName(CategoryId) ItemName , \r\ndbo.fn_CheckCategoryCentralProvince(CategoryId) IsCentralOffice,\r\ncount(*) Count from DataEntries  where CategoryId is not null and DataEntryType = '4'\r\ngroup by dbo.fn_getParentId(CategoryId), dbo.fn_getParentName(CategoryId), dbo.fn_CheckCategoryCentralProvince(CategoryId)\r\n union all\r\nselect tblComma.value Id , dbo.fn_getParentName(value) ItemName, dbo.fn_CheckCategoryCentralProvince(value) IsCentralOffice,\r\ncount(*) Count from (\r\nSELECT value , dbo.fn_getParentName(value) ItemName, dbo.fn_CheckCategoryCentralProvince(value) IsCentralOffice\r\nFROM DataEntries\r\nCROSS APPLY STRING_SPLIT(Moavenats, ',')\r\nwhere Moavenats is not null and DataEntryType = '4'\r\n) tblComma GROUP BY value , dbo.fn_getParentName(value), dbo.fn_CheckCategoryCentralProvince(value)\r\n) tblSumm\r\nGROUP BY Id , ItemName, IsCentralOffice\r\norder by Count desc";
        //    using (var connection = _dapperContext.CreateConnection())
        //    {
        //        var mokatebat = await connection.QueryAsync<MokatebeDashboardItemDto>(query);
        //        if (mokatebat.Count() == 0)
        //        {
        //            return null;
        //        }
        //        var groupedMokatebe = mokatebat.GroupBy(x => x.IsCentralOffice);

        //        int centralProvincesTotal = 0;

        //        var centralProvincesCheck = groupedMokatebe?.Where(q => q.Key == 1)?.ToList();

        //        if (centralProvincesCheck.Count > 0)
        //        {
        //            var centralProvincesCount =
        //                groupedMokatebe?.Where(q => q.Key == 1)?.ToList()[0].Sum(item => item.Count);
        //            centralProvincesTotal = (int)(centralProvincesCount);
        //        }

        //        var otherMokatebes = groupedMokatebe?.Where(q => q.Key == 0).ToList()[0].ToList();
        //        otherMokatebes?.Add(new MokatebeDashboardItemDto()
        //        {
        //            Count = centralProvincesTotal,
        //            ItemName = "ادارات کل استان‌ها"
        //        });
        //        return otherMokatebes;
        //    }
        //}

        //public async Task<List<DashboardItemDto>> Handle(GetDashboardSoalatByStatusQuery request, CancellationToken cancellationToken)
        //{
        //    var cuser = _currentUserService.GetCurrentUser();
        //    var dashboardData = await _context.Sovals.ToListAsync();


        //    var dastoorKarCommission = new DashboardItemDto()
        //    {
        //        Count = dashboardData.Count(x => x.QuestionStatus == QuestionStatusEnum.DastoorKarCommission),
        //        ItemName = "در دستور کار کمیسیون"
        //    };
        //    var tavigh = new DashboardItemDto()
        //    {
        //        Count = dashboardData.Count(x => x.QuestionStatus == QuestionStatusEnum.Tavigh),
        //        ItemName = "تعویق"
        //    };
        //    var emhaal = new DashboardItemDto()
        //    {
        //        Count = dashboardData.Count(x => x.QuestionStatus == QuestionStatusEnum.Baygani),
        //        ItemName = "بایگانی"
        //    };
        //    var elameVosool = new DashboardItemDto()
        //    {
        //        Count = dashboardData.Count(x => x.QuestionStatus == QuestionStatusEnum.Sahn_elameVosool),
        //        ItemName = "صحن(اعلام وصول)"
        //    };
        //    var darJalase = new DashboardItemDto()
        //    {
        //        Count = dashboardData.Count(x => x.QuestionStatus == QuestionStatusEnum.Convinced || x.QuestionStatus == QuestionStatusEnum.NotConincedThenConvinced || x.QuestionStatus == QuestionStatusEnum.NotConvincedYellowCard),
        //        ItemName = "صحن(در جلسه مطرح شده)"
        //    };
        //    var enseraf = new DashboardItemDto()
        //    {
        //        Count = dashboardData.Count(x => x.QuestionStatus == QuestionStatusEnum.Enseraf),
        //        ItemName = "انصراف"
        //    };
        //    var eghnaa = new DashboardItemDto()
        //    {
        //        Count = dashboardData.Count(x => x.QuestionStatus == QuestionStatusEnum.Eghnaa),
        //        ItemName = "اقناع"
        //    };

        //    return new List<DashboardItemDto> { dastoorKarCommission, tavigh, emhaal, elameVosool, darJalase, enseraf, eghnaa };

        //}

        //public async Task<List<DashboardItemDto>> Handle(GetDashboardSoalatByOrganizationQuery request, CancellationToken cancellationToken)
        //{
        //    //var cuser = _currentUserService.GetCurrentUser();
        //    //var dashboardData = await _unitOfWork.DataEntryRepository
        //    //    .NoTracking
        //    //    .Include(x => x.Soval)
        //    //    .Where(x => x.DataEntryType == DataEntryTypeEnum.Soval)
        //    //    .GroupBy(x => x.Category.ParentId)
        //    //    .Select(x => new DashboardItemDto() { ItemName = x.FirstOrDefault().Category.Name, Count = x.Count() })
        //    //    .ToListAsync(cancellationToken);


        //    var query = "select Id , ItemName, sum(Count) Count from(\r\nselect dbo.fn_getParentId(CategoryId) Id , dbo.fn_getParentName(CategoryId) ItemName ,count(*) Count from DataEntries  where CategoryId is not null and DataEntryType = '7'\r\ngroup by dbo.fn_getParentId(CategoryId), dbo.fn_getParentName(CategoryId)\r\n union all\r\nselect tblComma.value Id , dbo.fn_getParentName(value) ItemName, count(*) Count from (\r\nSELECT value , dbo.fn_getParentName(value) ItemName\r\nFROM DataEntries\r\nCROSS APPLY STRING_SPLIT(Moavenats, ',')\r\nwhere Moavenats is not null and DataEntryType = '7'\r\n) tblComma GROUP BY value , dbo.fn_getParentName(value)\r\n) tblSumm\r\nGROUP BY Id , ItemName\r\norder by Count desc";
        //    using (var connection = _dapperContext.CreateConnection())
        //    {
        //        var soalat = await connection.QueryAsync<DashboardItemDto>(query);
        //        return soalat.ToList();
        //    }
        //}

        //public async Task<List<DashboardItemDto>> Handle(GetDashboardSoalatByCityQuery request, CancellationToken cancellationToken)
        //{
        //    var cuser = _currentUserService.GetCurrentUser();
        //    var dashboardData = await _unitOfWork.DataEntryRepository
        //        .NoTracking
        //        .Include(x => x.Soval)
        //        .Include(x => x.Senator).ThenInclude(x => x.City)
        //        .Where(x => x.DataEntryType == DataEntryTypeEnum.Soval)
        //        .GroupBy(x => x.Senator.CityId)
        //        .Select(x => new DashboardItemDto() { ItemName = x.FirstOrDefault().Senator.City.Name, Count = x.Count() })
        //        .ToListAsync(cancellationToken);


        //    return dashboardData;
        //}

        //public async Task<List<DashboardItemDto>> Handle(GetDashboardMolaghatByMonthQuery request, CancellationToken cancellationToken)
        //{
        //    string[] months = { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };
        //    var cuser = _currentUserService.GetCurrentUser();

        //    var query = await _context.Molaghats
        //        .Where(x => x.Date > DateTime.Today.AddYears(-1) && x.Date <= DateTime.Today && x.Date != DateTime.MinValue)
        //        .ToListAsync();

        //    var dashboardData = query
        //        .Where(x => x.DateMonth != DateTime.Now.ToPersianDate().GetMonth() || x.Date.Year != DateTime.Now.Year - 1)
        //        .GroupBy(x => new { month = x.DateMonth, year = x.Date.ToPersianDate().GetYear() })
        //        .Select(x => new DashboardItemDto() { ItemName = x.FirstOrDefault().Date.ToPersianDate("yyyy/MM/dd").GetMonth(), Count = x.Sum(r => r.Count) })
        //        .ToList();

        //    var dashboardFinalData = new List<DashboardItemDto>();

        //    var currentMonth = Int16.Parse(DateTime.Now.ToPersianDate().Substring(5, 2));

        //    for (var i = currentMonth + 1; i < 13; i++)
        //    {
        //        var count = dashboardData.FirstOrDefault(item => item.ItemName == months[i - 1]) != null ? dashboardData.FirstOrDefault(item => item.ItemName == months[i - 1]).Count : 0;
        //        dashboardFinalData.Add(new DashboardItemDto() { ItemName = months[i - 1], Count = count });
        //    }

        //    for (var i = 1; i <= currentMonth; i++)
        //    {
        //        var count = dashboardData.FirstOrDefault(item => item.ItemName == months[i - 1]) != null ? dashboardData.FirstOrDefault(item => item.ItemName == months[i - 1]).Count : 0;
        //        dashboardFinalData.Add(new DashboardItemDto() { ItemName = months[i - 1], Count = count });
        //    }

        //    return dashboardFinalData;
        //}

        //public async Task<List<DashboardItemDto>> Handle(GetDashboardMolaghatByCityQuery request, CancellationToken cancellationToken)
        //{
        //    var cuser = _currentUserService.GetCurrentUser();
        //    var dashboardData = await _unitOfWork.DataEntryRepository
        //        .NoTracking
        //        .Include(x => x.Molaghat)
        //        .Include(x => x.Senator).ThenInclude(x => x.City)
        //        .Where(x => x.DataEntryType == DataEntryTypeEnum.Molaghat)
        //        .GroupBy(x => x.Senator.CityId)
        //        .Select(x => new DashboardItemDto() { ItemName = x.FirstOrDefault().Senator.City.Name, Count = x.Sum(r => r.Molaghat.Count) })
        //        .ToListAsync(cancellationToken);


        //    return dashboardData;
        //}

        //public async Task<List<DashboardMokatebeItemDto>> Handle(GetDashboardMokatebeByResponseStatusQuery request, CancellationToken cancellationToken)
        //{
        //    var cuser = _currentUserService.GetCurrentUser();
        //    var dashboardData = await _context.Mokatebes.ToListAsync();
        //    if (request.MokatebeType == MokatebeTypeEnum.PeyNevesht)
        //    {
        //        dashboardData = dashboardData.Where(q => q.MokatebeType == MokatebeTypeEnum.PeyNevesht).ToList();
        //    }
        //    if (dashboardData.Count == 0)
        //    {
        //        return new List<DashboardMokatebeItemDto> { 
        //        new DashboardMokatebeItemDto()
        //        {
        //            value = 0,
        //            category = "مثبت"
        //        },
        //        new DashboardMokatebeItemDto()
        //        {
        //            value = 0,
        //            category = "منفی"
        //        },
        //         new DashboardMokatebeItemDto()
        //        {
        //            value = 0,
        //            category = "ارائه‌گزارش"
        //        }
        //    };
        //}
        //    var answered = (dashboardData.Count(x => x.PasokhNo != null) * 100 / dashboardData.Count);


        //    var Mosbat = new DashboardMokatebeItemDto()
        //    {
        //        value = dashboardData.Count(x => x.VaziatPasokh == ResponseStatusEnum.Mosbat) * 100 / dashboardData.Count,
        //        category = "مثبت"
        //    };
        //    var Manfi = new DashboardMokatebeItemDto()
        //    {
        //        value = dashboardData.Count(x => x.VaziatPasokh == ResponseStatusEnum.Manfi) * 100 / dashboardData.Count,
        //        category = "منفی"
        //    };
        //    var Gozaresh = new DashboardMokatebeItemDto()
        //    {
        //        value = 100 - Manfi.value - Mosbat.value,
        //        category = "ارائه‌گزارش"
        //    };

        //    return new List<DashboardMokatebeItemDto> { Mosbat, Manfi, Gozaresh };
        //}

        //public async Task<List<DashboardMokatebeItemDto>> Handle(GetDashboardMokatebeByStatusQuery request, CancellationToken cancellationToken)
        //{
        //    var cuser = _currentUserService.GetCurrentUser();
        //    var dashboardData = await _context.Mokatebes.ToListAsync();
        //    if (request.MokatebeType == MokatebeTypeEnum.PeyNevesht)
        //    {
        //        dashboardData = dashboardData.Where(q => q.MokatebeType == MokatebeTypeEnum.PeyNevesht).ToList();
        //    }
        //    if (dashboardData.Count == 0)
        //    {
        //        return new List<DashboardMokatebeItemDto> {
        //            new DashboardMokatebeItemDto()
        //            {
        //                value = 0,
        //                category = "پاسخ داده"
        //            },
        //            new DashboardMokatebeItemDto()
        //            {
        //                value = 0,
        //                category = "بدون پاسخ"
        //            }
        //        };
        //    }
        //    var answered = (dashboardData.Count(x => x.PasokhNo != null) * 100 / dashboardData.Count);
        //    var pasokhDade = new DashboardMokatebeItemDto()
        //    {
        //        value = answered,
        //        category = "پاسخ داده"
        //    };
        //    var a = dashboardData.Count(x => x.PasokhNo == null) * 100;
        //    var c = dashboardData.Count;
        //    double b = a / c;
        //    var bedoonePasokh = new DashboardMokatebeItemDto()
        //    {
        //        value = 100 - answered,
        //        category = "بدون پاسخ"
        //    };



        //    return new List<DashboardMokatebeItemDto> { bedoonePasokh, pasokhDade };
        //}

        //public async Task<List<DashboardItemDto>> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
        //{
        //    var cuser = _currentUserService.GetCurrentUser();
        //    var dashboardUserActed = await _unitOfWork.DataEntryRepository
        //        .NoTracking
        //        .Where(x =>
        //        (x.ActionReferences.OrderByDescending(xx => xx.Created).LastOrDefault().FromUserId == cuser.BussinessUserId))
        //        .GroupBy(x => x.DataEntryType)
        //        .Select(x => new { x.Key, Count = x.Count() })
        //        .ToListAsync(cancellationToken);
        //    var dashboardInUserState = await _unitOfWork.DataEntryRepository
        //        .NoTracking
        //        .Where(x =>
        //        (x.ActionReferences.OrderByDescending(xx => xx.Created).LastOrDefault().ToUserId == cuser.BussinessUserId
        //            && x.ActionReferences.OrderByDescending(xx => xx.Created).LastOrDefault().FromUserId != cuser.BussinessUserId))
        //        .GroupBy(x => x.DataEntryType)
        //        .Select(x => new { x.Key, Count = x.Count() })
        //        .ToListAsync(cancellationToken);
        //    var dashboardCreatedByUser = await _unitOfWork.DataEntryRepository
        //        .NoTracking
        //        .Where(x => x.CreatorUserId == cuser.UserId || x.SenatorId == cuser.BussinessUserId)
        //        .GroupBy(x => x.DataEntryType)
        //        .Select(x => new { x.Key, Count = x.Count() })
        //        .ToListAsync(cancellationToken);
        //    var dUA = dashboardUserActed.Select(x => new DashboardItemDto()
        //    {
        //        Count = x.Count,
        //        ItemName = x.Key.GetDisplayName() + " اقدام شده",
        //        ItemType = x.Key
        //    });
        //    var dIUS = dashboardInUserState.Select(x => new DashboardItemDto()
        //    {
        //        Count = x.Count,
        //        ItemName = x.Key.GetDisplayName() + " در انتظار اقدام",
        //        ItemType = x.Key
        //    });
        //    var dCBU = dashboardCreatedByUser.Select(x => new DashboardItemDto()
        //    {
        //        Count = x.Count,
        //        ItemName = x.Key.GetDisplayName() + " ایجاد شده",
        //        ItemType = x.Key
        //    });
        //    return dUA
        //        .Union(dIUS)
        //        .Union(dCBU)
        //        .ToList();
        //}

        public async Task<DashboardLoanDto> Handle(GetDashboardLoanByStatusQuery request, CancellationToken cancellationToken)
        {
            string[] months = { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };
            var senatorId = request.SenatorId;

            var cuser = _currentUserService.GetCurrentUser();
            var dataEntryIds = await _context.ActionReferences
                .AsNoTracking()
                .Where(a => a.FromUserId == request.ShoraUserId && a.ActRefType == ActRefTypeEnum.Refer)
                .GroupBy(a => a.DataEntryId)
                .Select(g => g.Key)
                .ToListAsync();

            // سپس با Include کامل بگیرید
            var queryShora = _context.ActionReferences
                .AsNoTracking()
                .Include(a => a.DataEntry)
                .Where(a => dataEntryIds.Contains(a.DataEntryId)
                        && a.FromUserId == request.ShoraUserId
                        && a.ActRefType == ActRefTypeEnum.Refer);
            var query = _context.DataEntries
                .AsNoTracking()
                .Include(x=>x.Loan)
                .Include(x => x.ActionReferences).ThenInclude(x => x.FromUser)
                .Include(x => x.ActionReferences).ThenInclude(x => x.ToUser)
                .Where(x => x.DataEntryType == DataEntryTypeEnum.Loan);

            if (cuser.Roles.Any(e => e == RoleTypeEnum.Organization))
            {
                query = query.Where(q => (q.ActionReferences.Any(ar =>
                    ar.ToUserId == cuser.BussinessUserId)));

                queryShora = queryShora.Where(q=>q.ToUserId == cuser.BussinessUserId);
            }

            if (cuser.Roles.Any(e => e == RoleTypeEnum.Senator))
            {
                query = query.Where(x => x.SenatorId == cuser.BussinessUserId);
                queryShora = queryShora.Where(x => x.DataEntry.SenatorId == cuser.BussinessUserId);
            }

            try
            {
                var countOfRefrenceShora = queryShora
               .Count();
                //Mohasebe data kolli
                // تعداد کل وام‌ها
                var countOfAllLoans = query.Count();


                // تعداد وام‌های تایید شده
                var countOfApprovedLoan = query
                    .Where(de =>
                        de.Loan.VaziatPasokh == ResponseStatusEnum.Mosbat)
                    .Count();

                // تعداد وام‌های رد شده
                var countOfDeniedLoan = query
                    .Where(de =>
                        de.Loan.VaziatPasokh == ResponseStatusEnum.Manfi)
                    .Count();

                // تعداد وام‌های در حال بررسی
                var countOfInProgressLoan = query
                    .Where(de =>
                        de.Loan.VaziatPasokh == ResponseStatusEnum.Inprogress)
                    .Count();

                // تعداد وام‌های در حال بررسی
                var countOfInBranchRequest = query
                    .Where(de =>
                        de.Loan.VaziatPasokh == ResponseStatusEnum.Shobe)
                    .Count();

                // محاسبه تعداد تسهیلات رد شده توسط شعبه (از گزارش بانک‌ها)
                // این تعداد باید برابر با مجموع UnPaidCount از همه بانک‌ها باشد
                var tempTableForRejected = query
                      .Select(l => new
                      {
                          Loan = l.Loan,
                          CurrentUserId = l.ActionReferences.Where(a => a.ActRefType == ActRefTypeEnum.Refer)
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


                if (request.Status != null)
                {
                    query = query.Where(x => x.Loan.VaziatPasokh == request.Status);
                }

                var dataList = await query
                     .Select(de => new
                     {
                         CreatedDate = de.Created, // تاریخ اصلی
                         Amount = de.Loan.Amount
                     })
                     .ToListAsync();

                // گروه‌بندی و محاسبات
                var dashboardData = dataList
                    .Select(x => new
                    {
                        PersianDate = PersianDateTime.Parse(x.CreatedDate.ToPersianDate(), "/"), // تبدیل به تاریخ شمسی
                        Amount = x.Amount
                    })
                    .GroupBy(x => new { x.PersianDate.Year, x.PersianDate.Month })
                    .Select(g => new
                    {
                        Year = g.Key.Year,
                        Month = g.Key.Month,
                        TotalRequests = g.Count(),
                        TotalAmount = g.Sum(x => x.Amount),
                        AverageAmount = g.Average(x => x.Amount)
                    })
                    .OrderByDescending(x => x.Year)
                    .ThenByDescending(x => x.Month)
                    .ToList();

                // محاسبه تاریخ شمسی فعلی
                var currentPersianDate = PersianDateTime.Now;
                var currentYear = currentPersianDate.Year;
                var currentMonth = currentPersianDate.Month;

                var dashboardFinalData = new List<DashboardLoanItemDto>();

                // تولید ۱۲ ماه اخیر به ترتیب معکوس (جدیدترین اول)
                for (int i = 0; i < 12; i++)
                {
                    // محاسبه سال و ماه برای هر دوره
                    var targetMonth = currentMonth - i;
                    var targetYear = currentYear;

                    if (targetMonth <= 0)
                    {
                        targetMonth += 12;
                        targetYear--;
                    }

                    // پیدا کردن داده مربوط به این ماه و سال
                    var monthData = dashboardData.FirstOrDefault(x =>
                        x.Year == targetYear && x.Month == targetMonth);

                    // نام ماه بر اساس index (ماه شمسی از ۱ شروع می‌شود)
                    var monthName = months[targetMonth - 1];

                    dashboardFinalData.Add(new DashboardLoanItemDto
                    {
                        ItemName = monthName,
                        Count = monthData?.TotalRequests ?? 0,
                        ItemType = DataEntryTypeEnum.Loan,
                        TotalAmount = monthData?.TotalAmount ?? 0,
                    });
                }

                // معکوس کردن لیست تا قدیمی‌ترین ماه اول باشد
                dashboardFinalData.Reverse();


                DashboardLoanDto dto = new();
                dto.CountAllLoan = countOfAllLoans;
                dto.CountOfApprovedLoan = countOfApprovedLoan;
                dto.CountOfDeniedLoan = countOfDeniedLoan;
                dto.CountOfInProgressLoan = countOfInProgressLoan;
                dto.CountOfInBranchLoan = countOfInBranchRequest;
                dto.CountOfRejectedByBranch = rejectedByBranchCount; // NEW: تعداد تسهیلات رد شده توسط شعبه
                dto.DashboardChartData = dashboardFinalData;
                dto.CountShoraRefrences = countOfRefrenceShora;
                return dto;
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }
    }
}
