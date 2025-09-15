using MajlesMefa.Back.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.Back.Enums
{
    public enum LoanTypeEnum: byte
    {
        [Display(Name = "قرض الحسنه")]
        Gharzolhasane = 1,
        
        [Display(Name = "مرابحه")]
        Morabehe = 2,

    }

    public static class LoanTypeEnumHelper
    {
        public static List<IdNameDto> GetList()
        {
            return new List<IdNameDto>()
            {
                IdNameDto.Create(
                    (int)LoanTypeEnum.Gharzolhasane ,
                    LoanTypeEnum.Gharzolhasane.GetPersianName()),
                 IdNameDto.Create(
                    (int)LoanTypeEnum.Morabehe ,
                    LoanTypeEnum.Morabehe.GetPersianName())
            };
        }
    }

    public static class LoanTypeEnumExtension
    {
        public static string GetPersianName(this LoanTypeEnum entityType)
        {
            switch (entityType)
            {
                case LoanTypeEnum.Gharzolhasane: return "قرض الحسنه";
                case LoanTypeEnum.Morabehe: return "مرابحه";
                default: return "تعریف نشده";
            }
        }
    }
}
