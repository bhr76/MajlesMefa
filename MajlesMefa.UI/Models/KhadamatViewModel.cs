using AutoMapper;
using MajlesMefa.Back.Dtos;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.UseCases.Commmands.CreateNewDataEntryCommand;
using MajlesMefa.Back.UseCases.Commmands.UpdateDataEntryCommand;
using MajlesMefa.Back.Utilities.Mapping;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.UI.Models
{
    public class KhadamatVm :IModifyDataEntryDto
    {
        [Display(Name = "نماینده مرتبط")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [NotEmptyGuid]
        public Guid SenatorId { get; set; }



        [Display(Name = "عنوان خدمت")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string Title { get;  set; }

        [Display(Name = "توضیحات")]
        
        public string Description { get; set; }

        [Display(Name = "نماینده مرتبط")]
        public string? SenatorName { get; set; }

        [Display(Name = "حوزه انتخابی نماینده")]
        public string HozeEntekhabi { get; set; }

        [Display(Name = "استان نماینده")]
        public string SenatorCity { get; set; }

        [Display(Name = "تاریخ")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string TarikhKhedmat { get; set; }


        public KhadamatDetailDto KhedmatData{ get; set; }



        public CreateNewDataEntryCommand ConvertToCommand()
        {
            return new CreateNewDataEntryCommand()
            {
                DataEntryData = KhedmatData,
                CategoryId =null,
                Moavenats = null,
                Title = Title,
                Description =Description,
                DataEntryType = DataEntryTypeEnum.Khadamat,
                SenatorId = SenatorId,
            };
        }

        public UpdateDataEntryCommand ConvertToUpdateCommand()
        {
            return new UpdateDataEntryCommand()
            {
                DataEntryData = KhedmatData,
                CategoryId = null,
                Moavenats = null,
                Title = Title,
                Description = Description,
                DataEntryType = DataEntryTypeEnum.Khadamat,
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
