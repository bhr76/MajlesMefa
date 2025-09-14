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
    public class NotghDto
    {
        public DateTime JalaseAlaniDate { get; set; }
        public string JalaseAlaniPersianDate { get => this.JalaseAlaniDate.ToPersianDate(); }
        public string AnswerFromProUnitNo { get; set; }

        public DateTime AnswerFromProUnitDate { get; set; }
        public string AnswerFromProUnitPersianDate { get => this.AnswerFromProUnitDate == DateTime.MinValue ? "--" : this.AnswerFromProUnitDate.ToPersianDate(); }
        public string Chekide { get; set; }
        public string PasokhNo { get; set; }
        public DateTime PasokhDate { get; set; }
        public ResponseStatusEnum PasokhState { get; set; }
        public EnumDto<ResponseStatusEnum> PasokhStateDesc => new EnumDto<ResponseStatusEnum>
        {
            Value = this.PasokhState,
            Name = this.PasokhState.GetPersianName()
        };
        public string PasokhPersianDate { get => this.PasokhDate == DateTime.MinValue ? "--" : this.PasokhDate.ToPersianDate(); }

        public string GardeshErjaat { get; set; }

        public GetAccessActionRefrenceResultDto AccessActionRefrence { get; set; }
    }
}
