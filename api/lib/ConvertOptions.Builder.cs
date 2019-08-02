using System.Collections;
using System.Collections.Generic;

namespace Ockham.Data
{
    public class ConvertOptionsBuilder : IEnumerable<OptionSet>
    {
        // Intialize with empty options
        public static ConvertOptionsBuilder Empty => throw null;

        // Initialize from existing ConvertOptions
        public static ConvertOptionsBuilder FromConvertOptions(ConvertOptions source) => throw null;

        public ConvertOptionsBuilder() => throw null;
        public ConvertOptionsBuilder(IEnumerable<OptionSet> options) => throw null;
        public ConvertOptionsBuilder(IEnumerable<OptionSet> options, OptionSet newOptions) => throw null;

        public ConvertOptions Options { get; }

        public T GetOptions<T>() where T : OptionSet => throw null;

        public ConvertOptionsBuilder WithBoolOptions(IEnumerable<string> trueStrings, IEnumerable<string> falseStrings) => throw null;
        public ConvertOptionsBuilder WithTrueStrings(params string[] trueStrings) => throw null;
        public ConvertOptionsBuilder WithFalseStrings(params string[] falseStrings) => throw null;
        public ConvertOptionsBuilder WithEnumOptions(UndefinedValueOption undefinedNames, UndefinedValueOption undefinedValues) => throw null;
        public ConvertOptionsBuilder WithNumberOptions(ParseNumericStringFlags parseFlags) => throw null;
        public ConvertOptionsBuilder WithStringOptions(StringAsNullOption asNullOption, TrimStringFlags trimFlags) => throw null;
        public ConvertOptionsBuilder WithValueTypeOptions(ValueTypeConvertFlags convertFlags) => throw null;
        public ConvertOptionsBuilder WithOptions(OptionSet options) => throw null;

        public IEnumerator<OptionSet> GetEnumerator() => throw null;
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}
