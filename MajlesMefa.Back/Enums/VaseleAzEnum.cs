using MajlesMefa.Back.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.Back.Enums
{
    public enum VaseleAzEnum : byte
    {
        [Display(Name = "بدون واصله")]
        None = 0,
        [Display(Name = "ریاست جمهوری")]
        RiasatJomhoori = 1,
        [Display(Name = "هیئت رئیسه")]
        HeyatRayise = 2,
    }

    public static class VaseleAzEnumHelper
    {
        public static List<IdNameDto> GetList()
        {
            return new List<IdNameDto>()
            {
                IdNameDto.Create(
                    (int)VaseleAzEnum.None ,
                    VaseleAzEnum.None.GetPersianName()),
                IdNameDto.Create(
                    (int)VaseleAzEnum.RiasatJomhoori ,
                    VaseleAzEnum.RiasatJomhoori.GetPersianName()),
                 IdNameDto.Create(
                    (int)VaseleAzEnum.HeyatRayise ,
                    VaseleAzEnum.HeyatRayise.GetPersianName()),

            };
        }
    }

    public static class VaseleAzEnumExtension
    {
        public static string GetPersianName(this VaseleAzEnum entityType)
        {
            switch (entityType)
            {
                case VaseleAzEnum.None: return "بدون واصله";
                case VaseleAzEnum.RiasatJomhoori: return "ریاست جمهوری";
                case VaseleAzEnum.HeyatRayise: return "هیئت رئیسه";
                default: return "تعریف نشده";
            }
        }
    }
}
