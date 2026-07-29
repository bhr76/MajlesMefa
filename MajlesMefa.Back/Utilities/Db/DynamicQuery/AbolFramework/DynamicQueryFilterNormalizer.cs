using MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework.Models;

using System.Reflection;


namespace MajlesMefa.Back.Utilities.Db.DynamicQuery.AbolFramework
{
    public static class DynamicQueryFilterNormalizer
    {
        public static void NormalizeFilters(TableRequestModel requestModel, Type targetType)
        {
            if (requestModel?.Filter == null) return;
            NormalizeFilterNode(requestModel.Filter, targetType);
        }

        private static void NormalizeFilterNode(FilterModel filter, Type type)
        {
            if (filter == null) return;

            // ۱. اگر فیلتر گروهی (کامپوزیت) است، فرزندان آن را به صورت بازگشتی پردازش کن
            if (filter.Filters != null && filter.Filters.Any())
            {
                foreach (var subFilter in filter.Filters)
                {
                    NormalizeFilterNode(subFilter, type);
                }
                return;
            }

            // ۲. بررسی تک فیلتر
            if (string.IsNullOrWhiteSpace(filter.Field)) return;

            var prop = type.GetProperty(filter.Field,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (prop == null) return;

            var targetType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;

            // بررسی می‌کنیم که آیا نوع داده پراپرتی مبدا Enum است یا خیر
            if (!targetType.IsEnum) return;
            if (filter.Value == null) return;

            try
            {
                var rawValue = filter.Value.ToString();

                // اگر مقدار خالی فرستاده شده بود
                if (string.IsNullOrWhiteSpace(rawValue))
                {
                    filter.Value = null;
                    return;
                }

                // الف) اگر مقدار به صورت رشته عددی (مثلا "1") باشد
                if (int.TryParse(rawValue, out var intVal))
                {
                    filter.Value = Enum.ToObject(targetType, intVal);
                }
                else
                {
                    // ب) اگر نام عضو Enum فرستاده شده باشد (مثلا "Inprogress")
                    filter.Value = Enum.Parse(targetType, rawValue, ignoreCase: true);
                }
            }
            catch
            {
                // در صورت بروز خطا، فیلتر را دستکاری نمی‌کنیم تا خطا در استک مشخص شود
            }
        }
    }
}
