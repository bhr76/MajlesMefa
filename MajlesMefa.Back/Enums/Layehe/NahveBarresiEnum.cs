using MajlesMefa.Back.Dtos.Common;
using MajlesMefa.Back.Enums.Tarh;
using MajlesMefa.Back.Utilities.EnumHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Enums.Layehe
{
    public enum NahveBarresiEnum
    {
        [Display(Name = "یک شوری")]
        YekShora = 1,
        [Display(Name = "دو شوری")]
        DoShora = 2,
        [Display(Name = "اصل 85")]
        Asl85 =3,
    }
    public static class NahveBarresiEnumHelper
    {
        public static List<IdNameDto> GetList()
        {
            return new List<IdNameDto>()
            {
                IdNameDto.Create(
                    (int)NahveBarresiEnum.YekShora ,
                    NahveBarresiEnum.YekShora.GetDisplayName()),
                 IdNameDto.Create(
                    (int)NahveBarresiEnum.DoShora ,
                    NahveBarresiEnum.DoShora.GetDisplayName()),
                 IdNameDto.Create(
                    (int)NahveBarresiEnum.Asl85 ,
                    NahveBarresiEnum.Asl85.GetDisplayName()),
            };
        }

        public static string GetPersianName(this NahveBarresiEnum entityType)
        {
            switch (entityType)
            {
                case NahveBarresiEnum.YekShora: return "یک شوری";
                case NahveBarresiEnum.DoShora: return "دو شوری";
                case NahveBarresiEnum.Asl85: return "اصل 85";
                default: return "تعریف نشده";
            }
        }

    }
}
