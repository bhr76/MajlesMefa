using AutoMapper;
using MediatR;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Utilities.Mapping;
using System.ComponentModel.DataAnnotations;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.UseCases.Commmands.CreateNewDataEntryCommand;
using MajlesMefa.Back.UseCases.Commmands.UpdateDataEntryCommand;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MajlesMefa.UI.Models
{
    public class MokatebeVm 
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string Title { get; set; }

        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string Description { get; set; }

        [Display(Name = " موضوع")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public Guid? CategoryId { get; set; }

        public string? CategoryName { get; set; }

        [Display(Name = "معاونت")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public Guid? CategoryParentId { get; set; }

        [Display(Name = "معاونت‌ها")]
        
        public List<Guid> Moavenats { get; set; }

        [Display(Name = "معاونت")]
        
        public string CategoryParentName { get; set; }

        [Display(Name = "تاریخ پیگیری")]
        public string TarikhPeygiri { get; set; }

        [Display(Name = "تاریخ  دبیرخانه")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string TarikhDabirKhane { get; set; }

        [Display(Name = "تاریخ نامه نماینده")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string TarikhNamenamaynde { get; set; }

        [Display(Name = "تاریخ پاسخ")]
        public string PasokhDate { get; set; }

        [Display(Name = "نماینده مرتبط")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [NotEmptyGuid]
        public Guid SenatorId { get; set; }

        [Display(Name = "نماینده مرتبط")]
        public string? SenatorName { get; set; }

        [Display(Name = "حوزه انتخابی نماینده")]
        public string HozeEntekhabi { get; set; }

        [Display(Name = "استان نماینده")]
        public string SenatorCity { get; set; }

        [Display(Name = "معاونت مشترک")]
        public bool Moshtarak { get; set; }

        public MokatebeInputDto MokatebeData { get; set; }

        [ Display(Name = "کاربر جهت ارجاع")]
        public Guid? UserId { get; set; }
        public SelectList UserSelectList { get; set; }

        public CreateNewDataEntryCommand ConvertToCommand()
        {
            return new CreateNewDataEntryCommand()
            {
                DataEntryData = MokatebeData,
                CategoryId = CategoryId.HasValue ? CategoryId : CategoryParentId,
                Moavenats = Moavenats,
                Title = Title,
                Description = Description,
                DataEntryType = DataEntryTypeEnum.Mokatebe,
                SenatorId = SenatorId,
            };
        }

        public UpdateDataEntryCommand ConvertToUpdateCommand()
        {
            return new UpdateDataEntryCommand()
            {
                DataEntryData = MokatebeData,
                CategoryId = CategoryId.HasValue ? CategoryId : CategoryParentId,
                Title = Title,
                Moavenats= Moavenats,
                Description = Description,
                DataEntryType = DataEntryTypeEnum.Mokatebe,
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