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
    public class SovalDto
    {
        public Guid Commission { get; set; }

        public DateTime KarbargDate { get; set; }
        public string KarbargDateStr => this.KarbargDate.ToPersianDate();

        public string DabirkhaneMakaziNo { get; set; }

        public string Organization { get;set; }
        public GetAccessActionRefrenceResultDto AccessActionRefrence { get; set; }
        
        public QuestionStatusEnum QuestionStatus { get; set; }
        public EnumDto<QuestionStatusEnum> QuestionStatusDesc => new EnumDto<QuestionStatusEnum>
        {
            Value = this.QuestionStatus,
            Name = this.QuestionStatus.GetPersianName()
        };
    }
}
