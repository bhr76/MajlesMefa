using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajlesMefa.Back.Utilities.Convertor
{
    public static class CustomCurrencyFormat
    {
        public static string ShowCurrencyFormat(this string digit)
        {
            if(digit != "")
            {
                int index = digit.IndexOf(".");
                if (index >= 0)
                    digit = digit.Substring(0, index);

                string res2 = String.Format("{0:#,#}", Convert.ToInt64(digit));
                return res2;
            }else
                return digit;
            

            //return Convert.ToInt64(digit).ToString("C", CultureInfo.CreateSpecificCulture("fa-IR"));          

        }

        public static string ShowCurrencyFormat(this int digit)
        {
            //return digit.ToString("C", CultureInfo.CreateSpecificCulture("fa-IR"));
            string res2 = String.Format("{0:#,#}", digit);
            return res2;
        }

        public static string ShowCurrencyFormat(this decimal? digit)
        {
            //return digit.ToString("C", CultureInfo.CreateSpecificCulture("fa-IR"));
            string res2 = String.Format("{0:#,#}", digit);
            return res2;
        }

        public static string ShowCurrencyFormat(this int? digit)
        {
            //string sh = digit.HasValue ? digit.Value.ToString("C", CultureInfo.CreateSpecificCulture("fa-IR")) : string.Empty;
            string sh = digit.HasValue ? String.Format("{0:#,#}", digit) : string.Empty;
            return sh;
        }
        public static string ShowCurrencyFormat(this long digit)
        {
            //return digit.ToString("C", CultureInfo.CreateSpecificCulture("fa-IR"));
            string res2 = String.Format("{0:#,#}", digit);
            return res2;
        }
        public static string ShowCurrencyFormat(this long? digit)
        {
            //string sh = digit.HasValue ? digit.Value.ToString("C", CultureInfo.CreateSpecificCulture("fa-IR")) : string.Empty;
            string sh = digit.HasValue ? String.Format("{0:#,#}", digit) : string.Empty;
            return sh;
        }
    }
}