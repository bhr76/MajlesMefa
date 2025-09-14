using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Enums.Molaghat;
using MajlesMefa.Back.Enums.Soval;
using MajlesMefa.Back.Utilities.Date;
using MajlesMefa.Back.Utilities.EnumHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Dtos.DataEntryTypesDtos.Grid
{
    public class MolaghatDto
    {
        public short Count { get; set; }

        public MolaghatLocationEnum Mahal { get; set; }

        public EnumDto<MolaghatLocationEnum> MahalDesc => new EnumDto<MolaghatLocationEnum>
        {
            Value = this.Mahal,
            Name = this.Mahal.GetPersianName()
        };

        public string PasokhNo { get; set; }

        public DateTime Tarikh { get; set; }
        public string TarikhStr => Tarikh != DateTime.MinValue ?  Tarikh.ToPersianDate() : "";

        public MolaghatTypeEnum MolaghatType { get; set; }
        public EnumDto<MolaghatTypeEnum> MolaghatTypeDesc => new EnumDto<MolaghatTypeEnum>
        {
            Value = this.MolaghatType,
            Name = this.MolaghatType.GetPersianName()
        };

        public ResponseStatusEnum PasokhState { get; set; }
        public EnumDto<ResponseStatusEnum> PasokhStateDesc => new EnumDto<ResponseStatusEnum>
        {
            Value = this.PasokhState,
            Name = this.PasokhState.GetPersianName()
        };

        public bool IsMolaghatBaVazir { get; set; }

    }
}
