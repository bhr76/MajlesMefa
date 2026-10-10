using AutoMapper;
using MajlesMefa.Back.ActionFilters;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Enums.Senator;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework;
using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;
using MajlesMefa.Back.Utilities.Limit;
using MajlesMefa.UI.Models;
using MajlesMefa.UI.Views.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.UI.Views.SenatorAnnualLoanBudget
{
    public class SenatorAnnualLoanBudgetController : BaseController
    {
        public const string CONTROLLER = "SenatorAnnualLoanBudget";
        private readonly RefahMajlesDbContext _context;

        public SenatorAnnualLoanBudgetController(IMapper mapper, RefahMajlesDbContext context) : base(mapper)
        {
            _context = context;
        }

        [Display(Name = "سقف سالانه تسهیلات نمایندگان")]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        public async Task<IActionResult> Index(Guid? senatorId)
        {
            ViewBag.CurrentPersianYear = new PersianCalendar().GetYear(DateTime.Now);
            if (senatorId.HasValue)
            {
                ViewBag.SenatorName = await _context.SenatorProfiles.Where(x => x.UserId == senatorId.Value).Select(x => x.Name).SingleOrDefaultAsync();
                if (ViewBag.SenatorName == null) return NotFound();
            }
            return View(senatorId);
        }

        [RequestLimit(NoOfRequest = 30, Seconds = 5)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        [HttpPost]
        public async Task<IActionResult> GetBudgets(string models, Guid? senatorId, CancellationToken cancellationToken)
        {
            await RefreshUsageAsync(cancellationToken);
            var filter = JsonConvert.DeserializeObject<TableRequestModel>(models) ?? new TableRequestModel();
            var query = _context.SenatorAnnualLoanBudgets.AsNoTracking().AsQueryable();
            if (senatorId.HasValue && senatorId.Value != Guid.Empty)
                query = query.Where(x => x.SenatorId == senatorId.Value);

            var table = await query.Select(x => new SenatorAnnualLoanBudgetRowVm
            {
                Id = x.Id,
                SenatorId = x.SenatorId,
                SenatorName = x.Senator.Name,
                Year = x.Year,
                GharzolHasaneLimit = x.GharzolHasaneLimit,
                MorabeheLimit = x.MorabeheLimit,
                GharzolHasaneUsed = x.GharzolHasaneUsed,
                MorabeheUsed = x.MorabeheUsed
            }).ToTableResultAsync(filter);
            return Json(DataSourceResult.GetFromTable(table));
        }

        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        public async Task<IActionResult> Create(Guid? senatorId)
        {
            var vm = new SenatorAnnualLoanBudgetVm
            {
                SenatorId = senatorId ?? Guid.Empty,
                Year = new PersianCalendar().GetYear(DateTime.Now)
            };
            await FillSenatorsAsync(vm);
            return View(vm);
        }

        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SenatorAnnualLoanBudgetVm request, CancellationToken cancellationToken)
        {
            if (request.GharzolHasaneLimit < 0 || request.MorabeheLimit < 0)
                ModelState.AddModelError(string.Empty, "سقف تسهیلات نمی‌تواند منفی باشد.");
            if (request.SenatorId == Guid.Empty || !await _context.SenatorProfiles.AnyAsync(x => x.UserId == request.SenatorId, cancellationToken))
                ModelState.AddModelError(nameof(request.SenatorId), "نمایندهٔ انتخاب‌شده معتبر نیست.");
            if (ModelState.IsValid && await _context.SenatorAnnualLoanBudgets.AnyAsync(x => x.SenatorId == request.SenatorId && x.Year == request.Year, cancellationToken))
                ModelState.AddModelError(nameof(request.Year), "برای این نماینده در این سال سقف ثبت شده است.");
            if (!ModelState.IsValid)
            {
                await FillSenatorsAsync(request);
                return View(request);
            }

            _context.SenatorAnnualLoanBudgets.Add(new SenatorAnnualLoanBudgetEntity
            {
                Id = Guid.NewGuid(),
                SenatorId = request.SenatorId,
                Year = request.Year,
                GharzolHasaneLimit = request.GharzolHasaneLimit,
                MorabeheLimit = request.MorabeheLimit
            });
            await _context.SaveChangesAsync(cancellationToken);
            return RedirectToAction(nameof(Index), new { senatorId = request.SenatorId });
        }

        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        public async Task<IActionResult> Edit(Guid id)
        {
            var item = await _context.SenatorAnnualLoanBudgets.AsNoTracking().Where(x => x.Id == id)
                .Select(x => new SenatorAnnualLoanBudgetVm
                {
                    Id = x.Id, SenatorId = x.SenatorId, SenatorName = x.Senator.Name, Year = x.Year,
                    GharzolHasaneLimit = x.GharzolHasaneLimit, MorabeheLimit = x.MorabeheLimit
                }).SingleOrDefaultAsync();
            if (item == null) return NotFound();
            return View(item);
        }

        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SenatorAnnualLoanBudgetVm request, CancellationToken cancellationToken)
        {
            var item = await _context.SenatorAnnualLoanBudgets.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (item == null) return NotFound();
            if (request.GharzolHasaneLimit < 0 || request.MorabeheLimit < 0)
                ModelState.AddModelError(string.Empty, "سقف تسهیلات نمی‌تواند منفی باشد.");
            if (ModelState.IsValid && await _context.SenatorAnnualLoanBudgets.AnyAsync(x => x.Id != item.Id && x.SenatorId == item.SenatorId && x.Year == request.Year, cancellationToken))
                ModelState.AddModelError(nameof(request.Year), "برای این نماینده در این سال سقف ثبت شده است.");
            if (!ModelState.IsValid)
            {
                request.SenatorId = item.SenatorId;
                request.SenatorName = await _context.SenatorProfiles.Where(x => x.UserId == item.SenatorId).Select(x => x.Name).SingleAsync(cancellationToken);
                return View(request);
            }

            item.Year = request.Year;
            item.GharzolHasaneLimit = request.GharzolHasaneLimit;
            item.MorabeheLimit = request.MorabeheLimit;
            await _context.SaveChangesAsync(cancellationToken);
            return RedirectToAction(nameof(Index), new { senatorId = item.SenatorId });
        }

        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var item = await _context.SenatorAnnualLoanBudgets.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (item == null) return NotFound();
            var senatorId = item.SenatorId;
            _context.SenatorAnnualLoanBudgets.Remove(item);
            await _context.SaveChangesAsync(cancellationToken);
            return Json(new { success = true, senatorId });
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkCreate(BulkCreateSenatorAnnualLoanBudgetVm request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "سال یا مبالغ واردشده معتبر نیست." });

            var senatorIds = await _context.SenatorProfiles.AsNoTracking().Select(x => x.UserId).Distinct().ToListAsync(cancellationToken);
            var existingSenatorIds = await _context.SenatorAnnualLoanBudgets
                .Where(x => x.Year == request.Year && senatorIds.Contains(x.SenatorId))
                .Select(x => x.SenatorId)
                .ToListAsync(cancellationToken);
            var existing = existingSenatorIds.ToHashSet();
            var missingSenatorIds = senatorIds.Where(id => !existing.Contains(id)).ToList();

            _context.SenatorAnnualLoanBudgets.AddRange(missingSenatorIds.Select(senatorId => new SenatorAnnualLoanBudgetEntity
            {
                Id = Guid.NewGuid(),
                SenatorId = senatorId,
                Year = request.Year,
                GharzolHasaneLimit = request.GharzolHasaneLimit,
                MorabeheLimit = request.MorabeheLimit
            }));

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException)
            {
                return Conflict(new { message = "ثبت گروهی به‌دلیل ثبت هم‌زمان سقف برای یکی از نمایندگان انجام نشد. صفحه را تازه‌سازی و دوباره بررسی کنید." });
            }

            return Json(new
            {
                success = true,
                created = missingSenatorIds.Count,
                skipped = existing.Count,
                message = $"برای {missingSenatorIds.Count:N0} نماینده رکورد ساخته شد؛ {existing.Count:N0} نماینده به‌دلیل داشتن رکورد در این سال رد شدند."
            });
        }

        [RequestLimit(NoOfRequest = 10, Seconds = 10)]
        [Auth(RoleTypeEnum.Admin, RoleTypeEnum.MinistryAdmin, RoleTypeEnum.MinistryMember)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkAdjust(BulkAdjustSenatorAnnualLoanBudgetVm request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid || (request.GharzolHasaneDelta == 0 && request.MorabeheDelta == 0))
                return BadRequest(new { message = "سال معتبر و حداقل یک تغییر غیرصفر وارد کنید." });

            var budgets = await _context.SenatorAnnualLoanBudgets
                .Include(x => x.Senator)
                .Where(x => x.Year == request.Year)
                .ToListAsync(cancellationToken);
            if (budgets.Count == 0)
                return BadRequest(new { message = "برای این سال هنوز رکوردی وجود ندارد." });

            try
            {
                foreach (var budget in budgets)
                {
                    var newGharzolLimit = checked(budget.GharzolHasaneLimit + request.GharzolHasaneDelta);
                    var newMorabeheLimit = checked(budget.MorabeheLimit + request.MorabeheDelta);
                    if (newGharzolLimit < 0 || newMorabeheLimit < 0)
                        return BadRequest(new { message = $"تغییر برای نمایندهٔ «{budget.Senator.Name}» سقف را منفی می‌کند؛ هیچ رکوردی تغییر نکرد." });
                }
            }
            catch (OverflowException)
            {
                return BadRequest(new { message = "مبلغ نهایی از محدودهٔ قابل ذخیره بیشتر می‌شود؛ هیچ رکوردی تغییر نکرد." });
            }

            foreach (var budget in budgets)
            {
                budget.GharzolHasaneLimit += request.GharzolHasaneDelta;
                budget.MorabeheLimit += request.MorabeheDelta;
            }
            await _context.SaveChangesAsync(cancellationToken);

            return Json(new
            {
                success = true,
                updated = budgets.Count,
                message = $"سقف‌های {budgets.Count:N0} نماینده برای سال {request.Year} به‌روزرسانی شد."
            });
        }

        private async Task FillSenatorsAsync(SenatorAnnualLoanBudgetVm model)
        {
            model.Senators = await _context.SenatorProfiles.AsNoTracking().OrderBy(x => x.Name)
                .Select(x => new SelectListItem { Value = x.UserId.ToString(), Text = x.Name, Selected = x.UserId == model.SenatorId })
                .ToListAsync();
        }

        private async Task RefreshUsageAsync(CancellationToken cancellationToken)
        {
            var budgets = await _context.SenatorAnnualLoanBudgets.ToListAsync(cancellationToken);
            if (budgets.Count == 0) return;

            var calendar = new PersianCalendar();
            var fromDate = calendar.ToDateTime(budgets.Min(x => x.Year), 1, 1, 0, 0, 0, 0);
            var toDate = calendar.ToDateTime(budgets.Max(x => x.Year) + 1, 1, 1, 0, 0, 0, 0);
            var senatorIds = budgets.Select(x => x.SenatorId).Distinct().ToList();
            var loans = await (from entry in _context.DataEntries
                               join loan in _context.Loans on entry.Id equals loan.DataEntryId
                               where entry.DataEntryType == DataEntryTypeEnum.Loan
                                  && entry.SenatorId.HasValue && senatorIds.Contains(entry.SenatorId.Value)
                                  && entry.Created >= fromDate && entry.Created < toDate
                                  && loan.VaziatPasokh != ResponseStatusEnum.Manfi
                               select new { SenatorId = entry.SenatorId.Value, entry.Created, loan.LoanType, loan.Amount })
                .ToListAsync(cancellationToken);

            var used = loans.GroupBy(x => new { x.SenatorId, Year = calendar.GetYear(x.Created), x.LoanType })
                .ToDictionary(x => x.Key, x => x.Sum(y => y.Amount));
            foreach (var budget in budgets)
            {
                used.TryGetValue(new { budget.SenatorId, budget.Year, LoanType = LoanTypeEnum.Gharzolhasane }, out var gharzolUsed);
                used.TryGetValue(new { budget.SenatorId, budget.Year, LoanType = LoanTypeEnum.Morabehe }, out var morabeheUsed);
                budget.GharzolHasaneUsed = gharzolUsed;
                budget.MorabeheUsed = morabeheUsed;
            }
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
