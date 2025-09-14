using AutoMapper;
using MediatR;
using MajlesMefa.Back.Enums;
using System.ComponentModel.DataAnnotations;
using MajlesMefa.Back.UseCases.Commmands.CreateNewDataEntryCommand;
using MajlesMefa.Back.UseCases.Commmands.UpdateDataEntryCommand;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details;
using static MajlesMefa.UI.Models.SoalVm;

namespace MajlesMefa.UI.Models
{
    public class MolaghatVm 
    {
      

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "زیر موضوع")]
        public Guid? CategoryId { get; set; }

        [Display(Name = "موضوع")]
        public Guid? CategoryParentId { get; set; }

        [Display(Name = "نماینده مرتبط")]
        [Required]
        [NotEmptyGuid]
        public Guid SenatorId { get; set; }

        [Display(Name = "نماینده مرتبط")]
        public string? SenatorName { get; set; }

        [Display(Name = "حوزه انتخابی نماینده")]
        public string HozeEntekhabi { get; set; }

        [Display(Name = "استان نماینده")]
        public string SenatorCity { get; set; }

        [Display(Name = "تاریخ")]
        public string Date { get; set; }

        [Display(Name = "تاریخ پاسخ")]
        public string PasokhDate { get; set; }


        public MolaghatDtailDto MolaghatData { get; set; }


        public CreateNewDataEntryCommand ConvertToCommand()
        {
            return new CreateNewDataEntryCommand()
            {
                DataEntryData = MolaghatData,
                CategoryId = CategoryId,
                Title = "",
                Description = Description,
                SenatorId = SenatorId,
                DataEntryType = DataEntryTypeEnum.Molaghat,
            };
        }

        public UpdateDataEntryCommand ConvertToUpdateCommand()
        {
            return new UpdateDataEntryCommand()
            {
                DataEntryData = MolaghatData,
                CategoryId = CategoryId,
                Title = "",
                SenatorId = SenatorId,
                Description = Description,
                DataEntryType = DataEntryTypeEnum.Molaghat,
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