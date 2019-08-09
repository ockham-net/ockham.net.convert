using System;

namespace Ockham.Data
{
    public static partial class Convert
    {
        /// <summary>
        /// Converts null or DBNull to null, and returns other values as is.. 
        /// </summary> 
        /// <param name="value">A value of any type</param>
        public static object ToNull(object value)
            => Value.IsNull(value) ? null : value;

        /// <summary>
        /// Converts null or DBNull to null, and returns other values as is.. Empty or strings will also be converted to null 
        /// is <paramref name="emptyStringAsNull"/> is true
        /// </summary>
        /// <param name="value">A value of any type</param> 
        /// <param name="emptyStringAsNull">Whether to treat an empty string as null</param>
        public static object ToNull(object value, bool emptyStringAsNull)
            => Value.IsNull(value, emptyStringAsNull) ? null : value;

        /// <summary>
        /// Converts null or DBNull to null, and returns other values as is.. Empty or whitespace strings or other values may
        /// also be converted to null depending on the configuration of the provided <see cref="ConvertOptions"/>
        /// </summary>
        /// <param name="value">A value of any type</param> 
        /// <param name="options"></param>
        /// <seealso cref="ConvertOptions.Strings"/>
        /// <seealso cref="StringConvertOptions.AsNullOption"/>
        public static object ToNull(object value, ConvertOptions options)
            => Value.IsNull(value, options) ? null : value;

        /// <summary>
        /// Converts null or DBNull to DBNull, and returns other values as is.
        /// </summary> 
        /// <param name="value">A value of any type</param>
        public static object ToDBNull(object value)
            => Value.IsNull(value) ? DBNull.Value : value;

        /// <summary>
        /// Converts null or DBNull to DBNull, and returns other values as is.. Empty or strings will also be converted to null 
        /// is <paramref name="emptyStringAsNull"/> is true
        /// </summary>
        /// <param name="value">A value of any type</param> 
        /// <param name="emptyStringAsNull">Whether to treat an empty string as null</param>
        public static object ToDBNull(object value, bool emptyStringAsNull)
            => Value.IsNull(value, emptyStringAsNull) ? DBNull.Value : value;

        /// <summary>
        /// Converts null or DBNull to DBNull, and returns other values as is.. Empty or whitespace strings or other values may
        /// also be converted to null depending on the configuration of the provided <see cref="ConvertOptions"/>
        /// </summary>
        /// <param name="value">A value of any type</param> 
        /// <param name="options"></param>
        /// <seealso cref="ConvertOptions.Strings"/>
        /// <seealso cref="StringConvertOptions.AsNullOption"/>
        public static object ToDBNull(object value, ConvertOptions options)
            => Value.IsNull(value, options) ? DBNull.Value : value;
    }
}
