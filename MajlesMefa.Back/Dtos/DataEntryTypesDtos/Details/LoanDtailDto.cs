using AutoMapper;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Grid;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Enums.Molaghat;
using MajlesMefa.Back.Enums.Soval;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.Mapping;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.Back.Dtos.DataEntryTypesDtos.Details
{
    public class LoanDtailDto : IMapping, IModifyDataEntryDto
    {
        public Guid Id { get; set; }

        [Display(Name = "وضعیت درخواست")]
        public ResponseStatusEnum PasokhState { get; set; }
        public string PasokhStateDesc => PasokhState.GetPersianName();

        [Display(Name = "وضعیت درخواست")]
        public int PasokhStateInt { get { return (int)PasokhState; } set { PasokhState = (ResponseStatusEnum)value; } }

        [Display(Name = "شماره پاسخ")]
        public string PasokhNo { get; set; }

        [Display(Name = "تاریخ پاسخ")]
        public DateTime PasokhDate { get; set; }

        [Display(Name = "تاریخ پاسخ")]
        public string PasokhPersianDate => this.PasokhDate.ToPersianDate();


        [Display(Name = "تاریخ ارجا به شعبه")]
        public string ActionRefrenceDate { get; set; }

        [Display(Name = "نام و نام خانوادگی متقاضی تسهیلات")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string FullName { get; set; }

        [Display(Name = "کدملی متقاضی")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [MaxLength(10, ErrorMessage = "کدملی را 10 رقمی وارد نمایید")]
        public string NationalNo { get; set; }

        [Display(Name = "شماره موبایل متقاضی")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [MaxLength(11, ErrorMessage = "شماره موبایل را 11 رقمی وارد نمایید")]
        public string MobileNo { get; set; }

        [Display(Name = "مبلغ تسهیلات درخواستی(تومان)")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [RegularExpression(@"^[0-9]{1,3}(,[0-9]{3})*$|^[0-9]+$", ErrorMessage = "فرمت مبلغ صحیح نیست. لطفاً عدد یا عدد با جداکننده کامای هزارگان وارد کنید")]
        [MaxLength(15, ErrorMessage = "مبلغ بیش از حد مجاز است")]
        public string Amount { get; set; }
        //public string AmountStr => Amount.ToString().ShowCurrencyFormat();

        [Display(Name = "نوع تسهیلات")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public LoanTypeEnum LoanType { get; set; }
        public string LoanTypeDesc => LoanType.GetPersianName();

        [Display(Name = "نوع تسهیلات")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public int LoanTypeInt { get { return (int)LoanType; } set { LoanType = (LoanTypeEnum)value; } }

        //[Display(Name = "تاریخ درخواست")]
        //public string RequestDate { get; set; }

        [Display(Name = "بانک پیشنهادی")]
        public Guid? SuggestedBankId { get; set; }
        public string SuggestedBankName { get; set; }

        [Display(Name = "بانک پیشنهادی شورا")]
        public Guid? RelatedBankId { get; set; }
        public Guid? SenatorBudgetId { get; set; }
        public Guid? UserId { get; set; }

        [Display(Name = "شناسه یکتا")]
        public int? TrackingCode { get; set; }

        public GetAccessActionRefrenceResultDto AccessActionRefrence { get; set; }



        [Display(Name = "استان")]
        public Guid? ProvinceId { get; set; }

        [Display(Name = "شهر")]
        public Guid? CityId { get; set; }

        [Display(Name = "آدرس کامل")]
        [MaxLength(500, ErrorMessage = "آدرس نباید بیشتر از 500 کاراکتر باشد")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string Address { get; set; }



        public void Mapping(Profile profile)
        {
            profile.CreateMap<LoanDtailDto, LoanEntity>()
                 .ForMember(x => x.DataEntryId, s => s.MapFrom(y => y.Id));
        }
    }
}
