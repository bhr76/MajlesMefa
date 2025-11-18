using MajlesMefa.Back.Dtos.Common.Grid;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Enums.Senator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Dtos
{
    public class DataEntryDto<TData>
    {
        public Guid Id { get;set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DataEntryTypeEnum DataEntryType { get; set; }

        public List<string> Moavenats { get; set; }

        public Guid? CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string CategoryParentName { get; set; }
        
        public Guid? CategoryParentId { get; set; }

        public TData MyData { get; set; }

        public string CreatorUserName { get; set; }
        public string PersianCreatedDate { get; set; }
        public string CommissionTitle { get; set; }

        public SenatorDto Senator { get; set; }

        public string SenatorName { get; set; }

        public string SenatorHozeEntekhabi { get; set; }
        public HozeEntekhabiEnum SenatorHozeEntekhabiEnum { get; set; }

        public string SenatorCity { get; set; }
        public string TrackingCode { get; set; }
        public string LoanOwnerFullName { get; set; }
        public string LoanOwnerNationalCode { get; set; }
        public string LoanOwnerMobile { get; set; }
    }

    public class DataEntryDto: DataEntryDto<object>
    {

    }
}
