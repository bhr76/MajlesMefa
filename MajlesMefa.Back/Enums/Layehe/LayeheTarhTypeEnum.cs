using MajlesMefa.Back.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.Back.Enums.Tarh
{
    public enum LayeheTarhTypeEnum: byte
    {
        [Display(Name = "طرح")]
        Tarh =1,
        [Display(Name = "لایحه")]
        Layehe =2,
    }
    public static class LayeheTarhTypeEnumHelper
    {
        public static List<IdNameDto> GetList()
        {
            return new List<IdNameDto>()
            {
                 IdNameDto.Create(
                    (int)LayeheTarhTypeEnum.Tarh ,
                    LayeheTarhTypeEnum.Tarh.GetPersianName()),
                 IdNameDto.Create(
                    (int)LayeheTarhTypeEnum.Layehe ,
                    LayeheTarhTypeEnum.Layehe.GetPersianName()),
            };
        }
        public static string GetPersianName(this LayeheTarhTypeEnum entityType)
        {
            switch (entityType)
            {
                case LayeheTarhTypeEnum.Tarh: return "طرح";
                case LayeheTarhTypeEnum.Layehe: return "لایحه";
                default: return "تعریف نشده";
            }
        }
    }
    
}
