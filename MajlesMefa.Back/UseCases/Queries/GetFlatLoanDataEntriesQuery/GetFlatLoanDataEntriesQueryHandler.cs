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

            // Apply Filters
            query = ApplyFilters(query, request, cuser);

            // متغیرهای local برای استفاده در LINQ query
            var cuserBusinessId = cuser.BussinessUserId;
            var isAdminRole = cuser.Roles.Any(x =>
                x == RoleTypeEnum.MinistryAdmin ||
                x == RoleTypeEnum.Admin ||
                x == RoleTypeEnum.MinistryMember);

            // Select و Map به FlatDataEntryDto
            var result = await query.Select(x => new FlatDataEntryDto()
            {
                // ============ فیلدهای اصلی DataEntry ============
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                DataEntryType = x.DataEntryType,
                Moavenats = x.Moavenats != null
                    ? x.Moavenats.Split(",", StringSplitOptions.None).ToList()
                    : null,
                CreatorUserName = x.Creator.Name,
                PersianCreatedDate = x.Created.ToPersianDate("yyyy/MM/dd"),

                // ============ Category ============
                CategoryId = x.Category.ParentId != null ? x.CategoryId : null,
                CategoryName = x.Category.ParentId != null ? x.Category.Name : null,
                CategoryParentId = x.Category.ParentId != null ? x.Category.ParentId : x.CategoryId,
                CategoryParentName = x.Category.ParentId != null
                    ? x.Category.Parent.Name
                    : x.Category.Name,

                // ============ Senator Information (فلت شده) ============
                SenatorName = x.Senator != null ? x.Senator.SenatorProfile.Name : null,
                SenatorHozeEntekhabi = x.Senator != null ? x.Senator.SenatorProfile.HozeCity.Name : null,
                SenatorHozeEntekhabiEnum = x.Senator != null ? x.Senator.SenatorProfile.HozeEntekhabi : null,
                SenatorCity = x.Senator != null ? x.Senator.City.Name : null,
                SenatorUserId = x.SenatorId,

                // ============ Loan Owner Basic Info ============
                LoanOwnerFullName = x.Loan.FullName,
                LoanOwnerMobile = x.Loan.MobileNo,
                LoanOwnerNationalCode = x.Loan.NationalNo,
                TrackingCode = x.Loan.TrackingCode.ToString(),

                // ============ Loan Details (قبلاً MyData.FullName بود، الان FullName) ============
                FullName = x.Loan.FullName,
                NationalNo = x.Loan.NationalNo,
                MobileNo = x.Loan.MobileNo,
                Amount = x.Loan.Amount.ShowCurrencyFormat(),
                LoanType = x.Loan.LoanType,
                PasokhNo = x.Loan.PasokhNo,
                PasokhState = x.Loan.VaziatPasokh,
                TrackingCodeInt = x.Loan.TrackingCode,

                // ============ Action Reference Information ============
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

                // ============ Access Rights (محاسبه inline) ============
                CanAccessActionRefrence = isAdminRole ||
                    x.ActionReferences
                        .Where(a => a.ActRefType == ActRefTypeEnum.Refer)
                        .OrderByDescending(a => a.Created)
                        .Select(a => a.ToUserId)
                        .FirstOrDefault() == cuserBusinessId,

                AccessActionRefrenceMessage = null

            }).ToTableResultAsync(request.Filter);

            // ============ Post-processing ============
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

        /// <summary>
        /// دریافت عناوین Moavenats (برای استفاده تکی)
        /// </summary>
        private async Task<List<string>> GetMoavenatTitles(List<string> moavenatIds)
        {
            if (moavenatIds == null || !moavenatIds.Any())
                return new List<string>();

            var guidIds = moavenatIds
                .Where(id => Guid.TryParse(id, out _))
                .Select(id => Guid.Parse(id))
                .ToList();

            if (!guidIds.Any())
                return new List<string>();

            return await _context.Categories
                .Where(x => guidIds.Contains(x.Id))
                .Select(x => x.Name)
                .ToListAsync();
        }

        #endregion
    }
}