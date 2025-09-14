using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.EnumHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Dtos.DataEntryTypesDtos.Grid
{
    public class TazakorDto
    {
        public DateTime GheraatSahnDate { get; set; }
        public DateTime PasokhDate { get; set; }
        public TazakorEnum TazakorType { get; set; }
        public EnumDto<TazakorEnum> TazakorTypeDesc => new EnumDto<TazakorEnum>
        {
            Value = this.TazakorType,
            Name = this.TazakorType.GetPersianName()
        };

        public ResponseStatusEnum PasokhState { get; set; }
        public EnumDto<ResponseStatusEnum> PasokhStateDesc => new EnumDto<ResponseStatusEnum>
        {
            Value = this.PasokhState,
            Name = this.PasokhState.GetPersianName()
        };
        
        public string GheraatSahnPersianDate { get => this.GheraatSahnDate.ToPersianDate(); }
        public string ShomareName { get; set; }
        public string PasokhNo { get; set; }
        public string PasokhNoDesc => PasokhNo != null ? $"  پاسخ داده شده با شماره  {PasokhNo} " : "در دست پیگیری";

        public string PasokhPersianDate { get => this.PasokhDate.ToPersianDate(); }

        public DateTime PishnevisDate { get; set; }
        public string PishnevisPersianDate { get => this.PishnevisDate.ToPersianDate(); }
        public string GardeshErjaat { get; set; }

        public string GardeshErjaatMoavenat { get; set; }
        public GetAccessActionRefrenceResultDto AccessActionRefrence { get; set; }


        public DateTime NameVaseleDate { get; set; }
        public string NameVaselePersianDate { get => this.NameVaseleDate.ToPersianDate(); }
        public string NameVaseleNo { get; set; }
        public string NameVaseleDabirkhaneNo { get; set; }
        public VaseleAzEnum VaseleAz { get; set; }
        public int VaseleAzInt { get { return (int)VaseleAz; } set { VaseleAz = (VaseleAzEnum)value; } }
    }
}
