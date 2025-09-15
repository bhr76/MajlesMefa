using AutoMapper;
using MajlesMefa.Back.Entities;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Utilities.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.EnumHelper;
using MajlesMefa.Back.Utilities.Convertor;

namespace MajlesMefa.Back.Dtos.DataEntryTypesDtos
{
    public class MokatebeInputDto : IMapping, IModifyDataEntryDto
    {
        public Guid Id { get; set; }

        [Display(Name = "پیگیری کننده")]
        public string PeygiriKonande { get; set; }

        [Display(Name = "شماره پیگیری")]
        public string PeygiriNumber { get; set; }

        [Display(Name = "توضیحات پیگیری")]
        public string PeygiriDescription { get; set; }

        [Display(Name = "تاریخ پیگیری")]
        public DateTime? PeygiriDate { get; set; }

        public string PeygiriDateShamsi { get; set; }

        [Display(Name = "تاریخ پیگیری")]
        public String PeygiriDateStr => this.PeygiriDate?.ToPersianDate();

        [Display(Name = "ارسالی از")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public MokatebeKonandeEnum MokatebeKonande { get; set; }

        [Display(Name = "ارسالی از")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public int MokatebeKonandeInt { get { return (int)MokatebeKonande; } set { MokatebeKonande = (MokatebeKonandeEnum)value; } }

        [Display(Name = "نوع مکاتبه")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public MokatebeTypeEnum MokatebeType { get; set; }

        [Display(Name = "نوع مکاتبه")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public int MokatebeTypeInt { get { return (int)MokatebeType; } set { MokatebeType = (MokatebeTypeEnum)value; } }

        [Display(Name = "مخاطب نامه")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public ContactEnum Contact { get; set; }

        [Display(Name = "مخاطب نامه")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public int ContactInt { get { return (int)Contact; } set { Contact = (ContactEnum)value; } }

        [Display(Name = "وضعیت پاسخ")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public ResponseStatusEnum VaziatPasokh { get; set; }

        [Display(Name = "وضعیت پاسخ")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public int VaziatPasokhInt { get { return (int)VaziatPasokh; } set { VaziatPasokh = (ResponseStatusEnum)value; } }

        [Display(Name = "شماره نامه نماینده")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public string ShomareDabirkhane { get; set; }

        [Display(Name = "شماره دبیرخانه مرکزی")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        [StringLength(30, ErrorMessage = "{0} معتبر نیست", MinimumLength = 2)]
        [RegularExpression("^[0-9]*$", ErrorMessage = "{0} معتبر نیست")]
        public string ShomareDabirkhaneMarkazi { get; set; }

        [Display(Name = "تاریخ دبیرخانه")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public DateTime TarikhDabirKhane { get; set; }

        public string TarikhDabirKhaneShamsi { get; set; }

        [Display(Name = "تاریخ دبیرخانه")]
        public string TarikhDabirKhaneStr => this.TarikhDabirKhane.ToPersianDate();

        [Display(Name = "تاریخ نامه نماینده")]
        [Required(ErrorMessage = "پرکردن این فیلد اجباری است")]
        public DateTime TarikhNameNamayande { get; set; }

        public string TarikhNameNamayandeShamsi { get; set; }

        [Display(Name = "تاریخ نامه نماینده")]
        public string TarikhNameNamayandeStr => this.TarikhNameNamayande.ToPersianDate();

        [Display(Name = "شماره پاسخ")]
        public string PasokhNo { get; set; }

        [Display(Name = "تاریخ پاسخ")]
        public DateTime PasokhDate { get; set; }
        public string PasokhDateShamsi { get; set; }

        [Display(Name = "تاریخ پاسخ")]
        public string PasokhPersianDate => this.PasokhDate.ToPersianDate();

        [Display(Name = "مبلغ")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "{0} معتبر نیست")]
        public decimal? Amount { get; set; }
        public string AmountStr => this.Amount.ToString().Replace(".", string.Empty).ShowCurrencyFormat();

        public bool HasPeygiry => PeygiriDate != default || PeygiriKonande != default || PeygiriDescription != default || PeygiriNumber != default;

        public void Mapping(Profile profile)
        {
            profile.CreateMap<MokatebeInputDto, MokatebeEntity>()
                .ForMember(x => x.DataEntryId, s => s.MapFrom(y => y.Id));
            profile.CreateMap<MokatebeInputDto, PeygiriEntity>()
                .ForMember(x => x.Id, s => s.Ignore())
                .ForMember(x => x.Description, s => s.MapFrom(y => y.PeygiriDescription))
                .ForMember(x => x.DataEntryId, s => s.MapFrom(y => y.Id));
        }
    }
}
