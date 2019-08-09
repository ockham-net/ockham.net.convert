﻿using System;

namespace Ockham.Data
{
    // ------------------------------------------------------------------
    // ** HEY, YOU, DEVELOPER! **
    // 
    //  Do not modify this file directly!
    //  
    //  Update and rerun the code generating script Generate-ConvertAliases.ps1 with $instance = $true
    // ------------------------------------------------------------------

    // Type-specific conversion wrappers for underlying To method
    public partial class Converter
    {
        /// <summary>
        /// Convert any input value to an equivalent boolean value. Attempting to convert
        /// a value that has no meaningful boolean equivalent, including an empty value,
        /// will cause an exception to be raised.
        /// </summary>
        /// <param name="value">Any value</param>
        public bool ToBoolean(object value)
        {
            if (value is bool boolValue) return boolValue;
            return Convert.ToStruct<bool>(value, this.Options);
        }

        /// <summary>
        /// Convert any input value to an equivalent date time value. Attempting to convert
        /// a value that has no meaningful date or time equivalent, including an empty value,
        /// will cause an exception to be raised.
        /// </summary>
        /// <param name="value">Any value</param>
        public DateTime ToDateTime(object value)
        {
            if (value is DateTime dateTimeValue) return dateTimeValue;
            return Convert.ToStruct<DateTime>(value, this.Options);
        }

        /// <summary>
        /// Convert any input value to an equivalent exact decimal value. Attempting to convert
        /// a value that has no meaningful decimal equivalent, including an empty value,
        /// will cause an exception to be raised.
        /// </summary>
        /// <param name="value">Any value</param>
        public decimal ToDecimal(object value)
        {
            if (value is decimal decimalValue) return decimalValue;
            return Convert.ToStruct<decimal>(value, this.Options);
        }

        /// <summary>
        /// Convert any input value to an equivalent double-precision floating point number value. Attempting to convert
        /// a value that has no meaningful floating point equivalent, including an empty value,
        /// will cause an exception to be raised.
        /// </summary>
        /// <param name="value">Any value</param>
        public double ToDouble(object value)
        {
            if (value is double doubleValue) return doubleValue;
            return Convert.ToStruct<double>(value, this.Options);
        }

        /// <summary>
        /// Convert any input value to an equivalent 128-bit globally unique identifier value. Attempting to convert
        /// a value that has no meaningful GUID equivalent, including an empty value,
        /// will cause an exception to be raised.
        /// </summary>
        /// <param name="value">Any value</param>
        public Guid ToGuid(object value)
        {
            if (value is Guid guidValue) return guidValue;
            return Convert.ToStruct<Guid>(value, this.Options);
        }

        /// <summary>
        /// Convert any input value to an equivalent 32-bit signed integer value. Attempting to convert
        /// a value that has no meaningful integer equivalent, including an empty value,
        /// will cause an exception to be raised.
        /// </summary>
        /// <param name="value">Any value</param>
        public int ToInt32(object value)
        {
            if (value is int intValue) return intValue;
            return Convert.ToStruct<int>(value, this.Options);
        }

        /// <summary>
        /// Convert any input value to an equivalent 64-bit signed integer value. Attempting to convert
        /// a value that has no meaningful integer equivalent, including an empty value,
        /// will cause an exception to be raised.
        /// </summary>
        /// <param name="value">Any value</param>
        public long ToInt64(object value)
        {
            if (value is long longValue) return longValue;
            return Convert.ToStruct<long>(value, this.Options);
        }

        /// <summary>
        /// Convert any input value to an equivalent string value. Attempting to convert
        /// a value that has no meaningful string equivalent, including an empty value,
        /// will cause an exception to be raised.
        /// </summary>
        /// <param name="value">Any value</param>
        public string ToString(object value)
        {
            if (value is string stringValue) return stringValue;
            return (string)Convert.ToReference(value, typeof(string), this.Options, false, null);
        }

        /// <summary>
        /// Convert any input value to an equivalent timespan value. Attempting to convert
        /// a value that has no meaningful timespan equivalent, including an empty value,
        /// will cause an exception to be raised.
        /// </summary>
        /// <param name="value">Any value</param>
        public TimeSpan ToTimeSpan(object value)
        {
            if (value is TimeSpan timeSpanValue) return timeSpanValue;
            return Convert.ToStruct<TimeSpan>(value, this.Options);
        }
    }
}