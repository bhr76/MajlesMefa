using MajlesMefa.Back.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.Back.Enums
{
    public enum MokatebeKonandeEnum: byte
    {
        [Display(Name = "ریاست جمهوری")]
        RiasatJomhoori = 1,
        
        [Display(Name = "مجلس")]
        Majles = 2,

        [Display(Name = "سازمان‌ها و وزارتخانه‌ها ")]
        Sazmanha = 3,

        [Display(Name = "متفرقه")]
        Motefareghe = 4,
    }

    public static class MokatebeKonandeEnumHelper
    {
        public static List<IdNameDto> GetList()
        {
            return new List<IdNameDto>()
            {
                IdNameDto.Create(
                    (int)MokatebeKonandeEnum.RiasatJomhoori ,
                    MokatebeKonandeEnum.RiasatJomhoori.GetPersianName()),
                 IdNameDto.Create(
                    (int)MokatebeKonandeEnum.Majles ,
                    MokatebeKonandeEnum.Majles.GetPersianName()),
                 IdNameDto.Create(
                    (int)MokatebeKonandeEnum.Sazmanha ,
                    MokatebeKonandeEnum.Sazmanha.GetPersianName()),
                 IdNameDto.Create(
                    (int)MokatebeKonandeEnum.Motefareghe ,
                    MokatebeKonandeEnum.Motefareghe.GetPersianName()),

            };
        }
    }

    public static class NokatebeKonandeEnumExtension
    {
        public static string GetPersianName(this MokatebeKonandeEnum entityType)
        {
            switch (entityType)
            {
                case MokatebeKonandeEnum.RiasatJomhoori: return "ریاست جمهوری";
                case MokatebeKonandeEnum.Majles: return "مجلس";
                case MokatebeKonandeEnum.Sazmanha: return "‌سازمان‌ها و وزارتخانه‌ها";
                case MokatebeKonandeEnum.Motefareghe: return "‌متفرقه";
                default: return "تعریف نشده";
            }
        }
    }
}
