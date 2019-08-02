namespace Ockham.Data
{
    // *Immutable* set of enum convert options
    public class EnumConvertOptions : OptionSet
    {
        // Settings that match BCL behavior
        public static EnumConvertOptions Default { get; }

        public EnumConvertOptions(UndefinedValueOption undefinedNames, UndefinedValueOption undefinedValues) { }
        public UndefinedValueOption UndefinedValues { get; }
        public UndefinedValueOption UndefinedNames { get; }

        public bool IgnoreUndefinedValues { get; }
        public bool CoerceUndefinedValues { get; }
        public bool IgnoreUndefinedNames { get; }
    }

    // Options for how to handle undefined values during conversion, such as to an enum type
    public enum UndefinedValueOption
    {
        Throw = 1,  // Throw an exception when an undefined value is encountered 
        Ignore = 2, // Ignore and exclude from the output 
        Coerce = 3  // If possible, coerce the source value to the target type, as <see cref="Enum.ToObject(Type, object)"/> does with enums
    }
}
