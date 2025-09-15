using MajlesMefa.Back.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.Back.Enums.Soval
{
    public enum MohlatZamaniEnum : byte
    {
        [Display(Name = "فوری")]
        Fori = 1,

        [Display(Name = "آنی")]
        Ani = 2,

        [Display(Name = "خیلی فوری")]
        KeyliFori = 3,
    }

    public static class MohlatZamaniEnumHelper
    {
        public static List<IdNameDto> GetList()
        {
            return new List<IdNameDto>()
            {
                IdNameDto.Create(
                    (int)MohlatZamaniEnum.Fori ,
                    MohlatZamaniEnum.Fori.GetPersianName()),
                 IdNameDto.Create(
                    (int)MohlatZamaniEnum.Ani ,
                    MohlatZamaniEnum.Ani.GetPersianName()),
                 IdNameDto.Create(
                    (int)MohlatZamaniEnum.KeyliFori ,
                    MohlatZamaniEnum.KeyliFori.GetPersianName()),
            };
        }
    }

    public static class MohlatZamaniEnumExtension
    {
        public static string GetPersianName(this MohlatZamaniEnum entityType)
        {
            switch (entityType)
            {
                case MohlatZamaniEnum.Fori: return "فوری";
                case MohlatZamaniEnum.Ani: return "آنی";
                case MohlatZamaniEnum.KeyliFori: return "خیلی فوری";
                default: throw new Exception("تعریف نشده");
            }
        }
    }
}
