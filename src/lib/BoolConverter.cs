using System;
using VBConvert = Microsoft.VisualBasic.CompilerServices.Conversions;

namespace Ockham.Data
{
    /// <summary>
    /// Simple utility for converting values to <see cref="bool"/>
    /// </summary>
    public static class BoolConverter
    {
        /// <summary>
        /// Convert the provided value to a <see cref="bool"/>. Strings are compared against
        /// <see cref="BooleanConvertOptions.TrueStrings"/> and <see cref="BooleanConvertOptions.FalseStrings"/>
        /// </summary> 
        /// <seealso cref="ConvertOptions.Booleans"/>
        /// <seealso cref="BooleanConvertOptions"/>
        public static bool ToBool(object value, ConvertOptions options)
        {
            if (value is bool) return (bool)value;
            if (value is string stringValue)
            {
                if (options.Booleans.TrueStrings.Contains(stringValue)) return true;
                if (options.Booleans.FalseStrings.Contains(stringValue)) return false;

                var stringOpts = options.Strings;
                if (stringOpts.TrimStart || stringOpts.TrimEnd)
                {
                    if (stringOpts.TrimAll) stringValue = stringValue.Trim();
                    else if (stringOpts.TrimStart) stringValue = stringValue.TrimStart();
                    else stringValue = stringValue.TrimEnd();

                    if (options.Booleans.TrueStrings.Contains(stringValue)) return true;
                    if (options.Booleans.FalseStrings.Contains(stringValue)) return false;
                }

                throw new InvalidCastException($"Cannot convert string '{stringValue}' to a boolean");
            }

            return VBConvert.ToBoolean(value);
        }
    }
}
