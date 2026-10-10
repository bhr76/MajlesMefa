using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MajlesMefa.UI.Models
{
    public class SenatorAnnualLoanBudgetVm
    {
        public Guid? Id { get; set; }

        [Display(Name = "نماینده")]
        [Required(ErrorMessage = "انتخاب نماینده الزامی است.")]
        public Guid SenatorId { get; set; }

        [Display(Name = "سال شمسی")]
        [Range(1300, 1500, ErrorMessage = "سال شمسی معتبر نیست.")]
        public int Year { get; set; }

        [Display(Name = "سقف قرض‌الحسنه (تومان)")]
        [Range(typeof(long), "0", "9223372036854775807", ErrorMessage = "مبلغ باید صفر یا بیشتر باشد.")]
        public long GharzolHasaneLimit { get; set; }

        [Display(Name = "سقف مرابحه (تومان)")]
        [Range(typeof(long), "0", "9223372036854775807", ErrorMessage = "مبلغ باید صفر یا بیشتر باشد.")]
        public long MorabeheLimit { get; set; }

        public string SenatorName { get; set; }
        public List<SelectListItem> Senators { get; set; } = new();
    }

    public class SenatorAnnualLoanBudgetRowVm
    {
        public Guid Id { get; set; }
        public Guid SenatorId { get; set; }
        public string SenatorName { get; set; }
        public int Year { get; set; }
        public long GharzolHasaneLimit { get; set; }
        public long MorabeheLimit { get; set; }
        public long GharzolHasaneUsed { get; set; }
        public long MorabeheUsed { get; set; }
    }
    public class BulkCreateSenatorAnnualLoanBudgetVm
    {
        [Range(1300, 1500, ErrorMessage = "سال شمسی معتبر نیست.")]
        public int Year { get; set; }

        [Range(typeof(long), "0", "9223372036854775807", ErrorMessage = "مبلغ نمی‌تواند منفی باشد.")]
        public long GharzolHasaneLimit { get; set; }

        [Range(typeof(long), "0", "9223372036854775807", ErrorMessage = "مبلغ نمی‌تواند منفی باشد.")]
        public long MorabeheLimit { get; set; }
    }

    public class BulkAdjustSenatorAnnualLoanBudgetVm
    {
        [Range(1300, 1500, ErrorMessage = "سال شمسی معتبر نیست.")]
        public int Year { get; set; }
        public long GharzolHasaneDelta { get; set; }
        public long MorabeheDelta { get; set; }
    }
}
