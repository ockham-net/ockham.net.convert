using System;

namespace Ockham.Data
{
    /// <summary>
    /// Settings for converting to and from value types
    /// </summary>
    public class ValueTypeConvertOptions : OptionSet
    {
        /// <summary>
        /// <see cref="ValueTypeConvertOptions"/> that do not convert null to the default value of value types.
        /// Note that <see cref="System.Convert.ToInt32(object)"/> and related methods do coerce null 
        /// to default values (usually <c>0</c>, while <see cref="System.Convert.ChangeType(object, Type)"/> and
        /// language-level casts do not allow this. This <see cref="Default"/> options configuration matches the
        /// more familiar rules where null cannot be cast to a value type. Nulls can still be cast to the 
        /// special <see cref="Nullable{T}"/> struct.
        /// </summary>
        public static ValueTypeConvertOptions Default { get; }
            = new ValueTypeConvertOptions(ValueTypeConvertFlags.None);

        /// <summary>
        /// Create a new <see cref="ValueTypeConvertOptions"/>
        /// </summary>
        public ValueTypeConvertOptions(ValueTypeConvertFlags convertFlags)
        {
            this.ConvertFlags = convertFlags;

            this.NullToValueDefault = ConvertFlags.HasFlag(ValueTypeConvertFlags.NullToValueDefault);
        }

        /// <summary>
        /// Settings for converting to and from value types
        /// </summary>
        public ValueTypeConvertFlags ConvertFlags { get; }

        /// <summary>
        /// Whether to convert null values to the default value of value types
        /// </summary>
        public bool NullToValueDefault { get; }
    }

    /// <summary>
    /// Settings for converting to and from value types
    /// </summary>
    [Flags]
    public enum ValueTypeConvertFlags
    {
        /// <summary>
        /// No relaxed conversion options
        /// </summary>
        None = 0,

        /// <summary>
        /// Convert null values to the default value when the target type is a value type
        /// </summary>
        NullToValueDefault = 0x1
    }
}
