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
         
        public ConvertOptions(IEnumerable<OptionSet> options) => throw null;
        public ConvertOptions(IEnumerable<OptionSet> options, IReadOnlyDictionary<Type, ConverterDelegate> converters) => throw null;

        public BooleanConvertOptions Booleans { get; }
        public EnumConvertOptions Enums { get; }
        public NumberConvertOptions Numbers { get; }
        public StringConvertOptions Strings { get; }
        public ValueTypeConvertOptions ValueTypes { get; }

        public IReadOnlyDictionary<Type, ConverterDelegate> Converters { get; }

        public T GetOptions<T>() where T : OptionSet => throw null;
        public bool TryGetOptions<T>(out T optionSet) where T : OptionSet => throw null;
        public bool HasOptions<T>() where T : OptionSet => throw null;

        public IEnumerable<OptionSet> AllOptions() => throw null;
    }

    public abstract class OptionSet { }
}
