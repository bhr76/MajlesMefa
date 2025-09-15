using AutoMapper;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Enums.Layehe;
using MajlesMefa.Back.Enums.Tarh;
using MajlesMefa.Back.UseCases.Commmands.CreateNewDataEntryCommand;
using MajlesMefa.Back.UseCases.Commmands.UpdateDataEntryCommand;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.Mapping;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.UI.Models
{
    public class TarhViewModel : IMapping, IModifyDataEntryDto
    {
        public Guid Id { get; set; }

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
        public string CategoryParentName { get; set; }


        [Display(Name = "شناسه طرح")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(256, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string Shenase { get; set; }
        
        [Display(Name = "نتیجه بررسی کمیسیون")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public NatijeBarresiEnum NatijeBarresi { get; set; }
        
        [Display(Name = "نتیجه بررسی کمیسیون")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public int NatijeBarresiInt { get { return (int)NatijeBarresi; } set { NatijeBarresi = (NatijeBarresiEnum)value; } }
        
        [Display(Name = "وضعیت بررسی")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public VazeyatBarresiEnum VazeyatBarresi { get; set; }
        
        [Display(Name = "وضعیت بررسی")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public int VazeyatBarresiInt { get { return (int)VazeyatBarresi; } set { VazeyatBarresi = (VazeyatBarresiEnum)value; } }
        
        [Display(Name = "کلیات")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public NatijeBarresiEnum Kollyat { get; set; }
        
        [Display(Name = "کلیات")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public int KollyatInt { get { return (int)Kollyat; } set { Kollyat = (NatijeBarresiEnum)value; } }
        
        [Display(Name = "مواد طرح")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public NatijeBarresiEnum MavadTarh { get; set; }
        
        [Display(Name = "مواد طرح")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public int MavadTarhInt { get { return (int)MavadTarh; } set { MavadTarh = (NatijeBarresiEnum)value; } }
        
        [Display(Name = "نتیجه بررسی شورا")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public NatijeBarresiEnum NatijeBarresiShoraNegahban { get; set; }
        
        [Display(Name = "نتیجه بررسی شورا")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public int NatijeBarresiShoraNegahbanInt { get { return (int)NatijeBarresiShoraNegahban; } set { NatijeBarresiShoraNegahban = (NatijeBarresiEnum)value; } }
        
        [Display(Name = "توضیحات نتیجه بررسی شورا")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string NatijeBarresiShoraNegahbanDescription { get; set; }
        
        [Display(Name = "سوابق ابلاغ")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string SavabeghEblagh { get; set; }
        
        [Display(Name = "تهیه گزارش نهایی")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string GhozarshNahayi { get; set; }
        
        [Display(Name = "حواشی مجلس")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string Havashi { get; set; }
        
        [Display(Name = "ارسال برای وزیر")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string ErsalBeVazir { get; set; }
        
        [Display(Name = "هماهنگی برای شرکت")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string HamahangiBaraSherkat { get; set; }
        
        [Display(Name = "نمایندگان وزارتخانه برای شرکت")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string NamayandeghanBaraSherkat { get; set; }
        
        [Display(Name = "نظر نماینده")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string NazarNamayande { get; set; }
        
        [Display(Name = "گزارش  مذاکرات")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string GhozareshMozakerat { get; set; }
        
        [Display(Name = "کمیسیون مربوطه")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [NotEmptyGuid]
        public Guid RelatedComission { get; set; }

        [Display(Name = "کمیسیون مربوطه")]
        public string RelatedComissionTitle { get; set; }
        
        [Display(Name = "گردش کار")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string Gardeshkar { get; set; }
        
        [Display(Name = "نمایندگان امضا کننده")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(1024, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string NamayandeghanEmzaKonande { get; set; }
        
        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
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

        public static CreateNewDataEntryCommand ConvertToCommand(TarhViewModel tarh)
        {
            return new CreateNewDataEntryCommand()
            {
                Title = tarh.Title,
                DataEntryData = tarh,
                CategoryId = tarh.CategoryId.HasValue ? tarh.CategoryId : tarh.CategoryParentId,
                Description = tarh.Description,
                DataEntryType = DataEntryTypeEnum.Tarh,
                Moavenats = tarh.Moavenats,
                SenatorId = tarh.SenatorId,
            };
        }

        public static UpdateDataEntryCommand ConvertToUpdateCommand(TarhViewModel tarh)
        {
            return new UpdateDataEntryCommand()
            {
                DataEntryData = tarh,
                SenatorId= tarh.SenatorId,
                CategoryId = tarh.CategoryId.HasValue ? tarh.CategoryId : tarh.CategoryParentId,
                Title = tarh.Title,
                Description = tarh.Description,
                Moavenats = tarh.Moavenats,
                DataEntryType = DataEntryTypeEnum.Tarh,
            };
        }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TarhViewModel, TarhEntity>()
                .ForMember(x => x.DataEntryId, s => s.MapFrom(y => y.Id));
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
