using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Ockham.Data
{
    /// <summary>
    /// Options for controlling data type conversions executed by Ockham.Data.Convert
    /// This class is immutable. <see cref="ConvertOptionsBuilder"/> may be used to configure an options instance
    /// </summary>
    public partial class ConvertOptions
    {
        private static readonly Lazy<ConvertOptions> _default = new Lazy<ConvertOptions>(() => new ConvertOptions(
            BooleanConvertOptions.Default,
            EnumConvertOptions.Default,
            NumberConvertOptions.Default,
            StringConvertOptions.Default,
            ValueTypeConvertOptions.Default
        ));

        /// <summary>
        /// Default <see cref="ConvertOptions"/> with settings that match Base Class Library behavior for
        /// most conversions available through <see cref="System.Convert"/>, <see cref="Enum.ToObject(Type, object)"/>,
        /// and the various <c>Parse</c> methods of primitive types.
        /// </summary>
        /// <see cref="BooleanConvertOptions.Default"/>
        /// <see cref="EnumConvertOptions.Default"/>
        /// <see cref="NumberConvertOptions.Default"/>
        /// <see cref="StringConvertOptions.Default"/>
        /// <see cref="ValueTypeConvertOptions.Default"/>
        public static ConvertOptions Default => _default.Value;

        /// <summary>
        /// Create a new <see cref="ConvertOptions"/>
        /// </summary>
        /// <param name="booleanOptions">Settings for converting to boolean values</param>
        /// <param name="enumOptions">Settings for converting to enums</param>
        /// <param name="numberOptions">Settings for converting to numbers</param>
        /// <param name="stringOptions">Settings for how to process strings during conversions</param>
        /// <param name="valueTypeOptions">Settings for processing value types</param>
        /// <param name="otherOptions">Any other user-defined <see cref="OptionSet"/>s to store in the <see cref="ConvertOptions"/></param>
        public ConvertOptions(
            BooleanConvertOptions booleanOptions,
            EnumConvertOptions enumOptions,
            NumberConvertOptions numberOptions,
            StringConvertOptions stringOptions,
            ValueTypeConvertOptions valueTypeOptions,
            params OptionSet[] otherOptions
        ) : this((new OptionSet[] { booleanOptions, enumOptions, numberOptions, stringOptions, valueTypeOptions }).Concat(otherOptions)) { }

        /// <summary>
        /// Create a new <see cref="ConvertOptions"/> with all of the provided <see cref="OptionSet"/>s.
        /// <paramref name="options"/> must contain a <see cref="BooleanConvertOptions"/>, 
        /// <see cref="EnumConvertOptions"/>, <see cref="NumberConvertOptions"/>, <see cref="StringConvertOptions"/>,
        /// and <see cref="ValueTypeConvertOptions"/>
        /// </summary>
        public ConvertOptions(params OptionSet[] options) : this((IEnumerable<OptionSet>)options) { }

        /// <summary>
        /// Create a new <see cref="ConvertOptions"/> with all of the provided <see cref="OptionSet"/>s
        /// </summary>
        public ConvertOptions(IEnumerable<OptionSet> options)
        {
            _optionSets = options.ToImmutableDictionary(o => o.GetType());

            // Memoize known option sets
            this.Booleans = this.GetOptions<BooleanConvertOptions>() ?? throw new ArgumentNullException("booleanOptions");
            this.Enums = this.GetOptions<EnumConvertOptions>() ?? throw new ArgumentNullException("enumOptions");
            this.Numbers = this.GetOptions<NumberConvertOptions>() ?? throw new ArgumentNullException("numberOptions");
            this.Strings = this.GetOptions<StringConvertOptions>() ?? throw new ArgumentNullException("stringOptions");
            this.ValueTypes = this.GetOptions<ValueTypeConvertOptions>() ?? throw new ArgumentNullException("valueTypeOptions");

            this.FlattenedOptions = FlattenOptions(this);
            this.WhitespaceToNull = this.Strings.WhitespaceAsNull;
            this.StringToNull = this.WhitespaceToNull || this.Strings.EmptyStringAsNull;
            this.NullToValueDefault = this.ValueTypes.NullToValueDefault;
            this.ParseBaseN = this.Numbers.ParseHex | this.Numbers.ParseOctal | this.Numbers.ParseBinary;
            this.ParseFlage = this.Numbers.ParseFlags;
        }

        /// <summary>
        /// Get the defined option set of type <typeparamref name="T"/>, if any
        /// </summary>
        public T GetOptions<T>() where T : OptionSet => _optionSets.TryGetValue(typeof(T), out OptionSet optionSet) ? (T)optionSet : null;

        /// <summary>
        /// Test whether the current <see cref="ConvertOptions"/> has an <see cref="OptionSet"/> of type <typeparamref name="T"/> defined
        /// </summary>
        public bool HasOptions<T>() where T : OptionSet => _optionSets.ContainsKey(typeof(T));

        /// <summary>
        /// Get all options in the current <see cref="ConvertOptions"/>
        /// </summary>
        public IEnumerable<OptionSet> AllOptions() => _optionSets.Values;

        /// <summary>
        /// Get the <see cref="OptionSet"/> of type <typeparamref name="T"/> if defined.
        /// </summary>
        /// <returns>True if an option set of type <typeparamref name="T"/> was found, false it not</returns>
        public bool TryGetOptions<T>(out T optionSet) where T : OptionSet
        {
            if (_optionSets.TryGetValue(typeof(T), out OptionSet optionSetBase))
            {
                optionSet = (T)optionSetBase;
                return true;
            }
            else
            {
                optionSet = null;
                return false;
            }
        }

        /// <summary>
        /// Settings for converting to boolean values
        /// </summary>
        public BooleanConvertOptions Booleans { get; }

        /// <summary>
        /// Settings for converting to enums
        /// </summary>
        public EnumConvertOptions Enums { get; }

        /// <summary>
        /// Settings for converting to numbers
        /// </summary>
        public NumberConvertOptions Numbers { get; }

        /// <summary>
        /// Settings for how to process strings during conversions
        /// </summary>
        public StringConvertOptions Strings { get; }

        /// <summary>
        /// Settings for processing value types
        /// </summary>
        public ValueTypeConvertOptions ValueTypes { get; }

        private readonly ImmutableDictionary<Type, OptionSet> _optionSets;

        // Memoized internal flags for performance
        internal FlattenedOptions FlattenedOptions { get; }

        internal bool StringToNull { get; }
        internal bool WhitespaceToNull { get; }
        internal bool NullToValueDefault { get; }
        internal bool ParseBaseN { get; }
        internal ParseNumericStringFlags ParseFlage { get; }

        private static FlattenedOptions FlattenOptions(ConvertOptions options)
        {
            var flatOptions = FlattenedOptions.None;

            // Enums
            if (options.Enums.IgnoreUndefinedNames) flatOptions |= FlattenedOptions.IgnoreEnumNames;
            if (options.Enums.IgnoreUndefinedValues) flatOptions |= FlattenedOptions.IgnoreEnumValues;
            if (options.Enums.CoerceUndefinedValues) flatOptions |= FlattenedOptions.CoerceEnumValues;

            // Numbers
            if (options.Numbers.ParseHex) flatOptions |= FlattenedOptions.ParseHex;
            if (options.Numbers.ParseOctal) flatOptions |= FlattenedOptions.ParseOctal;
            if (options.Numbers.ParseBinary) flatOptions |= FlattenedOptions.ParseBinary;
            if (options.Numbers.AllowDigitSeparator) flatOptions |= FlattenedOptions.AllowDigitSeparator;

            // Strings
            if (options.Strings.TrimStart) flatOptions |= FlattenedOptions.TrimStart;
            if (options.Strings.TrimEnd) flatOptions |= FlattenedOptions.TrimEnd;
            if (options.Strings.EmptyStringAsNull) flatOptions |= FlattenedOptions.EmptyStringAsNull;
            if (options.Strings.WhitespaceAsNull) flatOptions |= FlattenedOptions.WhitespaceAsNull;

            // Value types
            if (options.ValueTypes.NullToValueDefault) flatOptions |= FlattenedOptions.NullToValueDefault;

            return flatOptions;
        }
    }

    /// <summary>
    /// Empty base class for categorical option sets
    /// </summary>
    /// <seealso cref="BooleanConvertOptions"/>
    /// <seealso cref="EnumConvertOptions"/>
    /// <seealso cref="NumberConvertOptions" />
    /// <seealso cref="StringConvertOptions"/>
    /// <seealso cref="ValueTypeConvertOptions"/>
    public abstract class OptionSet { }

    [Flags]
    internal enum FlattenedOptions
    {
        None = 0x0,

        // Enums
        IgnoreEnumNames = 0x1,
        IgnoreEnumValues = 0x2,
        CoerceEnumValues = 0x4,

        // Numbers
        ParseHex = 0x10,
        ParseOctal = 0x20,
        ParseBinary = 0x40,
        AllowDigitSeparator = 0x80,

        // Strings
        TrimStart = 0x1000,
        TrimEnd = 0x2000,
        TrimAll = 0x3000,

        EmptyStringAsNull = 0x10000,
        WhitespaceAsNull = 0x20000,

        // Value types
        NullToValueDefault = 0x100000
    }
}
