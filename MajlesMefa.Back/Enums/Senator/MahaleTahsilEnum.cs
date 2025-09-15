using MajlesMefa.Back.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.Back.Enums.Senator
{
    public enum MahaleTahsilEnum : byte
    {
        [Display(Name = "داخل-دولتی")]
        DakhelDolati = 1,

        [Display(Name = "داخل-غیردولتی")]
        DakhelGheyreDolati = 2,

        [Display(Name = "خارج")]
        Kharej = 3,

    }

    public static class MahaleTahsilEnumHelper
    {
        public static List<IdNameDto> GetList()
        {
            return new List<IdNameDto>()
            {
                IdNameDto.Create(
                    (int)MahaleTahsilEnum.DakhelDolati ,
                    MahaleTahsilEnum.DakhelDolati.GetPersianName()),
                 IdNameDto.Create(
                    (int)MahaleTahsilEnum.DakhelGheyreDolati ,
                    MahaleTahsilEnum.DakhelGheyreDolati.GetPersianName()),
                 IdNameDto.Create(
                    (int)MahaleTahsilEnum.Kharej ,
                    MahaleTahsilEnum.Kharej.GetPersianName()),
            };
        }
    }

    public static class MahaleTahsilEnumExtension
    {
        public static string GetPersianName(this MahaleTahsilEnum entityType)
        {
            switch (entityType)
            {
                case MahaleTahsilEnum.DakhelDolati: return "داخل-دولتی";
                case MahaleTahsilEnum.DakhelGheyreDolati: return "داخل-غیردولتی";
                case MahaleTahsilEnum.Kharej: return "خارج";
                default: return "تعریف نشده";
            }
        }
    }



}
