using AutoMapper;
using MediatR;
using MajlesMefa.Back.Enums;
using System.ComponentModel.DataAnnotations;
using MajlesMefa.Back.UseCases.Commmands.CreateNewDataEntryCommand;
using MajlesMefa.Back.UseCases.Commmands.UpdateDataEntryCommand;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details;
using MajlesMefa.Back.Utilities.Date;

namespace MajlesMefa.UI.Models
{
    public class EzharatVm 
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string Description { get; set; }

        [Display(Name = "موضوع")]
        public Guid? CategoryId { get; set; }

        [Display(Name = "معاونت مشترک")]
        public bool Moshtarak { get; set; }

        [Display(Name = "معاونت‌ها")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public List<Guid> Moavenats { get; set; }

        public string? CategoryName { get; set; }

        [Display(Name = "معاونت")]
        public Guid? CategoryParentId { get; set; }

        [Display(Name = "معاونت")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string CategoryParentName { get; set; }

        [Display(Name = "نماینده مرتبط")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [NotEmptyGuid]
        public Guid SenatorId { get; set; }

        [Display(Name = "حوزه انتخابی نماینده")]
        public string HozeEntekhabi { get; set; }

        [Display(Name = "استان نماینده")]
        public string SenatorCity { get; set; }

        [Display(Name = "نماینده مرتبط")]
        public string? SenatorName { get; set; }

        [Display(Name = "تاریخ")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string Date { get; set; }


        public EzhaaratResaneeeDetailDto EzharatData { get; set; }


        public CreateNewDataEntryCommand ConvertToCommand()
        {
            return new CreateNewDataEntryCommand()
            {
                DataEntryData = EzharatData,
                CategoryId = CategoryId.HasValue ? CategoryId : CategoryParentId,
                Title = Title,
                Description = Description,
                DataEntryType = DataEntryTypeEnum.EzhaaratResaneee,
                Moavenats = Moavenats,
                SenatorId = SenatorId
            };
        }

        public UpdateDataEntryCommand ConvertToUpdateCommand()
        {
            return new UpdateDataEntryCommand()
            {
                DataEntryData = EzharatData,
                CategoryId = CategoryId.HasValue ? CategoryId : CategoryParentId,
                Title = Title,
                Description = Description,
                SenatorId = SenatorId,
                Moavenats = Moavenats,
                DataEntryType = DataEntryTypeEnum.EzhaaratResaneee,
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