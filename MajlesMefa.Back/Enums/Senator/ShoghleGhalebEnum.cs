using MajlesMefa.Back.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.Back.Enums.Senator
{
    public enum ShoghleGhalebEnum : byte
    {
        [Display(Name = "کشاورزی")]
        Keshavarzi = 1,

        [Display(Name = "صنعتی")]
        Sanati = 2,

        [Display(Name = "مرزنشینی")]
        Marzneshini = 3,

        [Display(Name = "صیادی")]
        Sayyadi = 4,

    }

    public static class ShoghleGhalebEnumHelper
    {
        public static List<IdNameDto> GetList()
        {
            return new List<IdNameDto>()
            {
                IdNameDto.Create(
                    (int)ShoghleGhalebEnum.Keshavarzi ,
                    ShoghleGhalebEnum.Keshavarzi.GetPersianName()),
                 IdNameDto.Create(
                    (int)ShoghleGhalebEnum.Sanati ,
                    ShoghleGhalebEnum.Sanati.GetPersianName()),
                 IdNameDto.Create(
                    (int)ShoghleGhalebEnum.Marzneshini ,
                    ShoghleGhalebEnum.Marzneshini.GetPersianName()),
                 IdNameDto.Create(
                    (int)ShoghleGhalebEnum.Sayyadi ,
                    ShoghleGhalebEnum.Sayyadi.GetPersianName()),
            };
        }
    }

    public static class ShoghleGhalebEnumExtension
    {
        public static string GetPersianName(this ShoghleGhalebEnum entityType)
        {
            switch (entityType)
            {
                case ShoghleGhalebEnum.Keshavarzi: return "کشاورزی";
                case ShoghleGhalebEnum.Sanati: return "صنعتی";
                case ShoghleGhalebEnum.Marzneshini: return "مرزنشینی";
                case ShoghleGhalebEnum.Sayyadi: return "صیادی";
                default: return "تعریف نشده";
            }
        }
    }



}
