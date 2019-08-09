using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Ockham.Data
{
    /// <summary>
    /// Static utility for inspecting values
    /// </summary>
    public static class Value
    {
        /// <summary>
        /// Test if the input value is a non-null numeric type 
        /// or a string that can be parsed as a base-10 number.  
        /// </summary>
        /// <remarks>To detect valid hexadecimal strings, use <see cref="IsNumeric(object, ConvertOptions)"/></remarks>
        /// <param name="value"></param> 
        public static bool IsNumeric(object value) => IsNumeric(value, ConvertOptions.Default);

        /// <summary>
        /// Test if the input value is a non-null numeric type 
        /// or a string that can be parsed as a number, with detection
        /// of hex strings controlled by ConvertOptions flags
        /// </summary>
        /// <param name="value"></param> 
        /// <param name="options"></param>  
        public static bool IsNumeric(object value, ConvertOptions options) => IsNumeric(value, options.Numbers.ParseFlags);

        /// <summary>
        /// Test if the input value is a non-null numeric type 
        /// or a string that can be parsed as a number, with detection
        /// of hex strings controlled by <paramref name="parseFlags"/>
        /// </summary>
        /// <param name="value"></param> 
        /// <param name="parseFlags"></param>  
        public static bool IsNumeric(object value, ParseNumericStringFlags parseFlags)
            => IsNumeric(value, parseFlags, out _, out _);

        /// <summary>
        /// Test if the input value is a non-null numeric type 
        /// or a string that can be parsed as a number, with detection
        /// of hex strings controlled by <paramref name="parseFlags"/>
        /// </summary>
        /// <param name="value"></param> 
        /// <param name="parseFlags"></param>  
        /// <param name="base">The radix (base) of the number</param>
        /// <param name="hasSeparator">True if the input was a string and included a digit separator character</param>
        internal static bool IsNumeric(object value, ParseNumericStringFlags parseFlags, out int @base, out bool hasSeparator)
        {
            @base = 10;
            hasSeparator = false;

            if (null == value) return false;
            if (value is string sValue)
            {
                if (double.TryParse(sValue, out _)) return true;

                bool allowDigitSep = (parseFlags & ParseNumericStringFlags.AllowDigitSeparator) != 0;
                if (allowDigitSep)
                {
                    bool changed = false;
                    while (sValue.IndexOf('_') >= 0)
                    {
                        sValue = Regex.Replace(sValue, "([0-9a-fA-F])_+([0-9a-fA-F])", "$1$2");
                        changed = true;
                        hasSeparator = true;
                    }

                    if (changed && double.TryParse(sValue, out _)) return true;
                }

                if ((parseFlags & ParseNumericStringFlags.HexString) != 0)
                {
                    if (Regex.IsMatch(sValue, @"^\s*0[xX][0-9a-fA-F]+$"))
                    {
                        @base = 16;
                        return true;
                    }
                }

                if ((parseFlags & ParseNumericStringFlags.OctalString) != 0)
                {
                    if (Regex.IsMatch(sValue, @"^\s*0[oO][0-7]+$"))
                    {
                        @base = 8;
                        return true;
                    }
                }

                if ((parseFlags & ParseNumericStringFlags.BinaryString) != 0)
                {
                    if (Regex.IsMatch(sValue, @"^\s*0[bB][01]+$"))
                    {
                        @base = 2;
                        return true;
                    }
                }

                return false;
            }
            return TypeInspection.IsNumberType(value.GetType());
        }

        /// <summary>
        /// Determine if the value represents the default value (Nothing in Visual Basic) for the value's type. 
        /// A value of null (Nothing in Visual Basic) will always return true.
        /// </summary> 
        public static bool IsDefault(object value)
        {
            if (null == value) return true;
            Type type = value.GetType();
            if (!type.IsValueType) return false;

            object defaultValue = Activator.CreateInstance(type);
            return object.Equals(value, defaultValue);
        }

        /// <summary>
        /// Returns true if the object is null or DBNull
        /// </summary>
        /// <param name="value">A value of any type</param> 
        public static bool IsNull(object value)
        {
            return null == value || value is DBNull;
        }

        /// <summary>
        /// Test whether the input value is null or DBNull. Will also return true if <paramref name="emptyStringAsNull"/> is true and <paramref name="value"/> is an empty string.
        /// </summary> 
        public static bool IsNull(object value, bool emptyStringAsNull)
        {
            if (null == value) return true;
            if (value is DBNull) return true;
            if (emptyStringAsNull && (value is string) && ((string)value) == string.Empty) return true;
            return false;
        }

        /// <summary>
        /// Test whether the input value is null or DBNull. Empty or whitespace strings 
        /// will be treated as null according to options on <paramref name="options"/>
        /// </summary> 
        public static bool IsNull(object value, ConvertOptions options)
        {
            if (null == value) return true;
            if (value is DBNull) return true;
            if (options.StringToNull && (value is string stringValue))
            {
                if (string.IsNullOrEmpty(stringValue)) return true;
                if (options.WhitespaceToNull && string.IsNullOrWhiteSpace(stringValue)) return true;
            }
            return false;
        }

        /// <summary>
        /// Returns true if the object is null, DBNull, or an empty string
        /// </summary>
        /// <param name="value">A value of any type</param> 
        public static bool IsNullOrEmpty(object value)
        {
            return null == value || value is DBNull || (value is string && ((string)value == string.Empty));
        }

        /// <summary>
        /// Returns true if the object is null, DBNull, or an empty or whitespace string
        /// </summary>
        /// <param name="value">A value of any type</param> 
        public static bool IsNullOrWhitespace(object value)
        {
            if (null == value) return true;
            if (value is DBNull) return true;
            if (value is string) return string.IsNullOrWhiteSpace((string)value);
            return false;
        }
    }
}
