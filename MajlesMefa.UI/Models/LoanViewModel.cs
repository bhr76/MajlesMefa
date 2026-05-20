using MajlesMefa.Back.Enums;
using System.ComponentModel.DataAnnotations;
using MajlesMefa.Back.UseCases.Commmands.CreateNewDataEntryCommand;
using MajlesMefa.Back.UseCases.Commmands.UpdateDataEntryCommand;
using Microsoft.AspNetCore.Mvc.Rendering;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details;

namespace MajlesMefa.UI.Models
{
    public class LoanVm 
    {
        
        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "نماینده مرتبط")]
        public Guid SenatorId { get; set; }

        [Display(Name = "نماینده مرتبط")]
        public string? SenatorName { get; set; }

        [Display(Name = "حوزه انتخابی نماینده")]
        public string HozeEntekhabi { get; set; }

        [Display(Name = "استان نماینده")]
        public string SenatorCity { get; set; }

        [Display(Name = "باقیمانده بودجه")]
        public long RemainBudget { get; set; }


        public Guid ProvinceId { get; set; }
        public Guid CityId { get; set; }

        
        public string Address { get; set; }

        public LoanDtailDto LoanData { get; set; }

        [ Display(Name = "کاربر جهت ارجاع")]
        public Guid? UserId { get; set; }
        public SelectList UserSelectList { get; set; }
        public List<SelectListItem> Users { get; set; } = new();

        public CreateNewDataEntryCommand ConvertToCommand()
        {
            return new CreateNewDataEntryCommand()
            {
                DataEntryData = LoanData,
                Title = "",
                CategoryId = null,
                Moavenats = null,
                Description = Description,
                DataEntryType = DataEntryTypeEnum.Loan,
                SenatorId = SenatorId,
                
            };
        }

        public UpdateDataEntryCommand ConvertToUpdateCommand()
        {
            return new UpdateDataEntryCommand()
            {
                DataEntryData = LoanData,
                Title = "",
                CategoryId = null,
                Moavenats = null,
                Description = Description,
                DataEntryType = DataEntryTypeEnum.Loan,
                SenatorId = SenatorId,
            };
        }

        public class NotEmptyGuid : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value == null || (Guid)value == Guid.Empty)
                {
                    return new ValidationResult("پر کردن این فیلد اجباری است");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
        }


    }
}