using MajlesMefa.Back.Utilities.Date;

namespace MajlesMefa.Back.Dtos.DataEntryTypesDtos.Grid
{
    public class KhadamatDto
    {

        public DateTime TarikhKhedmat { get; set; }
        public string TarikhKhedmatPersianDate { get => TarikhKhedmat.ToPersianDate(); }

        public GetAccessActionRefrenceResultDto AccessActionRefrence { get; set; }
    }
}
