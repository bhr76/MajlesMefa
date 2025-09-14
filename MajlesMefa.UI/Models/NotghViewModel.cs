using AutoMapper;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.UseCases.Commmands.CreateNewDataEntryCommand;
using MajlesMefa.Back.UseCases.Commmands.UpdateDataEntryCommand;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.Mapping;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.UI.Models
{
    public class NotghViewModel : IMapping, IModifyDataEntryDto
    {
        public Guid Id { get; set; }

        [Display(Name = "موضوع")]
        public Guid? CategoryId { get; set; }

        [Display(Name = "معاونت مشترک")]
        public bool Moshtarak { get; set; }

        [Display(Name = "معاونت‌ها")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public List<Guid> Moavenats { get; set; }

        [Display(Name = "شماره پاسخ")]
        public string PasokhNo { get; set; }

        [Display(Name = "نتیجه پاسخ")]
        public ResponseStatusEnum PasokhState { get; set; }

        [Display(Name = "نتیجه پاسخ")]
        public int PasokhStateInt { get { return (int)PasokhState; } set { PasokhState = (ResponseStatusEnum)value; } }

        [Display(Name = "تاریخ پاسخ")]
        public DateTime PasokhDate { get; set; }

        [Display(Name = "تاریخ پاسخ")]
        public string PasokhPersianDate { get; set; }

        public string? CategoryName { get; set; }

        [Display(Name = "معاونت")]
        public Guid? CategoryParentId { get; set; }

        [Display(Name = "معاونت")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string CategoryParentName { get; set; }

        [Display(Name = "تاریخ جلسه علنی")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public DateTime JalaseAlaniDate { get; set; }
        
        [Display(Name = "تاریخ جلسه علنی")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string JalaseAlaniPersianDate { get; set; }
        
        [Display(Name = "شماره پیگیری")]
        public string AnswerFromProUnitNo { get; set; }
        
        [Display(Name = "تاریخ پیگیری")]
        public string AnswerFromProUnitPersianDate { get; set; }
        public DateTime AnswerFromProUnitDate { get; set; }
        
        [Display(Name = "چکیده نطق نماینده")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string Chekide { get; set; }
        
        [Display(Name = "گردش ارجاعات")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string GardeshErjaat { get; set; }
        
        [Display(Name = "توضیحات")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string Description { get; set; }
        
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string Title { get; set; }

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

        public static CreateNewDataEntryCommand ConvertToCommand(NotghViewModel notgh)
        {
            notgh.JalaseAlaniDate = notgh.JalaseAlaniPersianDate.ToMiladiDate();
            notgh.AnswerFromProUnitDate = notgh.JalaseAlaniPersianDate.ToMiladiDate();
            notgh.PasokhDate = notgh.PasokhPersianDate.ToMiladiDate();
            return new CreateNewDataEntryCommand()
            {
                Title = notgh.Title,
                DataEntryData = notgh,
                CategoryId = notgh.CategoryId.HasValue ? notgh.CategoryId : notgh.CategoryParentId,
                Description = notgh.Description,
                DataEntryType = DataEntryTypeEnum.Notgh,
                SenatorId = notgh.SenatorId,
                Moavenats = notgh.Moavenats,
            };
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<NotghViewModel, NotghEntity>()
                .ForMember(x => x.DataEntryId, s => s.MapFrom(y => y.Id));

        }

        public static UpdateDataEntryCommand ConvertToUpdateCommand(NotghViewModel notgh)
        {
            notgh.JalaseAlaniDate = notgh.JalaseAlaniPersianDate.ToMiladiDate();
            notgh.AnswerFromProUnitDate = notgh.JalaseAlaniPersianDate.ToMiladiDate();
            notgh.PasokhDate = notgh.PasokhPersianDate.ToMiladiDate();
            return new UpdateDataEntryCommand()
            {
                DataEntryData = notgh,
                Title = notgh.Title,
                CategoryId = notgh.CategoryId.HasValue ? notgh.CategoryId : notgh.CategoryParentId,
                Description = notgh.Description,
                DataEntryType = DataEntryTypeEnum.Notgh,
                SenatorId = notgh.SenatorId,
                Moavenats = notgh.Moavenats,
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
