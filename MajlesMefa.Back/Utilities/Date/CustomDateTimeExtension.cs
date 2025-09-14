using MD.PersianDateTime.Standard;

namespace MajlesMefa.Back.Utilities.Date
{
    public static class CustomDateTimeExtension
    {
        public static DateTime ToMiladiDateOffSet(this string shamsiDate)
        {
            var persianDate = PersianDateTime.Parse(shamsiDate);
            return persianDate.ToDateTime();
        }

        public static DateTime ToMiladiDate(this string shamsiDate)
        {
            if (string.IsNullOrEmpty(shamsiDate)) return DateTime.MinValue;
            var persianDate = PersianDateTime.Parse(shamsiDate);
            return persianDate.ToDateTime();
        }
        public static string ToPersianDateTime(this DateTime? dt, string format = "yyyy/MM/dd - HH:mm")
        {
            if (dt is null)
                return "";

            return new PersianDateTime(dt).ToString(format);
        }

        public static string ToPersianDateTime(this DateTime dt, string format = "yyyy/MM/dd - HH:mm")
        {
            return new PersianDateTime(dt).ToString(format);
        }

        public static string ToPersianDate(this DateTime dt, string format = "yyyy/MM/dd")
        {
            return new PersianDateTime(dt).ToString(format);
        }

        public static string GetMonth(this string dt)
        {            
            switch (dt.Substring(5, 2))
            {
                case "01": return "فروردین";
                case "02": return "اردیبهشت";
                case "03": return "خرداد";
                case "04": return "تیر";
                case "05": return "مرداد";
                case "06": return "شهریور";
                case "07": return "مهر";
                case "08": return "آبان";
                case "09": return "آذر";
                case "10": return "دی";
                case "11": return "بهمن";
                case "12": return "اسفند";
                default:return "mah";
            }
        }

        public static string GetYear(this string dt)
        {
            return dt.Substring(0, 4);
           
        }

        public static string ToPersianDateTime(this string dt)
        {
            return new PersianDateTime(DateTime.Parse(dt)).ToString("yyyy/MM/dd - HH:mm");
        }

        public static string RelativeDate(this DateTime theDate)
        {
            var thresholds = new Dictionary<long, string>();
            const int minute = 60;
            const int hour = 60 * minute;
            const int day = 24 * hour;
            thresholds.Add(60, "{0} ثانیه پیش");
            thresholds.Add(minute * 2, "یک دقیقه پیش");
            thresholds.Add(45 * minute, "{0} دقیقه پیش");
            thresholds.Add(120 * minute, "یکساعت پیش");
            thresholds.Add(day, "{0} ساعت پیش");
            thresholds.Add(day * 2, "دیروز");
            thresholds.Add(day * 30, "{0} روز پیش");
            thresholds.Add(day * 365, "{0} ماه پیش");
            thresholds.Add(long.MaxValue, "{0} سال پیش");

            var since = (DateTime.Now.Ticks - theDate.Ticks) / 10000000;
            foreach (var threshold in thresholds.Keys)
            {
                if (since >= threshold) continue;
                var t = new TimeSpan(DateTime.Now.Ticks - theDate.Ticks);
                return string.Format(thresholds[threshold], t.Days > 365 ? t.Days / 365 : t.Days > 0 ? t.Days : t.Hours > 0 ? t.Hours : t.Minutes > 0 ? t.Minutes : t.Seconds > 0 ? t.Seconds : 0);
            }
            return string.Empty;
        }
    }
}