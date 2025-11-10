

namespace MajlesMefa.Back.Utilities.Convertor
{
    public static class PersianToEnglish
    {
        public static string ConvertPersianToEnglishNumber(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var persianNumbers = new[] { '۰', '۱', '۲', '۳', '۴', '۵', '۶', '۷', '۸', '۹' };
            var englishNumbers = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

            for (int i = 0; i < persianNumbers.Length; i++)
            {
                input = input.Replace(persianNumbers[i], englishNumbers[i]);
            }

            return input;
        }
    }
}
