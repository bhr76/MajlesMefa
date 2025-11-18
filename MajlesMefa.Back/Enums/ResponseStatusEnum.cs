using MajlesMefa.Back.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.Back.Enums
{
    public enum ResponseStatusEnum : byte
    {
        [Display(Name = "در دست اقدام")]
        Inprogress = 0,

        [Display(Name = "پرداخت شده")]
        Mosbat = 1,

        [Display(Name = "رد درخواست")]
        Manfi = 2,

        [Display(Name = "در دست شعبه")]
        Shobe = 3,

    }

    public static class ResponseStatusEnumHelper
    {
        public static List<IdNameDto> GetList()
        {
            return new List<IdNameDto>()
            {
                IdNameDto.Create(
                    (int)ResponseStatusEnum.Inprogress ,
                    ResponseStatusEnum.Inprogress.GetPersianName()),
                IdNameDto.Create(
                    (int)ResponseStatusEnum.Mosbat ,
                    ResponseStatusEnum.Mosbat.GetPersianName()),
                 IdNameDto.Create(
                    (int)ResponseStatusEnum.Manfi ,
                    ResponseStatusEnum.Manfi.GetPersianName()),
                 IdNameDto.Create(
                    (int)ResponseStatusEnum.Shobe ,
                    ResponseStatusEnum.Shobe.GetPersianName()),


            };
        }
    }

    public static class ResponseStatusEnumExtension
    {
        public static string GetPersianName(this ResponseStatusEnum entityType)
        {
            switch (entityType)
            {
                case ResponseStatusEnum.Mosbat: return "پرداخت شده";
                case ResponseStatusEnum.Manfi: return "رد درخواست";
                case ResponseStatusEnum.Inprogress: return "در دست اقدام";
                case ResponseStatusEnum.Shobe: return "در دست شعبه";

                default: return "بدون پاسخ";
            }
        }
    }
}
