using System;
using System.Collections.Generic;

namespace Ockham.Data
{
    // *Immutable* set of convert options
    public class ConvertOptions
    { 
        // Default: Options that cause Ockham.Convert to behave as close as possible to the corresponding BCL functions.
        // For example, Enum.Parse(type, string) throws an exception for an undefined enum *name*, but 
        // Enum.ToObject(type, int) happily accepts an undefined enum *value*. And compiler mechanics 
        // generally do NOT allow converting null to a value type, but do allow converting null to a
        // Nullable<T> struct. Thus Convert.To<int>(null) should throw, but Convert.To<int?>(null) should be ok.
        public static ConvertOptions Default { get; } 

        public ConvertOptions(
            StringConvertOptions stringOptions,
            EnumConvertOptions enumOptions,
            BooleanConvertOptions booleanOptions,
            ValueTypeOptions valueTypeOptions
        )
        { }

        public StringConvertOptions Strings { get; }
        public EnumConvertOptions Enums { get; }
        public BooleanConvertOptions Booleans { get; }
        public ValueTypeOptions ValueTypes { get; }
    }

    public class ConvertOptionsBuilder<T> where T : ConvertOptions
    {
        public T Options => throw null;

        public ConvertOptionsBuilder<T> WithStringOptions(EmptyStringConvertOptions emptyStringOptions, bool allowHex = false, bool trim = false) => throw null;
        public ConvertOptionsBuilder<T> WithBoolOptions(IEnumerable<string> trueStrings, IEnumerable<string> falseStrings) => throw null;
        public ConvertOptionsBuilder<T> WithEnumOptions(UndefinedValueOption undefinedNames, UndefinedValueOption undefinedValues) => throw null;
        public ConvertOptionsBuilder<T> WithValueTypeOptions(ValueTypeOptions valueTypeOptions) => throw null;
        public ConvertOptionsBuilder<T> WithTrueStrings(params string[] trueStrings) => throw null;
        public ConvertOptionsBuilder<T> WithFalseStrings(params string[] falseStrings) => throw null;
    }

    public class ConvertOptionsBuilder : ConvertOptionsBuilder<ConvertOptions>
    {
        public static ConvertOptionsBuilder<T> Create<T>() where T : ConvertOptions => throw null;
        public static ConvertOptionsBuilder Create() => throw null;
    }

    // *Immutable* set of string convert options
    public class StringConvertOptions
    {
        public StringConvertOptions(EmptyStringConvertOptions emptyStringOptions, bool allowHex, bool trim) { }
        public EmptyStringConvertOptions EmptyStrings { get; }
        public bool Allow0xHex { get; }
        public bool Trim { get; } // Trim strings before converting to other types
    }

    [Flags]
    public enum EmptyStringConvertOptions
    {
        None = 0,
        EmptyStringAsNull = 0x1,
        WhitespaceAsNull = 0x2
    }

    // *Immutable* set of enum convert options
    public class EnumConvertOptions
    {
        public EnumConvertOptions(UndefinedValueOption undefinedNames, UndefinedValueOption undefinedValues) { }
        public UndefinedValueOption UndefinedValues { get; }
        public UndefinedValueOption UndefinedNames { get; }
    }

    // Options for how to handle undefined values during conversion, such as to an enum type
    public enum UndefinedValueOption
    {
        Throw = 1,  // Throw an exception when an undefined value is encountered 
        Ignore = 2, // Ignore and exclude from the output 
        Coerce = 3  // If possible, coerce the source value to the target type, as <see cref="Enum.ToObject(Type, object)"/> does with enums
    }

    // *Immutable* set of bool convert options
    public class BooleanConvertOptions
    {
        public ICollection<string> TrueStrings { get; }
        public ICollection<string> FalseStrings { get; }
    }

    [Flags]
    public enum ValueTypeOptions
    {
        None = 0,
        NullToValueDefault = 0x1  // Convert null values to the default value when the target type is a value type
    }
}
