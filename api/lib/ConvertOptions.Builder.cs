using System;
using System.Collections;
using System.Collections.Generic;

namespace Ockham.Data
{
    public class ConvertOptionsBuilder : IEnumerable<OptionSet>
    {
        // Initialize with empty options
        public static ConvertOptionsBuilder Empty => throw null;

        // Initialize with default options
        public static ConvertOptionsBuilder Default => throw null;

        // Initialize from existing ConvertOptions
        public static ConvertOptionsBuilder FromConvertOptions(ConvertOptions source) => throw null;

        public ConvertOptions Options { get; }

        public T GetOptions<T>() where T : OptionSet => throw null;

        public ConvertOptionsBuilder WithBoolOptions(IEnumerable<string> trueStrings, IEnumerable<string> falseStrings) => throw null;
        public ConvertOptionsBuilder WithTrueStrings(params string[] trueStrings) => throw null;
        public ConvertOptionsBuilder WithFalseStrings(params string[] falseStrings) => throw null;
        public ConvertOptionsBuilder WithEnumOptions(UndefinedValueOption undefinedNames, UndefinedValueOption undefinedValues) => throw null;
        public ConvertOptionsBuilder WithNumberOptions(ParseNumericStringFlags parseFlags) => throw null;
        public ConvertOptionsBuilder WithStringOptions(StringAsNullOption asNullOption, TrimStringFlags trimFlags) => throw null;
        public ConvertOptionsBuilder WithValueTypeOptions(ValueTypeConvertFlags convertFlags) => throw null;
        public ConvertOptionsBuilder WithOptions(params OptionSet[] options) => throw null;

        public ConvertOptionsBuilder WithConverter<T>(ConverterDelegate<T> @delegate) => throw null;
        public ConvertOptionsBuilder WithConverter(Type targetType, ConverterDelegate @delegate) => throw null;
        public ConvertOptionsBuilder WithoutConverters() => throw null;                     // Remove all custom conveters
        public ConvertOptionsBuilder WithoutConverters(params Type[] types) => throw null;  // Remove custom conveters of specific types
        public ConvertOptionsBuilder WithoutConverter<T>() => throw null;                   // Remove custom converter of type T

        public IReadOnlyDictionary<Type, ConverterDelegate> Converters { get; }
        public IEnumerator<OptionSet> GetEnumerator() => throw null;
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        // Constructor is private. Use ConvertOptionsBuilder.Empty
        private ConvertOptionsBuilder() => throw null;
    }

}
