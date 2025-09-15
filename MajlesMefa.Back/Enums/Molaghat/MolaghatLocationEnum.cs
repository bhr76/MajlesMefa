using MajlesMefa.Back.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.Back.Enums.Molaghat
{
    public enum MolaghatLocationEnum: byte
    {
        [Display(Name = "مجلس")]
        Majles =1,

        [Display(Name = "مجموعه تلاش")]
        Talaash = 2,

        [Display(Name = "دفتر وزیر")]
        Daftar_vazir = 3,

        [Display(Name = "دفتر دولت")]
        Daftar_dolat = 4,

        [Display(Name = "در محل سازمان")]
        Sazman = 5,
    }

    public static class MolaghatLocationEnumHelper
    {
        public static List<IdNameDto> GetList()
        {
            return new List<IdNameDto>()
            {
                IdNameDto.Create(
                    (int)MolaghatLocationEnum.Majles ,
                    MolaghatLocationEnum.Majles.GetPersianName()),
                 IdNameDto.Create(
                    (int)MolaghatLocationEnum.Talaash ,
                    MolaghatLocationEnum.Talaash.GetPersianName()),
                 IdNameDto.Create(
                    (int)MolaghatLocationEnum.Daftar_vazir ,
                    MolaghatLocationEnum.Daftar_vazir.GetPersianName()),
                 IdNameDto.Create(
                    (int)MolaghatLocationEnum.Daftar_dolat ,
                    MolaghatLocationEnum.Daftar_dolat.GetPersianName()),
                 IdNameDto.Create(
                    (int)MolaghatLocationEnum.Sazman ,
                    MolaghatLocationEnum.Sazman.GetPersianName()),
            };
        }
    }

    public static class MolaghatLocationEnumExtension
    {
        public static string GetPersianName(this MolaghatLocationEnum entityType)
        {
            switch (entityType)
            {
                case MolaghatLocationEnum.Majles: return "مجلس";
                case MolaghatLocationEnum.Talaash: return "مجموعه تلاش";
                case MolaghatLocationEnum.Daftar_dolat: return "‌دفتر دولت";
                case MolaghatLocationEnum.Daftar_vazir: return "دفتر وزیر";
                case MolaghatLocationEnum.Sazman: return "در محل سازمان" +
                        "";
                default: return "تعریف نشده";
            }
        }
    }

}
