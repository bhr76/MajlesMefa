using MajlesMefa.Back.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.Back.Enums.Senator
{
    public enum HozeEntekhabiEnum : byte
    {
        [Display(Name = "محروم")]
        Mahroom = 1,

        [Display(Name = "متوسط")]
        Motevaset = 2,

        [Display(Name = "توسعه یافته")]
        ToseYafte = 3,

    }

    public static class HozeEntekhabiEnumHelper
    {
        public static List<IdNameDto> GetList()
        {
            return new List<IdNameDto>()
            {
                IdNameDto.Create(
                    (int)HozeEntekhabiEnum.Mahroom ,
                    HozeEntekhabiEnum.Mahroom.GetPersianName()),
                 IdNameDto.Create(
                    (int)HozeEntekhabiEnum.Motevaset ,
                    HozeEntekhabiEnum.Motevaset.GetPersianName()),
                 IdNameDto.Create(
                    (int)HozeEntekhabiEnum.ToseYafte ,
                    HozeEntekhabiEnum.ToseYafte.GetPersianName()),
            };
        }
    }

    public static class HozeEntekhabiEnumExtension
    {
        public static string GetPersianName(this HozeEntekhabiEnum entityType)
        {
            switch (entityType)
            {
                case HozeEntekhabiEnum.Mahroom: return "محروم";
                case HozeEntekhabiEnum.Motevaset: return "متوسط";
                case HozeEntekhabiEnum.ToseYafte: return "توسعه یافته";
                default: return "تعریف نشده";
            }
        }
    }



}
