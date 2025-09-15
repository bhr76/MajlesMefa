using MajlesMefa.Back.Dtos.Common.Grid;
using MajlesMefa.Back.Dtos.DataEntryTypesDtos;
using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Enums.Senator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Dtos
{
    public class MokatebeDto
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

        public string ShomareDabirkhaneMarkazi { get; set; }

        public string ShomareDabirkhane { get; set; }

        public MokatebeRsltDto MyData { get; set; }

        public string CreatorUserName { get; set; }
        public string CommissionTitle { get; set; }

        public SenatorDto Senator { get; set; }

        public string SenatorName { get; set; }

        public string SenatorHozeEntekhabi { get; set; }
        public HozeEntekhabiEnum SenatorHozeEntekhabiEnum { get; set; }

        public string SenatorCity { get; set; }
    }

}
