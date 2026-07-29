using MajlesMefa.Back.Enums;
using MajlesMefa.Back.Enums.Senator;
using MajlesMefa.Back.Enums.Molaghat;

namespace MajlesMefa.Back.Dtos
{
    public class FlatDataEntryDto
    {
        // فیلدهای اصلی DataEntry
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DataEntryTypeEnum DataEntryType { get; set; }
        public List<string> Moavenats { get; set; }
        public Guid? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryParentName { get; set; }
        public Guid? CategoryParentId { get; set; }
        public string CreatorUserName { get; set; }
        public string PersianCreatedDate { get; set; }
        public string CommissionTitle { get; set; }

        // فیلدهای Senator (قبلا در Senator object بود)
        public string SenatorName { get; set; }
        public string SenatorHozeEntekhabi { get; set; }
        public HozeEntekhabiEnum? SenatorHozeEntekhabiEnum { get; set; }
        public string SenatorCity { get; set; }
        public Guid? SenatorUserId { get; set; }

        // فیلدهای Loan Owner
        public string TrackingCode { get; set; }
        public string LoanOwnerFullName { get; set; }
        public string LoanOwnerNationalCode { get; set; }
        public string LoanOwnerMobile { get; set; }

        // فیلدهای MyData (قبلا در LoanDtailDto بود)
        public string FullName { get; set; }
        public string NationalNo { get; set; }
        public string MobileNo { get; set; }
        public string Amount { get; set; }
        public LoanTypeEnum LoanType { get; set; }
        public string LoanTypeDesc => LoanType.GetPersianName();
        public string PasokhNo { get; set; }
        public ResponseStatusEnum PasokhState { get; set; }
        public string PasokhStateDesc => PasokhState.GetPersianName();
        public int? TrackingCodeInt { get; set; }
        public string ActionRefrenceDate { get; set; }
        public string SuggestedBankName { get; set; }
        public Guid? SuggestedBankId { get; set; }
        public Guid? RelatedBankId { get; set; }
        public Guid? SenatorBudgetId { get; set; }

        // فیلدهای AccessActionRefrence
        public bool CanAccessActionRefrence { get; set; }
        public string AccessActionRefrenceMessage { get; set; }
        public int Year { get; set; }
    }
}