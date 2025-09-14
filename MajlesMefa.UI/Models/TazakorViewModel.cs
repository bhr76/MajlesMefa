using AutoMapper;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Enums.Layehe;
using MajlesMefa.Back.UseCases.Commmands.CreateNewDataEntryCommand;
using MajlesMefa.Back.UseCases.Commmands.UpdateDataEntryCommand;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.Mapping;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.UI.Models
{
    public class TazakorViewModel : IMapping, IModifyDataEntryDto
    {
        public Guid Id { get; set; }

        [Display(Name = "موضوع")]
        public Guid? CategoryId { get; set; }

        public string? CategoryName { get; set; }

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

        [Display(Name = "معاونت")]
        public Guid? CategoryParentId { get; set; }

        [Display(Name = "معاونت")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string CategoryParentName { get; set; }

        [Display(Name = "تاریخ قرائت در صحن")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public DateTime GheraatSahnDate { get; set; }
        
        [Display(Name = "تاریخ قرائت در صحن")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string GheraatSahnPersianDate { get; set; }
        
        [Display(Name = "نوع تذکر")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public TazakorEnum TazakorType { get; set; } 
        
        [Display(Name = "نوع تذکر")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public int TazakorTypeInt { get { return (int)TazakorType; } set { TazakorType = (TazakorEnum)value; } }
        
        [Display(Name = "شماره نامه")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(256, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string ShomareName { get; set; }
        
        //[Display(Name = "تاریخ ارسال پیش نویس")]
        //[Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        //public DateTime PishnevisDate { get; set; }
        
        //[Display(Name = "تاریخ ارسال پیش نویس")]
        //[Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        //public string PishnevisPersianDate { get; set; }
        
        [Display(Name = "توضیحات")]
        public string Description { get; set; }
        
        [Display(Name = "خلاصه تذکر")]
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

        [Display(Name = "شماره نامه واصله")]
        public string NameVaseleNo { get; set; }

        [Display(Name = "تاریخ نامه واصله")]
        public DateTime  NameVaseleDate { get; set; }

        [Display(Name = "تاریخ نامه واصله")]
        public string  NameVaselePersianDate { get; set; }

        [Display(Name = "شماره دبیرخانه مرکزی نامه واصله")]
        public string NameVaseleDabirkhaneNo { get; set; }

        [Display(Name = "واصله از")]
        public VaseleAzEnum VaseleAz { get; set; }
        [Display(Name = "واصله از")]
        public int VaseleAzInt { get { return (int)VaseleAz; } set { VaseleAz = (VaseleAzEnum)value; } }



        public static CreateNewDataEntryCommand ConvertToCommand(TazakorViewModel tazakor)
        {
            tazakor.NameVaseleDate = tazakor.NameVaselePersianDate.ToMiladiDate();
            tazakor.GheraatSahnDate = tazakor.GheraatSahnPersianDate.ToMiladiDate();
            tazakor.PasokhDate = tazakor.PasokhPersianDate.ToMiladiDate();
            return new CreateNewDataEntryCommand()
            {
                Title = tazakor.Title,
                DataEntryData = tazakor,
                CategoryId = tazakor.CategoryId.HasValue ? tazakor.CategoryId : tazakor.CategoryParentId,
                SenatorId = tazakor.SenatorId,
                Description = tazakor.Description,
                Moavenats = tazakor.Moavenats,
                DataEntryType = DataEntryTypeEnum.Tazakor,
            };
        }

        public static UpdateDataEntryCommand ConvertToUpdateCommand(TazakorViewModel tazakor)
        {
            tazakor.NameVaseleDate = tazakor.NameVaselePersianDate.ToMiladiDate();
            tazakor.GheraatSahnDate = tazakor.GheraatSahnPersianDate.ToMiladiDate();
            tazakor.PasokhDate = tazakor.PasokhPersianDate.ToMiladiDate();
            return new UpdateDataEntryCommand()
            {
                Title = tazakor.Title,
                CategoryId = tazakor.CategoryId.HasValue ? tazakor.CategoryId : tazakor.CategoryParentId,
                SenatorId = tazakor.SenatorId,
                DataEntryData = tazakor,
                Description = tazakor.Description,
                Moavenats = tazakor.Moavenats,
                DataEntryType = DataEntryTypeEnum.Tazakor,
            };
        }

        public void Mapping(Profile profile)
        {
            throw new NotImplementedException();
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
