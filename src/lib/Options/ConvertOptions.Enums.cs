using System;

namespace Ockham.Data
{
    /// <summary>
    /// Options for converting to enum types
    /// </summary>
    public class EnumConvertOptions : OptionSet
    {
        /// <summary>
        /// <see cref="EnumConvertOptions"/> that match BCL behavior. Attempting to convert
        /// undefined enum names will throw an exception, but any number *can* be coerced to an enum
        /// </summary>
        public static EnumConvertOptions Default { get; }
            = new EnumConvertOptions(UndefinedValueOption.Throw, UndefinedValueOption.Coerce);

        /// <summary>
        /// Create a new <see cref="EnumConvertOptions"/> instance
        /// </summary>
        /// <param name="undefinedNames">Controls how undefined values are treated</param>
        /// <param name="undefinedValues">Controls how undefined names are treated</param>
        public EnumConvertOptions(UndefinedValueOption undefinedNames, UndefinedValueOption undefinedValues)
        {
            if (undefinedNames == UndefinedValueOption.Coerce) throw new ArgumentException("Cannot coerce undefined enum names");

            this.UndefinedValues = undefinedValues;
            this.UndefinedNames = undefinedNames;

            this.IgnoreUndefinedNames = this.UndefinedNames == UndefinedValueOption.Ignore;
            this.CoerceUndefinedValues = this.UndefinedValues == UndefinedValueOption.Coerce;
            this.IgnoreUndefinedValues = this.UndefinedValues == UndefinedValueOption.Ignore;
        }

        /// <summary>
        /// Controls how undefined values are treated 
        /// </summary>
        public UndefinedValueOption UndefinedValues { get; }

        /// <summary>
        /// Controls how undefined names are treated 
        /// Does not allow <see cref="UndefinedValueOption.Coerce"/>
        /// </summary>
        public UndefinedValueOption UndefinedNames { get; }

        /// <summary>
        /// Ignore undefined values when converting to enums
        /// </summary>
        public bool IgnoreUndefinedValues { get; }

        /// <summary>
        /// Coerce undefined values when converting to enums
        /// </summary>
        public bool CoerceUndefinedValues { get; }

        /// <summary>
        /// Ingore undefined names when converting to enums
        /// </summary>
        public bool IgnoreUndefinedNames { get; }
    }

    /// <summary>
    /// Options for how to handle undefined values during conversion, such as to an enum type
    /// </summary>
    public enum UndefinedValueOption
    {
        /// <summary>
        /// Throw an exception when an undefined value is encountered, such as for an enum
        /// </summary>
        Throw = 1,

        /// <summary>
        /// Ignore and exclude from the output
        /// </summary>
        Ignore = 2,

        /// <summary>
        /// If possible, coerce the source value to the target type, as <see cref="Enum.ToObject(Type, object)"/> does with enums
        /// </summary>
        Coerce = 3
    }
}
