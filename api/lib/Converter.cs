using System;
using System.Collections.Generic;

namespace Ockham.Data
{
    public delegate object ConverterDelegate(object value, ConvertOptions options);
    public delegate T ConverterDelegate<T>(object value, ConvertOptions options);

    public partial class Converter
    {
        public static Converter Default { get; } = new Converter(ConvertOptions.Default);

        public Converter(ConvertOptions options) => throw null;
        public Converter(ConvertOptions options, params (Type targetType, ConverterDelegate @delegate)[] converters) => throw null;

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

        public IReadOnlyDictionary<Type, ConverterDelegate> Converters { get; }
        public Converter WithConverter<T>(ConverterDelegate<T> @delegate) => throw null;
        public Converter WithConverter(Type targetType, ConverterDelegate @delegate) => throw null;
        public Converter WithConverters(params (Type targetType, ConverterDelegate @delegate)[] converters) => throw null;
    }

    public partial class Converter
    {
        public bool ToBool(object value) => throw null;
        public DateTime ToDate(object value) => throw null;
        public decimal ToDec(object value) => throw null;
        public double ToDbl(object value) => throw null;
        public Guid ToGuid(object value) => throw null;
        public int ToInt(object value) => throw null;
        public long ToLng(object value) => throw null;
        public string ToStr(object value) => throw null;
        public TimeSpan ToTimeSpan(object value) => throw null;
    } 
}

