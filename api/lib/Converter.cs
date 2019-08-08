using System;

namespace Ockham.Data
{
    public delegate object ConverterDelegate(object value, ConvertOptions options);
    public delegate T ConverterDelegate<T>(object value, ConvertOptions options);

    public partial class Converter
    {
        public static Converter Default { get; } = new Converter(ConvertOptions.Default);

        public Converter(ConvertOptions options) => throw null;

        public ConvertOptions Options { get; }

        public object Force(object value, Type targetType) => throw null;
        public T Force<T>(object value) => throw null;
        public T Force<T>(object value, T defaultValue) => throw null;
        public T To<T>(object value) => throw null;
        public object To(object value, Type targetType) => throw null;

        public bool IsNumeric(object value) => throw null;
        public bool IsNull(object value) => throw null;
        public object ToNull(object value) => throw null;
        public object ToDBNull(object value) => throw null;
    }

    public partial class Converter
    {
        public bool ToBoolean(object value) => throw null;
        public DateTime ToDateTime(object value) => throw null;
        public decimal ToDecimal(object value) => throw null;
        public double ToDouble(object value) => throw null;
        public Guid ToGuid(object value) => throw null;
        public int ToInt32(object value) => throw null;
        public long ToInt64(object value) => throw null;
        public string ToString(object value) => throw null;
        public TimeSpan ToTimeSpan(object value) => throw null;
    } 
}

