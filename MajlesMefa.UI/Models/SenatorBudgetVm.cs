using System.ComponentModel.DataAnnotations;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Enums.Senator;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MajlesMefa.UI.Models
{
    public class SenatorBudgetVm 
    {
        [Display(Name = "شناسه")]
        public Guid? Id { get; set; }

        [Display(Name = "مبلغ")]
        [Range(1, long.MaxValue, ErrorMessage = "{0} معتبر نیست")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public long Amount { get; set; }

        [Display(Name = "تاریخ")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string ExecDate { get; set; }

        [Display(Name = "نماینده مرتبط")]
        [NotEmptyGuid]
        public Guid SenatorId { get; set; }

        [Display(Name = "نوع وام")]
        public SenatorRequestLoanTypeEnum RequestLoanType { get; set; }

        [Display(Name = "نوع وام")]
        public int RequestLoanTypeInt { get { return (int)RequestLoanType; } set { RequestLoanType = (SenatorRequestLoanTypeEnum)value; } }

        [Display(Name = "کاربر بانک")]
        public Guid UserId { get; set; }
        public SelectList UserSelectList { get; set; }
        public List<SelectListItem> Senators { get; set; } = new();
        public List<SelectListItem> Users { get; set; } = new();
        public List<SenatorRequestLoanTypeEnum> LoanTypes { get; set; }

        public class NotEmptyGuid : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value is Guid g && g != Guid.Empty) return ValidationResult.Success;
                return new ValidationResult("پر کردن این فیلد اجباری است");
            }
        }
    }
}