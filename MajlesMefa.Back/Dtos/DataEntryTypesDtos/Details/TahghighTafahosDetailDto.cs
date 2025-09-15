using AutoMapper;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using MajlesMefa.Back.Enums.TahghighTafahos;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.Mapping;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details
{
    public class TahghighTafahosDetailDto : IMapping, IModifyDataEntryDto
    {
        [Display(Name = "شناسه")]
        public Guid Id { get; set; }

        [Display(Name = "تاریخ")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public DateTime Date { get; set; }

        [Display(Name = "تاریخ")]
        public string JalaliTarikh => this.Date.ToPersianDate();

        [Display(Name = "شماره نامه")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(128, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string ShomareName { get; set; }

        [Display(Name = "شماره دریافت")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(128, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string ShomareDaryaft { get; set; }

        [Display(Name = "کمیسیون مربوطه")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public Guid Commission { get; set; }

        [Display(Name = "کمیسیون مربوطه")]
        public string CommissionTitle { get; set; }

        [Display(Name = "مخاطب")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(128, ErrorMessage = "{0} معتبر نیست", MinimumLength = 3)]
        public string Mokhatab { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public TahghighTafahosVazyatEnum TahghighTafahosVazyat { get; set; }

        [Display(Name = "وضعیت")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public int TahghighTafahosVazyatInt { get { return (int)TahghighTafahosVazyat; } set { TahghighTafahosVazyat = (TahghighTafahosVazyatEnum)value; } }

        [Display(Name = "نمایندگان متقاضی")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public List<SenatorProfileEntity> TahghighTafahosSenators { get; set; }

        [Display(Name = "نمایندگان متقاضی")]
        public List<Guid> TahghighTafahosSenatorsId { get; set; }
        public string TahghighTafahosSenatorsName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<TahghighTafahosDetailDto, TahghighTafahosEntity>()
                .ReverseMap();
        }
    }
}
