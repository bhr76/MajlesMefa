using MajlesMefa.Back.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.Back.Enums.Senator
{
    public enum SenatorRequestLoanTypeEnum : byte
    {
        [Display(Name = "قرض الحسنه")]
        Gharzolhasane = 1,

        [Display(Name = "مرابحه")]
        Morabehe = 2,

        [Display(Name = "جعاله")]
        Jeale = 3,
    }

    public static class SenatorRequestLoanTypeEnumHelper
    {
        public static List<IdNameDto> GetList()
        {
            return new List<IdNameDto>()
            {
                IdNameDto.Create(
                    (int)SenatorRequestLoanTypeEnum.Gharzolhasane,
                    SenatorRequestLoanTypeEnum.Gharzolhasane.GetPersianName()),
                IdNameDto.Create(
                    (int)SenatorRequestLoanTypeEnum.Morabehe,
                    SenatorRequestLoanTypeEnum.Morabehe.GetPersianName()),
                IdNameDto.Create(
                    (int)SenatorRequestLoanTypeEnum.Jeale,
                    SenatorRequestLoanTypeEnum.Jeale.GetPersianName())
            };
        }
    }

    public static class SenatorRequestLoanTypeEnumExtension
    {
        public static string GetPersianName(this SenatorRequestLoanTypeEnum loanType)
        {
            switch (loanType)
            {
                case SenatorRequestLoanTypeEnum.Gharzolhasane: return "قرض الحسنه";
                case SenatorRequestLoanTypeEnum.Morabehe: return "مرابحه";
                case SenatorRequestLoanTypeEnum.Jeale: return "جعاله";
                default: return "تعریف نشده";
            }
        }
    }
}
