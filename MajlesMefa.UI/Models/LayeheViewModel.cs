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
using System;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.UI.Models
{
    public class LayeheViewModel : IMapping, IModifyDataEntryDto
    {
        public Guid Id { get; set; }
        
        [Display(Name = "شماره ثبت")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string ShomareSabt { get; set; }

        [Display(Name = "وضعیت طرح")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public VazeyatBarresiEnum VazeyatBarresi { get; set; }

        [Display(Name = "وضعیت طرح")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public int VazeyatBarresiInt { get { return (int)VazeyatBarresi; } set { VazeyatBarresi = (VazeyatBarresiEnum)value; } }

        [Display(Name = "نوع")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public LayeheTarhTypeEnum Type { get; set; }

        [Display(Name = "نوع")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public int TypeInt { get { return (int)Type; } set { Type = (LayeheTarhTypeEnum)value; } }

        [Display(Name = "معاونت مشترک")]
        public bool Moshtarak { get; set; }

        [Display(Name = "کمیسیون اصلی")]
        public List<Guid> MajorCommissions { get; set; }

        [Display(Name = "کمیسیون فرعی")]
        public List<Guid> MinorCommissions { get; set; }

        [Display(Name = "معاونت‌ها")]
        public List<Guid> Moavenats { get; set; }

        [Display(Name = "موضوع")]
        public Guid? CategoryId { get; set; }

        public string? CategoryName { get; set; }

        [Display(Name = "معاونت")]
        public Guid? CategoryParentId { get; set; }

        [Display(Name = "معاونت")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string CategoryParentName { get; set; }
        
        [Display(Name = "تاریخ بررسی کلیات در صحن")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public DateTime? BaresiKoliatDarSahnDate { get; set; }

        [Display(Name = "تاریخ بررسی کلیات در صحن")]
        public string BaresiKoliatDarSahnPersianDate { get; set; }

        [Display(Name = "تاریخ اعلام وصول")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public DateTime? ElamVosoolDate { get; set; }
        
        [Display(Name = "تاریخ اعلام وصول")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string ElamVosoolPersianDate { get; set; }

        [Display(Name = "تاریخ ابلاغ")]
        public DateTime? EblaghDate { get; set; }
        
        [Display(Name = "تاریخ ابلاغ")]
        public string EblaghPersianDate { get; set; }

        [Display(Name = "نتیجه بررسی در کمیسیون")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public NatijeBarresiEnum NatijeBarresiCommission { get; set; }

        [Display(Name = "نتیجه بررسی در کمیسیون")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public int NatijeBarresiCommissionInt { get { return (int)NatijeBarresiCommission; } set { NatijeBarresiCommission = (NatijeBarresiEnum)value; } }

        [Display(Name = "نتیجه بررسی در شورای نگهبان")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public NatijeBarresiShoraEnum NatijeBarresiShora { get; set; }

        [Display(Name = "نتیجه بررسی در شورای نگهبان")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public int NatijeBarresiShoraInt { get { return (int)NatijeBarresiShora; } set { NatijeBarresiShora = (NatijeBarresiShoraEnum)value; } }

        [Display(Name = "نتیجه بررسی کلیات در صحن")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public NatijeBarresiEnum NatijeBarresiSahn { get; set; }

        [Display(Name = "نتیجه بررسی کلیات در صحن")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public int NatijeBarresiSahnInt { get { return (int)NatijeBarresiSahn; } set { NatijeBarresiSahn = (NatijeBarresiEnum)value; } }
        
        [Display(Name = "توضیحات")]
        public string Description { get; set; }
        
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string Title { get; set; }


        public static CreateNewDataEntryCommand ConvertToCommand(LayeheViewModel layehe)
        {
            layehe.BaresiKoliatDarSahnDate = layehe.BaresiKoliatDarSahnPersianDate.ToMiladiDate();
            layehe.ElamVosoolDate = layehe.ElamVosoolPersianDate.ToMiladiDate();
            layehe.EblaghDate = layehe.EblaghPersianDate.ToMiladiDate();
            return new CreateNewDataEntryCommand()
            {
                Title = layehe.Title,
                DataEntryData = layehe,
                CategoryId = layehe.CategoryId.HasValue ? layehe.CategoryId : layehe.CategoryParentId,
                Description = layehe.Description,
                DataEntryType = DataEntryTypeEnum.Layehe,
                //Moavenats = layehe.Moavenats,
            };
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<LayeheViewModel, LayeheEntity>()
                .ForMember(x => x.DataEntryId, s => s.MapFrom(y => y.Id));
           
        }

        public static UpdateDataEntryCommand ConvertToUpdateCommand(LayeheViewModel layehe)
        {
            layehe.BaresiKoliatDarSahnDate = layehe.BaresiKoliatDarSahnPersianDate.ToMiladiDate();
            layehe.ElamVosoolDate = layehe.ElamVosoolPersianDate.ToMiladiDate();
            layehe.EblaghDate = layehe.EblaghPersianDate.ToMiladiDate();
            return new UpdateDataEntryCommand()
            {
                DataEntryData = layehe,
                Title = layehe.Title,
                CategoryId = layehe.CategoryId.HasValue ? layehe.CategoryId : layehe.CategoryParentId,
                Description = layehe.Description,
                DataEntryType = DataEntryTypeEnum.Layehe,
                //Moavenats = layehe.Moavenats,
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
