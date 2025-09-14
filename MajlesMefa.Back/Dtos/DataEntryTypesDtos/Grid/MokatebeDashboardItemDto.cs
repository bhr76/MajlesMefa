using MajlesMefa.Back.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Dtos.DataEntryTypesDtos.Grid
{
    public class MokatebeDashboardItemDto
    {
        public string ItemName { get; set; }
        public short IsCentralOffice { get; set; }

        public int Count { get; set; }

        public Guid Id { get; set; }

        public DataEntryTypeEnum ItemType { get; set; }
    }
}
