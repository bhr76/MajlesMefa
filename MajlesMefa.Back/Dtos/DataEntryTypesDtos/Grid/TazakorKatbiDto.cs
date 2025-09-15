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
    public class TazakorKatbiDto
    {
        public DateTime GheraatSahnDate { get; set; }
        public DateTime PasokhDate { get; set; }
        public ResponseStatusEnum PasokhState { get; set; }
        public EnumDto<ResponseStatusEnum> PasokhStateDesc => new EnumDto<ResponseStatusEnum>
        {
            Value = this.PasokhState,
            Name = this.PasokhState.GetPersianName()
        };
        public TazakorEnum TazakorEnum { get; set; } = TazakorEnum.Katbi;
        public string TazakorEnumDesc { get => this.TazakorEnum.GetDisplayName(); }
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
    }
}
