using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Dtos.Common;
using MajlesMefa.Back.Dtos.Common.Grid;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Services.Abstractioin;
using MajlesMefa.Back.Utilities.Convertor;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MajlesMefa.Back.Utilities.Db.Pagination;
using MajlesMefa.Back.Utilities.EnumHelper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MajlesMefa.Back.UseCases.Queries.GetFlatLoanDataEntriesQuery
{
    public class GetFlatLoanDataEntriesQueryHandler : IRequestHandler<GetFlatLoanDataEntriesQuery, TableModel<FlatDataEntryDto>>
    {
        private readonly RefahMajlesDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetFlatLoanDataEntriesQueryHandler(
            RefahMajlesDbContext context,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<TableModel<FlatDataEntryDto>> Handle(
        GetFlatLoanDataEntriesQuery request,
        CancellationToken cancellationToken)
        {
            var cuser = _currentUserService.GetCurrentUser();

            if (request.Filter != null && request.Filter.Filter != null && request.Filter.Filter.Filters != null)
            {
                // ۱. پیدا کردن فیلتر سال
                var yearFilter = request.Filter.Filter.Filters.FirstOrDefault(x =>
                    string.Equals(x.Field, "year", StringComparison.OrdinalIgnoreCase));

                if (yearFilter != null)
                {
                    // ۲. مقدار عددی سال را در request.Year ذخیره می‌کنیم
                    if (int.TryParse(yearFilter.Value?.ToString(), out var parsedYear))
                    {
                        request.Year = parsedYear;
                    }

                    // ۳. حذف فیلتر سال با استفاده از متد Remove خود شیء فیلتر (در صورت پشتیبانی کلاس) 
                    // یا بازسازی لیست فیلترها بدون فیلتر سال به روش کاملاً سازگار:
                    var filteredList = request.Filter.Filter.Filters
                        .Where(x => !string.Equals(x.Field, "year", StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    // اختصاص لیست فیلترهای تصفیه شده
                    request.Filter.Filter.Filters = filteredList;

                    // ۴. در صورتی که لیست فیلترها کاملاً خالی شد، جهت جلوگیری از ارجاع Null و خطای کلید در Dynamic LINQ، 
                    // کل شیء Filter داخلی را null می‌کنیم تا کوئری بدون فیلتر اضافه اجرا شود.
                    if (!request.Filter.Filter.Filters.Any())
                    {
                        request.Filter.Filter = null;
                    }
                }
            }

            // Base Query با Include های لازم
            var query = _context.DataEntries
                .AsNoTracking()
                .Include(x => x.ActionReferences)
                    .ThenInclude(x => x.FromUser)
                .Include(x => x.ActionReferences)
                    .ThenInclude(x => x.ToUser)
                        .ThenInclude(x => x.UserRoles)
                            .ThenInclude(x => x.Role)
                .Include(x => x.Category)
                    .ThenInclude(x => x.Parent)
                .Include(x => x.Senator)
                    .ThenInclude(x => x.SenatorProfile)
                        .ThenInclude(x => x.HozeCity)
                .Include(x => x.Senator)
                    .ThenInclude(x => x.City)
                .Include(x => x.Loan)
                .Include(x => x.Creator)
                .Where(x => x.DataEntryType == DataEntryTypeEnum.Loan);

            // Apply Permissions
            query = ApplyPermissions(query, cuser);

            // Apply Filters (فیلتر سال در اینجا به صورت بازه میلادی اعمال می‌شود)
            query = ApplyFilters(query, request, cuser);

            var cuserBusinessId = cuser.BussinessUserId;
            var isAdminRole = cuser.Roles.Any(x =>
                x == RoleTypeEnum.MinistryAdmin ||
                x == RoleTypeEnum.Admin ||
                x == RoleTypeEnum.MinistryMember);

            var baseQuery = query.Select(x => new FlatDataEntryDto()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                DataEntryType = x.DataEntryType,
                Moavenats = x.Moavenats != null
                    ? x.Moavenats.Split(",", StringSplitOptions.None).ToList()
                    : null,
                CreatorUserName = x.Creator.Name,
                PersianCreatedDate = x.Created.ToPersianDate("yyyy/MM/dd"),

                CategoryId = x.Category.ParentId != null ? x.CategoryId : null,
                CategoryName = x.Category.ParentId != null ? x.Category.Name : null,
                CategoryParentId = x.Category.ParentId != null ? x.Category.ParentId : x.CategoryId,
                CategoryParentName = x.Category.ParentId != null
                    ? x.Category.Parent.Name
                    : x.Category.Name,

                SenatorName = x.Senator != null ? x.Senator.SenatorProfile.Name : null,
                SenatorHozeEntekhabi = x.Senator != null ? x.Senator.SenatorProfile.HozeCity.Name : null,
                SenatorHozeEntekhabiEnum = x.Senator != null ? x.Senator.SenatorProfile.HozeEntekhabi : null,
                SenatorCity = x.Senator != null ? x.Senator.City.Name : null,
                SenatorUserId = x.SenatorId,

                LoanOwnerFullName = x.Loan.FullName,
                LoanOwnerMobile = x.Loan.MobileNo,
                LoanOwnerNationalCode = x.Loan.NationalNo,
                TrackingCode = x.Loan.TrackingCode.ToString(),

                FullName = x.Loan.FullName,
                NationalNo = x.Loan.NationalNo,
                MobileNo = x.Loan.MobileNo,
                Amount = x.Loan.Amount.ShowCurrencyFormat(),
                LoanType = x.Loan.LoanType,
                PasokhNo = x.Loan.PasokhNo,
                PasokhState = x.Loan.VaziatPasokh,
                TrackingCodeInt = x.Loan.TrackingCode,

                ActionRefrenceDate = x.ActionReferences
                    .Where(a => a.ToUser != null &&
                               a.ToUser.Name != null &&
                               a.ToUser.UserRoles.Any(ur => ur.Role.RoleType == RoleTypeEnum.Organization))
                    .OrderByDescending(a => a.Created)
                    .Select(a => a.Created.ToPersianDate("yyyy/MM/dd"))
                    .FirstOrDefault() ?? "-",

                SuggestedBankName = x.ActionReferences
                    .Where(a => a.ToUser != null &&
                               a.ToUser.UserRoles.Any(ur => ur.Role.RoleType == RoleTypeEnum.Organization))
                    .OrderByDescending(a => a.Created)
                    .Select(a => a.ToUser.Name)
                    .FirstOrDefault(),

                SuggestedBankId = x.ActionReferences
                    .Where(a => a.ToUser != null &&
                               a.ToUser.UserRoles.Any(ur => ur.Role.RoleType == RoleTypeEnum.Organization))
                    .OrderByDescending(a => a.Created)
                    .Select(a => a.ToUserId)
                    .FirstOrDefault(),

                CanAccessActionRefrence = isAdminRole ||
                    x.ActionReferences
                        .Where(a => a.ActRefType == ActRefTypeEnum.Refer)
                        .OrderByDescending(a => a.Created)
                        .Select(a => a.ToUserId)
                        .FirstOrDefault() == cuserBusinessId,

                AccessActionRefrenceMessage = null
            });

            if (request.Filter != null)
            {
                DynamicQueryFilterNormalizer.NormalizeFilters(request.Filter, typeof(FlatDataEntryDto));
            }

            var result = await baseQuery.ToTableResultAsync(request.Filter);

            await ApplyPostProcessing(result.Items);

            return result;
        }

        #region Private Methods - Permissions & Filters

        /// <summary>
        /// اعمال دسترسی‌های کاربر
        /// </summary>
        private IQueryable<DataEntryEntity> ApplyPermissions(
            IQueryable<DataEntryEntity> query,
            CurrentUserDto cuser)
        {
            // Ministry Member Permission
            if (cuser.Roles.Any(u => u == RoleTypeEnum.MinistryMember))
            {
                query = query.Where(q =>
                    q.ActionReferences.Any(ar =>
                        ar.FromUserId == cuser.BussinessUserId ||
                        ar.ToUserId == cuser.BussinessUserId) ||
                    q.ActionReferences.FirstOrDefault().FromUser.OrganizationId != null);
            }

            // Senator Permission
            if (cuser.Roles.Any(e => e == RoleTypeEnum.Senator))
            {
                query = query.Where(x => x.SenatorId == cuser.BussinessUserId);
            }

            // Organization Permission
            if (cuser.Roles.Any(e => e == RoleTypeEnum.Organization))
            {
                query = query.Where(q =>
                    q.ActionReferences.Any(ar => ar.ToUserId == cuser.BussinessUserId));
            }

            return query;
        }

        /// <summary>
        /// اعمال فیلترهای درخواستی
        /// </summary>
        private IQueryable<DataEntryEntity> ApplyFilters(
            IQueryable<DataEntryEntity> query,
            GetFlatLoanDataEntriesQuery request,
            CurrentUserDto cuser)
        {
            // Filter by DataEntryId
            if (request.DataEntryId.HasValue)
            {
                query = query.Where(x => x.Id == request.DataEntryId);
            }

            // Filter by SenatorId
            if (request.SenatorId.HasValue && !Guid.Empty.Equals(request.SenatorId))
            {
                query = query.Where(x => x.SenatorId == request.SenatorId);
            }

            // Filter by RelatedSenatorId
            if (request.RelatedSenatorId.HasValue)
            {
                query = query.Where(q => q.SenatorId == request.RelatedSenatorId);
            }

            // Filter by CategoryId
            if (request.CategoryId.HasValue)
            {
                query = query.Where(q => q.CategoryId == request.CategoryId);
            }

            // Filter by Title (Search)
            if (!string.IsNullOrEmpty(request.Title))
            {
                query = query.Where(q => q.Title.Contains(request.Title));
            }

            // Filter by IsNeedUserAction
            if (request.IsNeedUserAction)
            {
                query = query.Where(q =>
                    q.ActionReferences
                        .OrderByDescending(x => x.Created)
                        .First().ToUserId == cuser.UserId &&
                    q.ActionReferences
                        .OrderByDescending(x => x.Created)
                        .First().FromUserId != cuser.UserId);
            }

            // Filter by IsBelongCurrentUser
            if (request.IsBelongCurrentUser)
            {
                query = query.Where(q =>
                    q.ActionReferences
                        .OrderBy(x => x.Created)
                        .First().FromUserId == cuser.UserId);
            }

            // Filter by CurrentUserId and InProgress Status
            if (request.CurrentUserId.HasValue)
            {
                query = query.Where(x =>
                    x.ActionReferences
                        .Where(a => a.ActRefType == ActRefTypeEnum.Refer)
                        .OrderByDescending(a => a.Created)
                        .Select(a => a.ToUserId)
                        .FirstOrDefault() == request.CurrentUserId);

                query = query.Where(x => x.Loan.VaziatPasokh == ResponseStatusEnum.Inprogress);
            }
            if (request.Year.HasValue)
            {
                if (request.Year.Value == 1404)
                {
                    var fromDate = new DateTime(2025, 3, 21, 0, 0, 0, DateTimeKind.Utc);
                    var toDate = new DateTime(2026, 3, 20, 23, 59, 59, DateTimeKind.Utc);
                    query = query.Where(x => x.Created >= fromDate && x.Created <= toDate);
                }
                else if (request.Year.Value == 1405)
                {
                    var fromDate = new DateTime(2026, 3, 21, 0, 0, 0, DateTimeKind.Utc);
                    var toDate = new DateTime(2027, 3, 20, 23, 59, 59, DateTimeKind.Utc);
                    query = query.Where(x => x.Created >= fromDate && x.Created <= toDate);
                }
            }


            return query;
        }

        #endregion

        #region Private Methods - Post Processing

        /// <summary>
        /// پردازش‌های بعد از Query (Moavenats و غیره)
        /// </summary>
        private async Task ApplyPostProcessing(List<FlatDataEntryDto> items)
        {
            if (items == null || !items.Any())
                return;

            // جمع‌آوری تمام Moavenat IDs یکجا
            var allMoavenatIds = items
                .Where(x => x.Moavenats != null && x.Moavenats.Any())
                .SelectMany(x => x.Moavenats)
                .Where(id => Guid.TryParse(id, out _))
                .Select(id => Guid.Parse(id))
                .Distinct()
                .ToList();

            if (!allMoavenatIds.Any())
                return;

            // یک Query برای گرفتن تمام عناوین
            var moavenatTitles = await _context.Categories
                .Where(x => allMoavenatIds.Contains(x.Id))
                .Select(x => new { x.Id, x.Name })
                .ToDictionaryAsync(x => x.Id.ToString(), x => x.Name);

            // Apply کردن عناوین به آیتم‌ها
            foreach (var item in items.Where(x => x.Moavenats != null && x.Moavenats.Any()))
            {
                var titles = item.Moavenats
                    .Where(id => moavenatTitles.ContainsKey(id))
                    .Select(id => moavenatTitles[id])
                    .ToList();

                if (titles.Any())
                {
                    item.CategoryParentName = string.Join(" - ", titles);
                }
            }
        }

        
        #endregion
    }
}