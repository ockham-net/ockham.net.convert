namespace Ockham.Data
{
    public class BooleanConvertOptions : OptionSet
    {
        public BooleanConvertOptions(IEnumerable<string> trueStrings, IEnumerable<string> falseStrings);
        public static BooleanConvertOptions Default { get; }
        public IImmutableSet<string> FalseStrings { get; }
        public IImmutableSet<string> TrueStrings { get; }
    }
    public class ConvertOptions
    {
        public ConvertOptions(BooleanConvertOptions booleanOptions, EnumConvertOptions enumOptions, NumberConvertOptions numberOptions, StringConvertOptions stringOptions, ValueTypeConvertOptions valueTypeOptions, params OptionSet[] otherOptions);
        public ConvertOptions(params OptionSet[] options);
        public ConvertOptions(IEnumerable<OptionSet> options);
        public BooleanConvertOptions Booleans { get; }
        public static ConvertOptions Default { get; }
        public EnumConvertOptions Enums { get; }
        public NumberConvertOptions Numbers { get; }
        public StringConvertOptions Strings { get; }
        public ValueTypeConvertOptions ValueTypes { get; }
        public IEnumerable<OptionSet> AllOptions();
        public T GetOptions<T>() where T : OptionSet;
        public bool HasOptions<T>() where T : OptionSet;
        public bool TryGetOptions<T>(out T optionSet) where T : OptionSet;
    }
    public class ConvertOptionsBuilder : IEnumerable, IEnumerable<OptionSet>
    {
        public ConvertOptionsBuilder();
        public ConvertOptionsBuilder(IEnumerable<OptionSet> options);
        public ConvertOptionsBuilder(IEnumerable<OptionSet> options, OptionSet newOptions);
        public static ConvertOptionsBuilder Default { get; }
        public static ConvertOptionsBuilder Empty { get; }
        public ConvertOptions Options { get; }
        public static ConvertOptionsBuilder FromConvertOptions(ConvertOptions source);
        public IEnumerator<OptionSet> GetEnumerator();
        public T GetOptions<T>() where T : OptionSet;
        IEnumerator System.Collections.IEnumerable.GetEnumerator();
        public ConvertOptionsBuilder WithBoolOptions(IEnumerable<string> trueStrings, IEnumerable<string> falseStrings);
        public ConvertOptionsBuilder WithEnumOptions(UndefinedValueOption undefinedNames, UndefinedValueOption undefinedValues);
        public ConvertOptionsBuilder WithFalseStrings(params string[] falseStrings);
        public ConvertOptionsBuilder WithNumberOptions(ParseNumericStringFlags parseFlags);
        public ConvertOptionsBuilder WithOptions(OptionSet options);
        public ConvertOptionsBuilder WithStringOptions(StringAsNullOption asNullOption, TrimStringFlags trimFlags);
        public ConvertOptionsBuilder WithTrueStrings(params string[] trueStrings);
        public ConvertOptionsBuilder WithValueTypeOptions(ValueTypeConvertFlags convertFlags);
    }
    public class EnumConvertOptions : OptionSet
    {
        public EnumConvertOptions(UndefinedValueOption undefinedNames, UndefinedValueOption undefinedValues);
        public bool CoerceUndefinedValues { get; }
        public static EnumConvertOptions Default { get; }
        public bool IgnoreUndefinedNames { get; }
        public bool IgnoreUndefinedValues { get; }
        public UndefinedValueOption UndefinedNames { get; }
        public UndefinedValueOption UndefinedValues { get; }
    }
    public class NumberConvertOptions : OptionSet
    {
        public NumberConvertOptions(ParseNumericStringFlags parseFlags);
        public bool AllowDigitSeparator { get; }
        public static NumberConvertOptions Default { get; }
        public bool ParseBinary { get; }
        public ParseNumericStringFlags ParseFlags { get; }
        public bool ParseHex { get; }
        public bool ParseOctal { get; }
    }
    public abstract class OptionSet
    {
        protected OptionSet();
    }
    public enum ParseNumericStringFlags
    {
        AllowDigitSeparator = 16,
        BinaryString = 4,
        HexString = 1,
        None = 0,
        OctalString = 2,
    }
    public enum StringAsNullOption
    {
        EmptyString = 1,
        NullReference = 0,
        Whitespace = 2,
    }
    public class StringConvertOptions : OptionSet
    {
        public StringConvertOptions(StringAsNullOption asNullOption, TrimStringFlags trimFlags);
        public StringAsNullOption AsNullOption { get; }
        public static StringConvertOptions Default { get; }
        public bool EmptyStringAsNull { get; }
        public bool TrimAll { get; }
        public bool TrimEnd { get; }
        public TrimStringFlags TrimFlags { get; }
        public bool TrimNone { get; }
        public bool TrimStart { get; }
        public bool WhitespaceAsNull { get; }
    }
    public enum TrimStringFlags
    {
        None = 0,
        TrimAll = 3,
        TrimEnd = 2,
        TrimStart = 1,
    }
    public enum UndefinedValueOption
    {
        Coerce = 3,
        Ignore = 2,
        Throw = 1,
    }
    public static class Value
    {
        public static bool IsDefault(object value);
        public static bool IsNull(object value);
        public static bool IsNull(object value, ConvertOptions options);
        public static bool IsNull(object value, bool emptyStringAsNull);
        public static bool IsNullOrEmpty(object value);
        public static bool IsNullOrWhitespace(object value);
        public static bool IsNumeric(object value);
        public static bool IsNumeric(object value, ConvertOptions options);
        public static bool IsNumeric(object value, ParseNumericStringFlags parseFlags);
    }
    public enum ValueTypeConvertFlags
    {
        None = 0,
        NullToValueDefault = 1,
    }
    public class ValueTypeConvertOptions : OptionSet
    {
        public ValueTypeConvertOptions(ValueTypeConvertFlags convertFlags);
        public ValueTypeConvertFlags ConvertFlags { get; }
        public static ValueTypeConvertOptions Default { get; }
        public bool NullToValueDefault { get; }
    }
}
