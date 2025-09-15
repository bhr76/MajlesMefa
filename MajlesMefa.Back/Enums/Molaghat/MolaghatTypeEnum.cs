using MajlesMefa.Back.Dtos.Common;
using MajlesMefa.Back.Enums.Soval;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.Back.Enums.Molaghat
{
    public enum MolaghatTypeEnum: byte
    {
        [Display(Name = "فردی")]
        Fardi =1,

        [Display(Name = "مجمع استانی")]
        JamiOstani = 2,

        [Display(Name = "فراکسیون")]
        JamiFracsion = 3,

        [Display(Name = "کمیسیون")]
        JamiComission = 4,
    }

    public static class MolaghatTypeEnumHelper
    {
        public static List<IdNameDto> GetList()
        {
            return new List<IdNameDto>()
            {
                IdNameDto.Create(
                    (int)MolaghatTypeEnum.Fardi ,
                    MolaghatTypeEnum.Fardi.GetPersianName()),
                 IdNameDto.Create(
                    (int)MolaghatTypeEnum.JamiOstani ,
                    MolaghatTypeEnum.JamiOstani.GetPersianName()),
                 IdNameDto.Create(
                    (int)MolaghatTypeEnum.JamiFracsion ,
                    MolaghatTypeEnum.JamiFracsion.GetPersianName()),
                 IdNameDto.Create(
                    (int)MolaghatTypeEnum.JamiComission ,
                    MolaghatTypeEnum.JamiComission.GetPersianName()),
            };
        }
    }

    public static class MolaghatTypeEnumExtension
    {
        public static string GetPersianName(this MolaghatTypeEnum entityType)
        {
            switch (entityType)
            {
                case MolaghatTypeEnum.Fardi: return "فردی";
                case MolaghatTypeEnum.JamiOstani: return "مجمع استانی";
                case MolaghatTypeEnum.JamiComission: return "کمیسیون‌ها";
                case MolaghatTypeEnum.JamiFracsion: return "فراکسیون‌ها";
                default: return "تعریف نشده";
            }
        }
    }

}
