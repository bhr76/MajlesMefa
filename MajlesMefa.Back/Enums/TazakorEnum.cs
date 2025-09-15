using MajlesMefa.Back.Dtos.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Enums
{
    public enum TazakorEnum : byte
    {
        [Display(Name = "کتبی")]
        Katbi = 1,
        [Display(Name = "شفاهی")]
        Shafahi = 2,
    }

    public static class TazakorEnumHelper
    {
        public static List<IdNameDto> GetList()
        {
            return new List<IdNameDto>()
            {
                IdNameDto.Create(
                    (int)TazakorEnum.Katbi ,
                    TazakorEnum.Katbi.GetPersianName()),
                 IdNameDto.Create(
                    (int)TazakorEnum.Shafahi ,
                    TazakorEnum.Shafahi.GetPersianName()),

            };
        }
    }

    public static class TazakorEnumExtension
    {
        public static string GetPersianName(this TazakorEnum entityType)
        {
            switch (entityType)
            {
                case TazakorEnum.Katbi: return "کتبی";
                case TazakorEnum.Shafahi: return "شفاهی";
                default: return "تعریف نشده";
            }
        }
    }
}
