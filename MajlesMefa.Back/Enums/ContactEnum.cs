using MajlesMefa.Back.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.Back.Enums
{
    public enum ContactEnum: byte
    {
        [Display(Name = "وزیر")]
        Vazir = 1,
        
        [Display(Name = "معاونت مجلس‌")]
        MoavenatMajles = 2,

        [Display(Name = "سایر معاونت‌ها")]
        Others = 3,
    }

    public static class ContactEnumHelper
    {
        public static List<IdNameDto> GetList()
        {
            return new List<IdNameDto>()
            {
                IdNameDto.Create(
                    (int)ContactEnum.Vazir ,
                    ContactEnum.Vazir.GetPersianName()),
                 IdNameDto.Create(
                    (int)ContactEnum.MoavenatMajles ,
                    ContactEnum.MoavenatMajles.GetPersianName()),
                 IdNameDto.Create(
                    (int)ContactEnum.Others ,
                    ContactEnum.Others.GetPersianName()),
            };
        }
    }

    public static class ContactEnumExtension
    {
        public static string GetPersianName(this ContactEnum entityType)
        {
            switch (entityType)
            {
                case ContactEnum.Vazir: return "وزیر";
                case ContactEnum.MoavenatMajles: return "معاونت مجلس";
                case ContactEnum.Others: return "‌سایر معاونت‌ها";
                default: return "تعریف نشده";
            }
        }
    }
}
