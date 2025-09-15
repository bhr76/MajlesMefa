using MajlesMefa.Back.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.Back.Enums
{
    public enum MokatebeTypeEnum: byte
    {
        [Display(Name = "عادی")]
        Adi = 1,
        
        [Display(Name = "پی‌نوشت")]
        PeyNevesht = 2,

        [Display(Name = "اصل 90")]
        Asl90 = 3,
    }

    public static class MokatebeTypeEnumHelper
    {
        public static List<IdNameDto> GetList()
        {
            return new List<IdNameDto>()
            {
                IdNameDto.Create(
                    (int)MokatebeTypeEnum.Adi ,
                    MokatebeTypeEnum.Adi.GetPersianName()),
                 IdNameDto.Create(
                    (int)MokatebeTypeEnum.PeyNevesht ,
                    MokatebeTypeEnum.PeyNevesht.GetPersianName()),
                 IdNameDto.Create(
                    (int)MokatebeTypeEnum.Asl90 ,
                    MokatebeTypeEnum.Asl90.GetPersianName()),
            };
        }
    }

    public static class NokatebeTypeEnumExtension
    {
        public static string GetPersianName(this MokatebeTypeEnum entityType)
        {
            switch (entityType)
            {
                case MokatebeTypeEnum.Adi: return "عادی";
                case MokatebeTypeEnum.PeyNevesht: return "پی‌نوشت";
                case MokatebeTypeEnum.Asl90: return "‌اصل 90";
                default: return "تعریف نشده";
            }
        }
    }
}
