namespace OMP.Shared.Extensions
{
    using System;
    using System.Globalization;
    using System.Linq;

    public static class StringExtensions
    {
        /// <summary>
        /// Convert the string to Pascal Case replacing all non character 
        /// not digits or not letters by underscore character
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToPascalCase(this string value)
        {
            // Replace all non-letter and non-digits with an underscore and lowercase the rest.
            string _string = string.Concat(value?.Select(c => Char.IsLetterOrDigit(c) ? c.ToString().ToLower() : "_").ToArray());

            // Split the resulting string by underscore
            // Select first character, uppercase it and concatenate with the rest of the string
            var arr = _string?
                .Split(new[] { '_' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => $"{s.Substring(0, 1).ToUpper()}{s.Substring(1)}");

            // Join the resulting collection
            return string.Concat(arr);
        }

        /// <summary>
        /// Return true is the value is equal to "YES"
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Return a decimal if the string is valid number, otherwise return null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string value)
        {
            // var decimalSeparator = ConfigurationManager.AppSettings["DecimalSeparator"]?.Trim() ?? CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            const string decimalSeparator = ",";
            try
            {
                return decimal.Parse(value.ToAppDecimal(), new NumberFormatInfo { NumberDecimalSeparator = decimalSeparator });
            }
            catch (Exception)
            {
                throw new InvalidOperationException($"String '{value}' can´t be converted to valid Decimal number.");
            }
        }

        public static string ToAppDecimal(this string value)
        {
            // var decimalSeparator = ConfigurationManager.AppSettings["DecimalSeparator"]?.Trim() ?? CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            const string decimalSeparator = ",";
            if (decimalSeparator == ".")
            {
                value = value.Replace(",", decimalSeparator);
            }
            else if (decimalSeparator == ",")
            {
                value = value.Replace(".", decimalSeparator);
            }
            return value;
        }

        public static double ToDouble(this string value)
        {
            // var decimalSeparator = ConfigurationManager.AppSettings["DecimalSeparator"]?.Trim() ?? CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            const string decimalSeparator = ",";
            if (decimalSeparator == ".")
            {
                value = value.Replace(",", decimalSeparator);
            }
            else if (decimalSeparator == ",")
            {
                value = value.Replace(".", decimalSeparator);
            }
            try
            {
                return double.Parse(value, new NumberFormatInfo { NumberDecimalSeparator = decimalSeparator });
            }
            catch (Exception)
            {
                throw new InvalidOperationException($"String '{value}' can´t be converted to valid Double number.");
            }
        }

        /// <summary>
        ///  Return a short if the string is valid number, otherwise return null
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static short ToShort(this string value)
        {
            try
            {
                return short.Parse(value);
            }
            catch (Exception)
            {
                throw new InvalidOperationException($"String '{value}' can´t be converted to valid Short number.");
            }
        }

        public static int ToInt(this string value)
        {
            try
            {
                return int.Parse(value);
            }
            catch (Exception)
            {
                throw new InvalidOperationException($"String '{value}' can´t be converted to valid Integer number.");
            }
        }

        public static DateTime ToAppDateTime(this string value)
        {
            if (value.IsNullOrEmpty())
            {
                return DateTime.MinValue.ToAppDateTime();
            }

            return value.ToDateTime().ToAppDateTime();
        }
    }
}
