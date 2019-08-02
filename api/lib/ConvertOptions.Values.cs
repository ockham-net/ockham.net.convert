using System;

namespace Ockham.Data
{

    public class ValueTypeOptions : OptionSet
    {
        // Settings that match BCL behavior
        public static ValueTypeOptions Default { get; }

        public ValueTypeOptions(ValueTypeConvertFlags convertFlags) => throw null;
        public ValueTypeConvertFlags ConvertFlags { get; }

        public bool NullToValueDefault => ConvertFlags.HasFlag(ValueTypeConvertFlags.NullToValueDefault);
    }

    [Flags]
    public enum ValueTypeConvertFlags
    {
        None = 0,
        NullToValueDefault = 0x1  // Convert null values to the default value when the target type is a value type
    }
}
