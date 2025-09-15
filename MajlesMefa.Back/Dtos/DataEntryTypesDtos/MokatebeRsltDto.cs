using AutoMapper;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos.Grid;
using MajlesMefa.Back.Entities.DataEntryTypesEntities;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.EnumHelper;
using MajlesMefa.Back.Utilities.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Dtos.DataEntryTypesDtos
{
    public class MokatebeRsltDto
    {
        public MokatebeKonandeEnum MokatebeKonande { get; set; }

        public EnumDto<MokatebeKonandeEnum> MokatebeKonandeDesc => new EnumDto<MokatebeKonandeEnum>
        {
            Value = this.MokatebeKonande,
            Name = this.MokatebeKonande.GetPersianName()
        };

        public MokatebeTypeEnum MokatebeType { get; set; }

        public EnumDto<MokatebeTypeEnum> MokatebeTypeDesc => new EnumDto<MokatebeTypeEnum>
        {
            Value = this.MokatebeType,
            Name = this.MokatebeType.GetPersianName()
        };

        public ContactEnum Contact { get; set; }

        public EnumDto<ContactEnum> ContactDesc => new EnumDto<ContactEnum>
        {
            Value = this.Contact,
            Name = this.Contact.GetPersianName()
        };

        public ResponseStatusEnum VaziatPasokh { get; set; }

        public EnumDto<ResponseStatusEnum> VaziatPasokhDesc => new EnumDto<ResponseStatusEnum>
        {
            Value = this.VaziatPasokh,
            Name = this.VaziatPasokh.GetPersianName()
        };


        public string ShomareDabirkhane { get; set; }
        public string PasokhNo { get; set; }
        public DateTime PasokhDate { get; set; }
        public string PasokhPersianDate { get => this.PasokhDate == DateTime.MinValue ? "--" : this.PasokhDate.ToPersianDate(); }

        public string ShomareDabirkhaneMarkazi { get; set; }

        public DateTime TarikhDabirKhane { get; set; }
        public string TarikhDabirKhaneStr => this.TarikhDabirKhane == DateTime.MinValue ? "--" : TarikhDabirKhane.ToPersianDate();
        
        //تاریخ نامه نماینده
        public DateTime TarikhDabirMarkazi { get; set; }
        public string TarikhDabirKhaneMarkaziStr => this.TarikhDabirMarkazi == DateTime.MinValue ? "--" : TarikhDabirMarkazi.ToPersianDate();

        public string GardeshErjaat { get; set; }

        public List<PeygiryDto> Peygiries { get; set; }

        public decimal? Amount { get; set; }
        public GetAccessActionRefrenceResultDto AccessActionRefrence { get; set; }
    }
}
