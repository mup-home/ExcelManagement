namespace OMP.Shared.Extensions
{
    using System;
    using System.Globalization;

    public static class DateTimeExtensions
    {
        public static DateTime? ToUtc(this DateTime? value)
        {
            return value?.Kind == DateTimeKind.Utc ? value : value?.ToUniversalTime() ?? DateTime.UtcNow;
        }

        public static DateTime ToUtc(this DateTime value)
        {
            return value.Kind == DateTimeKind.Utc ? value : value.ToUniversalTime();
        }

        public static DateTime ToAppDateTime(this DateTime value)
        {
            return IsUtcDefaultAppFormat() ? value.ToUtc() : value;
        }

        public static DateTime? ToAppDateTime(this DateTime? value)
        {
            return IsUtcDefaultAppFormat() ? value.ToUtc() ?? DateTime.Now : value;
        }

        public static DateTime ToAppLocalDateTime(this DateTime value)
        {
            var appLocaleInfo = ConfigurationHelper.GetKeyValue("CultureInfo");
            var info = new CultureInfo(appLocaleInfo == string.Empty ? "es-ES" : appLocaleInfo);
            var success = DateTime.TryParse(value.ToString(), info, DateTimeStyles.None, out DateTime newValue);
            return success ? newValue : value;
        }

        private static bool IsUtcDefaultAppFormat()
        {
            var validKeyValue = bool.TryParse(ConfigurationHelper.GetKeyValue("UTC"), out bool useUtcFormat);
            return validKeyValue && useUtcFormat;
        }
    }
}
