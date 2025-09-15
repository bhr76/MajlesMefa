using MajlesMefa.Back.Dtos.Common;
using MajlesMefa.Back.Utilities.EnumHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Enums.Tarh
{
    public enum NatijeBarresiEnum: byte
    {
        [Display(Name = "تصویب شد")]
        TasvibShod =1,
        [Display(Name = "رد شد")]
        TasvibNashod =2,
        [Display(Name = "به تعویق افتاد")]
        BeTavighOftad = 3,
        [Display(Name = "در حال بررسی")]
        DarHaleBaresi = 4,
    }
    public static class NatijeBarresiEnumHelper
    {
        public static List<IdNameDto> GetList()
        {
            return new List<IdNameDto>()
            {
                 IdNameDto.Create(
                    (int)NatijeBarresiEnum.TasvibShod ,
                    NatijeBarresiEnum.TasvibShod.GetPersianName()),
                 IdNameDto.Create(
                    (int)NatijeBarresiEnum.TasvibNashod ,
                    NatijeBarresiEnum.TasvibNashod.GetPersianName()),
                 IdNameDto.Create(
                    (int)NatijeBarresiEnum.BeTavighOftad ,
                    NatijeBarresiEnum.BeTavighOftad.GetPersianName()),
                 IdNameDto.Create(
                    (int)NatijeBarresiEnum.DarHaleBaresi ,
                    NatijeBarresiEnum.DarHaleBaresi.GetPersianName()),
            };
        }
        public static string GetPersianName(this NatijeBarresiEnum entityType)
        {
            switch (entityType)
            {
                case NatijeBarresiEnum.TasvibShod: return "تصویب شد";
                case NatijeBarresiEnum.TasvibNashod: return "رد شد";
                case NatijeBarresiEnum.BeTavighOftad: return "به تعویق افتاد";
                case NatijeBarresiEnum.DarHaleBaresi: return "در حال بررسی";
                default: return "تعریف نشده";
            }
        }
    }
    
}
