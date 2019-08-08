using System;
using System.Text.RegularExpressions;
using VBConvert = Microsoft.VisualBasic.CompilerServices.Conversions;

namespace Ockham.Data
{
    /// <summary>
    /// A utility for converting values to <see cref="TimeSpan"/>
    /// </summary>
    public static class TimeSpanConverter
    {
        private const string RX_MINUTES = @"^(?<minutes>[0-9]{1,2}):[0-9]{2}(\.[0-9]+)?$";

        /// <summary>
        /// Convert the provided value to a <see cref="TimeSpan"/>
        /// </summary>  
        public static TimeSpan ToTimeSpan(object value, ConvertOptions options)
        {
            if (value is TimeSpan) return (TimeSpan)value;
            if (value is string && Regex.IsMatch((string)value, @"^\s*\d+\.\d+\s*$"))
            {
                // Treat decimal string as seconds, not ticks
                return TimeSpan.FromSeconds(Convert.To<double>(value, options));
            }
            else if (value != null && Value.IsNumeric(value, options))
            {
                return TimeSpan.FromTicks(Convert.To<long>(value, options));
            }
            else if (value is string stringValue)
            {
                var m = Regex.Match(stringValue, RX_MINUTES);
                if (m.Success)
                {
                    // The input is a minutes : seconds string. The built-in TimeSpan.Parse requires a leading
                    // hours segment as well. Append the 0: hours segment, and also pad the minutes segment with
                    // a leading 0 if necessary
                    stringValue = "0:" + (m.Groups["minutes"].Value.Length == 2 ? "" : "0") + stringValue;
                }

                return TimeSpan.Parse(stringValue);
            }
            else if (value is DateTime)
            {
                return TimeSpan.FromTicks(((DateTime)value).Ticks);
            }

            // If the input is not a TimeSpan, this will throw an invalid cast exception in the localized language:
            return (TimeSpan)value;
        }
    }
}
