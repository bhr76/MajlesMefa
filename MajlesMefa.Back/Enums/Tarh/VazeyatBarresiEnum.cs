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
    public enum VazeyatBarresiEnum: byte
    {
        [Display(Name = "عادی")]
        Adi =1,
        [Display(Name = "یک فوریتی")]
        YekForyati =2,
        [Display(Name = "دو فوریتی")]
        DoForyati =3,
        [Display(Name = "سه فوریتی")]
        SeForyati =4,
        [Display(Name = "تغییر دستور بررسی")]
        TaghirDastoorBarresi =5,
    }
    public static class VazeyatBarresiHelper
    {
        public static List<IdNameDto> GetList()
        {
            return new List<IdNameDto>()
            {
                IdNameDto.Create(
                    (int)VazeyatBarresiEnum.Adi ,
                    VazeyatBarresiEnum.Adi.GetDisplayName()),
                 IdNameDto.Create(
                    (int)VazeyatBarresiEnum.TaghirDastoorBarresi ,
                    VazeyatBarresiEnum.TaghirDastoorBarresi.GetDisplayName()),
                 IdNameDto.Create(
                    (int)VazeyatBarresiEnum.YekForyati ,
                    VazeyatBarresiEnum.YekForyati.GetDisplayName()),
                 IdNameDto.Create(
                    (int)VazeyatBarresiEnum.DoForyati ,
                    VazeyatBarresiEnum.DoForyati.GetDisplayName()),
                 IdNameDto.Create(
                    (int)VazeyatBarresiEnum.SeForyati ,
                    VazeyatBarresiEnum.SeForyati.GetDisplayName()),
            };
        }

        public static string GetPersianName(this VazeyatBarresiEnum entityType)
        {
            switch (entityType)
            {
                case VazeyatBarresiEnum.Adi: return "عادی";
                case VazeyatBarresiEnum.YekForyati: return "یک فوریتی";
                case VazeyatBarresiEnum.DoForyati: return "دو فوریتی";
                case VazeyatBarresiEnum.SeForyati: return "سه فوریتی";
                case VazeyatBarresiEnum.TaghirDastoorBarresi: return "تغییر دستور بررسی";
                default: return "تعریف نشده";
            }
        }
    }
}
