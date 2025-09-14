using MajlesMefa.Back.Dtos.Common;
using System.ComponentModel.DataAnnotations;

namespace MajlesMefa.Back.Enums.Soval
{
    public enum QuestionStatusEnum : byte
    {
        [Display(Name = "اقناع")]
        Eghnaa = 1,

        [Display(Name = "انصراف")]
        Enseraf = 2,

        [Display(Name = "صحن(اعلام وصول)")]
        Sahn_elameVosool = 4,

        [Display(Name = "بایگانی")]
        Baygani = 5,

        [Display(Name = "تعویق")]
        Tavigh = 6,

        [Display(Name = "در دستور کار کمیسیون")]
        DastoorKarCommission = 7,

        [Display(Name = "صحن(در جلسه مطرح شده)-نماینده قانع شد")]
        Convinced = 8,

        [Display(Name = "صحن(در جلسه مطرح شده)-نماینده قانع نشد-به رای گذاشته شد(نمایندگان قانع شدند)")]
        NotConincedThenConvinced = 9,

        [Display(Name = "صحن(در جلسه مطرح شده)-نماینده قانع نشد-به رای گذاشته شد(نمایندگان قانع نشدند-کارت زرد)")]
        NotConvincedYellowCard = 10,
    }

    public static class QuestionStatusEnumHelper
    {
        public static List<IdNameDto> GetList()
        {
            return new List<IdNameDto>()
            {
                IdNameDto.Create(
                    (int)QuestionStatusEnum.Eghnaa ,
                    QuestionStatusEnum.Eghnaa.GetPersianName()),
                 IdNameDto.Create(
                    (int)QuestionStatusEnum.Enseraf ,
                    QuestionStatusEnum.Enseraf.GetPersianName()),
                 IdNameDto.Create(
                    (int)QuestionStatusEnum.Sahn_elameVosool ,
                    QuestionStatusEnum.Sahn_elameVosool.GetPersianName()),
                 IdNameDto.Create(
                    (int)QuestionStatusEnum.Baygani ,
                    QuestionStatusEnum.Baygani.GetPersianName()),
                 IdNameDto.Create(
                    (int)QuestionStatusEnum.Tavigh ,
                    QuestionStatusEnum.Tavigh.GetPersianName()),
                 IdNameDto.Create(
                    (int)QuestionStatusEnum.DastoorKarCommission ,
                    QuestionStatusEnum.DastoorKarCommission.GetPersianName()),
                 IdNameDto.Create(
                    (int)QuestionStatusEnum.Convinced ,
                    QuestionStatusEnum.Convinced.GetPersianName()),
                 IdNameDto.Create(
                    (int)QuestionStatusEnum.NotConincedThenConvinced ,
                    QuestionStatusEnum.NotConincedThenConvinced.GetPersianName()),
                 IdNameDto.Create(
                    (int)QuestionStatusEnum.NotConvincedYellowCard ,
                    QuestionStatusEnum.NotConvincedYellowCard.GetPersianName()),
            };
        }
    }

    public static class QuestionStatusEnumExtension
    {
        public static string GetPersianName(this QuestionStatusEnum entityType)
        {
            switch (entityType)
            {
                case QuestionStatusEnum.Eghnaa: return "اقناع";
                case QuestionStatusEnum.Enseraf: return "انصراف";
                case QuestionStatusEnum.Sahn_elameVosool: return "صحن(اعلام وصول)";
                case QuestionStatusEnum.Baygani: return "بایگانی";
                case QuestionStatusEnum.Tavigh: return "تعویق";
                case QuestionStatusEnum.DastoorKarCommission: return "در دستور کار کمیسیون";
                case QuestionStatusEnum.Convinced: return "صحن(در جلسه مطرح شده)-نماینده قانع شد";
                case QuestionStatusEnum.NotConincedThenConvinced: return "صحن(در جلسه مطرح شده)-نماینده قانع نشد-به رای گذاشته شد(نمایندگان قانع شدند)";
                case QuestionStatusEnum.NotConvincedYellowCard: return "صحن(در جلسه مطرح شده)-نماینده قانع نشد-به رای گذاشته شد(نمایندگان قانع نشدند-کارت زرد)";
                default:return ("تعریف نشده");
            }
        }
    }
}
