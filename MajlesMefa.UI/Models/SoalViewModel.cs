using MajlesMefa.Back.Enums;
using System.ComponentModel.DataAnnotations;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.UseCases.Commmands.CreateNewDataEntryCommand;
using MajlesMefa.Back.UseCases.Commmands.UpdateDataEntryCommand;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details;
using Duende.IdentityServer.Models;

namespace MajlesMefa.UI.Models
{
    public class SoalVm
    {
        [Display(Name = "شناسه")]
        public Guid Id { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string Title { get; set; }

        [Display(Name = "معاونت مشترک")]
        public bool Moshtarak { get; set; }

        [Display(Name = "معاونت‌ها")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public List<Guid> Moavenats { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "موضوع")]
        public Guid? CategoryId { get; set; }

        public string? CategoryName { get; set; }

        [Display(Name = "معاونت")]
        public Guid? CategoryParentId { get; set; }

        [Display(Name = "معاونت")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string CategoryParentName { get; set; }

        [Display(Name = "تاریخ کاربرگ سوال")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string KarbargDate { get; set; }

        [Display(Name = "نماینده مرتبط")]
        [Required]
        [NotEmptyGuid]
        public Guid SenatorId { get; set; }

        [Display(Name = "حوزه انتخابی نماینده")]
        public string HozeEntekhabi { get; set; }

        [Display(Name = "استان نماینده")]
        public string SenatorCity { get; set; }

        [Display(Name = "نماینده مرتبط")]
        public string? SenatorName { get; set; }

        public SovalDetailDto SoalData { get; set; }


        public CreateNewDataEntryCommand ConvertToCommand()
        {
            return new CreateNewDataEntryCommand()
            {
                DataEntryData = SoalData,
                CategoryId = CategoryId.HasValue ? CategoryId : CategoryParentId,
                SenatorId = SenatorId,
                Title = Title,
                Moavenats = Moavenats,
                Description = Description,
                DataEntryType = DataEntryTypeEnum.Soval,
            };
        }

        public UpdateDataEntryCommand ConvertToUpdateCommand()
        {
            return new UpdateDataEntryCommand()
            {
                DataEntryData = SoalData,
                CategoryId = CategoryId.HasValue ? CategoryId : CategoryParentId,
                SenatorId = SenatorId,
                Title = Title,
                Moavenats = Moavenats,
                Description = Description,
                DataEntryType = DataEntryTypeEnum.Soval,
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