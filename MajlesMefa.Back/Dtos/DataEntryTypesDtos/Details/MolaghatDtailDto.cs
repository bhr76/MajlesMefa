using AutoMapper;
using DataAnnotationsExtensions;
using Duende.IdentityServer.Models;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Enums.Molaghat;
using MajlesMefa.Back.Enums.Soval;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.EnumHelper;
using MajlesMefa.Back.Utilities.Mapping;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details
{
    public class MolaghatDtailDto : IMapping, IModifyDataEntryDto
    {
        public Guid Id { get; set; }

        [Display(Name = "نتیجه پاسخ")]
        public ResponseStatusEnum PasokhState { get; set; }

        [Display(Name = "نتیجه پاسخ")]
        public int PasokhStateInt { get { return (int)PasokhState; } set { PasokhState = (ResponseStatusEnum)value; } }

        [Display(Name = "شماره پاسخ")]
        public string PasokhNo { get; set; }

        [Display(Name = "تاریخ پاسخ")]
        public DateTime PasokhDate { get; set; }

        [Display(Name = "تاریخ پاسخ")]
        public string PasokhPersianDate => this.PasokhDate.ToPersianDate();

        [Display(Name = "تعداد ملاقات")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public short Count { get; set; }

        [Display(Name = "محل ملاقات")]
        public MolaghatLocationEnum Mahal { get; set; }

        [Display(Name = "محل ملاقات")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public int MahalInt { get { return (int)Mahal; } set { Mahal = (MolaghatLocationEnum)value; } }

        [Display(Name = "تاریخ")]
        public DateTime Date { get; set; }
        [Display(Name = "تاریخ")]
        public string JalaliTarikh => this.Date == DateTime.MinValue ? null : this.Date.ToPersianDate();

        [Display(Name = "کمیسیون")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string Commision { get; set; }

        [Display(Name = " کمیسیون")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string CommisionTitle { get; set; }

        [Display(Name = "نوع ملاقات")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public MolaghatTypeEnum MolaghatType { get; set; }

        [Display(Name = "نوع ملاقات")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public int MolaghatTypeInt { get { return (int)MolaghatType; } set { MolaghatType = (MolaghatTypeEnum)value; } }

        [Display(Name = "ملاقات با وزیر؟")]
        public bool IsMolaghatBaVazir { get; set; } = false;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<MolaghatDtailDto, MolaghatEntity>();
        }
    }
}
