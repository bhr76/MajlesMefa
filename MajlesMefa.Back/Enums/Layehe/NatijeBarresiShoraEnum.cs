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
    public enum NatijeBarresiShoraEnum: byte
    {
        [Display(Name = "قانون شد")]
        TasvibShod =1,
        [Display(Name = "رد شد")]
        TasvibNashod =2,
    }
    public static class NatijeBarresiShoraEnumHelper
    {
        public static List<IdNameDto> GetList()
        {
            return new List<IdNameDto>()
            {
                 IdNameDto.Create(
                    (int)NatijeBarresiShoraEnum.TasvibShod ,
                    NatijeBarresiShoraEnum.TasvibShod.GetPersianName()),
                 IdNameDto.Create(
                    (int)NatijeBarresiShoraEnum.TasvibNashod ,
                    NatijeBarresiShoraEnum.TasvibNashod.GetPersianName()),
            };
        }
        public static string GetPersianName(this NatijeBarresiShoraEnum entityType)
        {
            switch (entityType)
            {
                case NatijeBarresiShoraEnum.TasvibShod: return "قانون شد";
                case NatijeBarresiShoraEnum.TasvibNashod: return "رد شد";
                default: return "تعریف نشده";
            }
        }
    }
    
}
