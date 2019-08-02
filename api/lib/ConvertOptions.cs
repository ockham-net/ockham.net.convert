using System.Collections.Generic;
using System.Linq;

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
            BooleanConvertOptions booleanOptions,
            EnumConvertOptions enumOptions,
            NumberConvertOptions numberOptions,
            StringConvertOptions stringOptions,
            ValueTypeOptions valueTypeOptions,
            params OptionSet[] otherOptions
        ) : this((new OptionSet[] { booleanOptions, enumOptions, numberOptions, stringOptions, valueTypeOptions }).Concat(otherOptions)) { }

        public ConvertOptions(params OptionSet[] options) : this((IEnumerable<OptionSet>)options) { }
        public ConvertOptions(IEnumerable<OptionSet> options) { }

        public BooleanConvertOptions Booleans { get; }
        public EnumConvertOptions Enums { get; }
        public NumberConvertOptions Numbers { get; }
        public StringConvertOptions Strings { get; }
        public ValueTypeOptions ValueTypes { get; }

        public T GetOptions<T>() where T : OptionSet => throw null;

        public IEnumerable<OptionSet> AllOptions() => throw null;
    }

    public abstract class OptionSet { }
}
