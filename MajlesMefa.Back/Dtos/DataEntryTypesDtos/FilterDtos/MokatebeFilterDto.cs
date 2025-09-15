using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Utilities.Db.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Dtos.DataEntryTypesDtos.FilterDtos
{
    public class MokatebeFilterDto: PagerDto, IBaseDataEntryFilterDto
    {
        public DateTime? DabirkhaneMarkaziFromDate { get; set; }
        public DateTime? DabirkhaneMarkaziToDate { get; set; }

        public string MokatebeKonande { get; set; }

        public MokatebeTypeEnum? MokatebeType { get; set; }

        public string ShomareDabirkhane { get; set; }

        public string ShomareDabirkhaneMarkazi { get; set; }

        public DateTime? DabirKhaneFromDate { get; set; }
        
        public DateTime? DabirKhaneToDate { get; set; }
    }
}
