using MajlesMefa.Back.Enums;

namespace MajlesMefa.Back.Dtos.DataEntryTypesDtos.Grid
{
    public class DashboardLoanDto
    {
        public int CountAllLoan { get; set; }
        public int CountShoraRefrences { get; set; }
        public int CountOfApprovedLoan { get; set; }
        public int CountOfDeniedLoan { get; set; }
        public int CountOfInProgressLoan { get; set; }
        public int CountOfInBranchLoan { get; set; }
        public long AmountOfApprovedLoan { get; set; }
        public long AmountOfInprogressLoan { get; set; }
        public List<DashboardLoanItemDto> DashboardChartData { get; set; }
    }
    public class DashboardLoanItemDto
    {
        public string ItemName { get; set; }

        public int Count { get; set; }

        public Guid Id { get; set; }

        public DataEntryTypeEnum ItemType { get; set; }

        public long TotalAmount { get; set; } = 0;
    }
}
