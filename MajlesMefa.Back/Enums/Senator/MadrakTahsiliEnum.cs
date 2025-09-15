using MajlesMefa.Back.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.Back.Enums.Senator
{
    public enum MadrakTahsiliEnum : byte
    {
        [Display(Name = "لیسانس")]
        Lisans = 1,

        [Display(Name = "فوق لیسانس")]
        Foghlisans = 2,

        [Display(Name = "دکترا")]
        PHD = 3,

        [Display(Name = "حوزوی")]
        Hozavi = 4,

        [Display(Name = "معادل")]
        Moadel = 5,

        [Display(Name = "فوق دکترا")]
        POSTPHD = 6,

    }

    public static class MadrakTahsiliEnumHelper
    {
        public static List<IdNameDto> GetList()
        {
            return new List<IdNameDto>()
            {
                IdNameDto.Create(
                    (int)MadrakTahsiliEnum.Lisans ,
                    MadrakTahsiliEnum.Lisans.GetPersianName()),
                 IdNameDto.Create(
                    (int)MadrakTahsiliEnum.Foghlisans ,
                    MadrakTahsiliEnum.Foghlisans.GetPersianName()),
                 IdNameDto.Create(
                    (int)MadrakTahsiliEnum.PHD ,
                    MadrakTahsiliEnum.PHD.GetPersianName()),
                 IdNameDto.Create(
                    (int)MadrakTahsiliEnum.Hozavi ,
                    MadrakTahsiliEnum.Hozavi.GetPersianName()),
                  IdNameDto.Create(
                    (int)MadrakTahsiliEnum.Moadel ,
                    MadrakTahsiliEnum.Moadel.GetPersianName()),
                  IdNameDto.Create(
                    (int)MadrakTahsiliEnum.POSTPHD ,
                    MadrakTahsiliEnum.POSTPHD.GetPersianName()),
            };
        }
    }

    public static class MadrakTahsiliEnumExtension
    {
        public static string GetPersianName(this MadrakTahsiliEnum entityType)
        {
            switch (entityType)
            {
                case MadrakTahsiliEnum.Lisans: return "لیسانس";
                case MadrakTahsiliEnum.Foghlisans: return "فوق لیسانس";
                case MadrakTahsiliEnum.PHD: return "دکترا";
                case MadrakTahsiliEnum.Hozavi: return "حوزوی";
                case MadrakTahsiliEnum.Moadel: return "معادل";
                case MadrakTahsiliEnum.POSTPHD: return "فوق دکترا";
                default: return "تعریف نشده";
            }
        }
    }



}
