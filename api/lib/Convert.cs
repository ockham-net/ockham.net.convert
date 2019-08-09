using System;

namespace Ockham.Data
{
    public static partial class Convert
    {
        public static object Force(object value, Type targetType) => throw null;
        public static T Force<T>(object value) => throw null;
        public static T Force<T>(object value, T defaultValue) => throw null;
        public static T To<T>(object value) => throw null;
        public static T To<T>(object value, ConvertOptions options) => throw null;
        public static object To(object value, Type targetType) => throw null;
        public static object To(object value, Type targetType, ConvertOptions options) => throw null;

        public static object ToNull(object value) => throw null;
        public static object ToNull(object value, bool emptyStringAsNull) => throw null;
        public static object ToNull(object value, ConvertOptions options) => throw null;
        public static object ToDBNull(object value) => throw null;
        public static object ToDBNull(object value, bool emptyStringAsNull) => throw null;
        public static object ToDBNull(object value, ConvertOptions options) => throw null;
    }

    public static partial class Convert
    {
        // These type-specific overloads use the ConvertOptions.Default set of options,
        // which behave as close as possible to the applicable BCL utilities.
        public static bool ToBoolean(object value) => throw null;
        public static DateTime ToDateTime(object value) => throw null;
        public static decimal ToDecimal(object value) => throw null;
        public static double ToDouble(object value) => throw null;
        public static Guid ToGuid(object value) => throw null;
        public static int ToInt32(object value) => throw null;
        public static long ToInt64(object value) => throw null;
        public static string ToString(object value) => throw null;
        public static TimeSpan ToTimeSpan(object value) => throw null;
    }
}
