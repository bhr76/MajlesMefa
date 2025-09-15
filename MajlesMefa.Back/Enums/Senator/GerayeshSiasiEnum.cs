using MajlesMefa.Back.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.Back.Enums.Senator
{
    public enum GerayeshSiasiEnum : byte
    {
        [Display(Name = "اصولگرا")]
        Osoolgara = 1,

        [Display(Name = "اصلاح طلب")]
        EslahTalab = 2,

        [Display(Name = "مستقل")]
        Mostaghel = 3,

        [Display(Name = "لیست ائتلاف")]
        ListEtelaf = 4,

        [Display(Name = "سایر")]
        Sayer = 5,

    }

    public static class GerayeshSiasiEnumHelper
    {
        public static List<IdNameDto> GetList()
        {
            return new List<IdNameDto>()
            {
                IdNameDto.Create(
                    (int)GerayeshSiasiEnum.Osoolgara ,
                    GerayeshSiasiEnum.Osoolgara.GetPersianName()),
                 IdNameDto.Create(
                    (int)GerayeshSiasiEnum.EslahTalab ,
                    GerayeshSiasiEnum.EslahTalab.GetPersianName()),
                 IdNameDto.Create(
                    (int)GerayeshSiasiEnum.Mostaghel ,
                    GerayeshSiasiEnum.Mostaghel.GetPersianName()),
                 IdNameDto.Create(
                    (int)GerayeshSiasiEnum.ListEtelaf ,
                    GerayeshSiasiEnum.ListEtelaf.GetPersianName()),
                 IdNameDto.Create(
                    (int)GerayeshSiasiEnum.Sayer ,
                    GerayeshSiasiEnum.Sayer.GetPersianName()),
            };
        }
    }

    public static class GerayeshSiasiEnumExtension
    {
        public static string GetPersianName(this GerayeshSiasiEnum entityType)
        {
            switch (entityType)
            {
                case GerayeshSiasiEnum.Osoolgara: return "اصولگرا";
                case GerayeshSiasiEnum.EslahTalab: return "اصلاح طلب";
                case GerayeshSiasiEnum.Mostaghel: return "مستقل";
                case GerayeshSiasiEnum.ListEtelaf: return "لیست ائتلاف";
                case GerayeshSiasiEnum.Sayer: return "سایر";
                default: return "تعریف نشده";
            }
        }
    }



}
