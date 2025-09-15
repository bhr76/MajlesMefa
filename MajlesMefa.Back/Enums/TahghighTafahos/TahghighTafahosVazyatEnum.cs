using MajlesMefa.Back.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.Back.Enums.TahghighTafahos
{
    public enum TahghighTafahosVazyatEnum : byte
    {
        [Display(Name = "رد")]
        Rad = 1,

        [Display(Name = "مسکوت")]
        Maskoot = 2,

        [Display(Name = "تصویب در کمیسیون")]
        TasvibDarCommission = 3,

        [Display(Name = "تصویب در صحن")]
        TasvibDarSahn = 4,

        [Display(Name = "در دست پیگیری")]
        DarDastPeygiri = 5,
    }

    public static class TahghighTafahosVazyatEnumHelper
    {
        public static List<IdNameDto> GetList()
        {
            return new List<IdNameDto>()
            {
                IdNameDto.Create(
                    (int)TahghighTafahosVazyatEnum.Rad ,
                    TahghighTafahosVazyatEnum.Rad.GetPersianName()),
                 IdNameDto.Create(
                    (int)TahghighTafahosVazyatEnum.Maskoot ,
                    TahghighTafahosVazyatEnum.Maskoot.GetPersianName()),
                 IdNameDto.Create(
                    (int)TahghighTafahosVazyatEnum.TasvibDarCommission ,
                    TahghighTafahosVazyatEnum.TasvibDarCommission.GetPersianName()),
                 IdNameDto.Create(
                    (int)TahghighTafahosVazyatEnum.TasvibDarSahn ,
                    TahghighTafahosVazyatEnum.TasvibDarSahn.GetPersianName()),
                 IdNameDto.Create(
                    (int)TahghighTafahosVazyatEnum.DarDastPeygiri ,
                    TahghighTafahosVazyatEnum.DarDastPeygiri.GetPersianName()),
            };
        }
    }

    public static class TahghighTafahosVazyatEnumExtension
    {
        public static string GetPersianName(this TahghighTafahosVazyatEnum entityType)
        {
            switch (entityType)
            {
                case TahghighTafahosVazyatEnum.Rad: return "رد";
                case TahghighTafahosVazyatEnum.Maskoot: return "مسکوت";
                case TahghighTafahosVazyatEnum.TasvibDarSahn: return "تصویب در صحن";
                case TahghighTafahosVazyatEnum.TasvibDarCommission: return "تصویب در کمیسیون";
                case TahghighTafahosVazyatEnum.DarDastPeygiri: return "در دست پیگیری";
                default: return "تعریف نشده";
            }
        }
    }



}
