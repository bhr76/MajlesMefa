using AutoMapper;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Enums.Soval;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.EnumHelper;
using MajlesMefa.Back.Utilities.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details
{
    public class SovalDetailDto : IMapping, IModifyDataEntryDto
    {
        public Guid Id { get; set; }

        [Display(Name = "کمیسیون مربوطه")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [NotEmptyGuid]
        public Guid Commission { get; set; }

        [Display(Name = "کمیسیون مربوطه")]
        public string CommissionTitle { get; set; }
        
        [Display(Name = "تاریخ کاربرگ سوال")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public DateTime KarbargDate { get; set; }

        [Display(Name = "تاریخ کاربرگ سوال")]
        public String KarbargDateStr => this.KarbargDate.ToPersianDate();

        [Display(Name = "شماره دبیرخانه مرکزی")]
        public string DabirkhaneMakaziNo { get; set; }

        [Display(Name = "وضعیت سوال")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [Range(1, int.MaxValue)]
        [EnumDataType(typeof(QuestionStatusEnum))]
        public QuestionStatusEnum QuestionStatus { get; set; }

        [Display(Name = "وضعیت سوال")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public int QuestionStatusInt { get { return (int)QuestionStatus; } set { QuestionStatus = (QuestionStatusEnum)value; } }

        [Display(Name = "جلسات داخلی برای بررسی سوال")]
        public string JalasatDakheli { get; set; }

        [Display(Name = "بررسی سوال در کمیسیون های تخصصی مجلس")]
        public string BarresiDarCommission { get; set; }

        [Display(Name = "بررسی سوال در صحن")]
        public string BarresiDarSahn { get; set; }

        public List<PeygiryDto> Peygiries { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<SovalDetailDto, SovalEntity>()
                .ReverseMap();
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
