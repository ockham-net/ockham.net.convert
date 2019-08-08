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
            }

            return VBConvert.ToBoolean(value);
        }
    }
}
