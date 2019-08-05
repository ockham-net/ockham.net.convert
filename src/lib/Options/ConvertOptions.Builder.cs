using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Ockham.Data
{
    /// <summary>
    /// Utility class for building up <see cref="ConvertOptions"/> 
    /// </summary>
    public class ConvertOptionsBuilder : IEnumerable<OptionSet>
    {

        private static readonly Lazy<ConvertOptionsBuilder> _default = new Lazy<ConvertOptionsBuilder>(() =>
            new ConvertOptionsBuilder(new OptionSet[] {
                BooleanConvertOptions.Default,
                EnumConvertOptions.Default,
                NumberConvertOptions.Default,
                StringConvertOptions.Default,
                ValueTypeConvertOptions.Default
            })
        );

        /// <summary>
        /// A <see cref="ConvertOptionsBuilder"/> initiliazed with the default settings for all options
        /// </summary>
        /// <see cref="ConvertOptions.Default"/>
        /// <see cref="BooleanConvertOptions.Default"/>
        /// <see cref="EnumConvertOptions.Default"/>
        /// <see cref="NumberConvertOptions.Default"/>
        /// <see cref="StringConvertOptions.Default"/>
        /// <see cref="ValueTypeConvertOptions.Default"/>
        public static ConvertOptionsBuilder Default => _default.Value;

        /// <summary>
        /// A <see cref="ConvertOptionsBuilder"/> with no options set
        /// </summary>
        public static ConvertOptionsBuilder Empty { get; } = new ConvertOptionsBuilder();  // Note ConvertOptionsBuilder is immutable, so we can always return the same instance here

        /// <summary>
        /// Intialize a <see cref="ConvertOptionsBuilder"/> with the options from an existing <see cref="ConvertOptions"/> instance
        /// </summary>
        public static ConvertOptionsBuilder FromConvertOptions(ConvertOptions source) => new ConvertOptionsBuilder(source.AllOptions());


        private readonly ImmutableDictionary<Type, OptionSet> _optionSets;

        /// <summary>
        /// Create a new <see cref="ConvertOptionsBuilder"/> with no options set
        /// </summary>
        public ConvertOptionsBuilder() : this(Enumerable.Empty<OptionSet>()) { }

        /// <summary>
        /// Create a new <see cref="ConvertOptionsBuilder"/> initialized with the provided <paramref name="options"/>
        /// </summary>
        public ConvertOptionsBuilder(IEnumerable<OptionSet> options) => _optionSets = options.ToImmutableDictionary(o => o.GetType());

        /// <summary>
        /// Create a new <see cref="ConvertOptionsBuilder"/> initialized with the provided <paramref name="options"/>
        /// and <paramref name="newOptions"/>. <paramref name="newOptions"/> will replace any existing options in <paramref name="options"/>
        /// of the same type
        /// </summary>
        public ConvertOptionsBuilder(IEnumerable<OptionSet> options, OptionSet newOptions)
            => _optionSets = options.ToImmutableDictionary(o => o.GetType()).SetItem(newOptions.GetType(), newOptions);

        /// <summary>
        /// Get an <see cref="ConvertOptions"/> instance will the options from the current builder
        /// </summary>
        public ConvertOptions Options => new ConvertOptions(this);

        /// <summary>
        /// Retrieve a specific set of options by type
        /// </summary> 
        /// <example>
        /// // Get the current BooleanConvertOptions, if any, from builder
        /// var boolOptions = builder.GetOptions&lt;BooleanConvertOptions&gt;()
        /// </example> 
        public T GetOptions<T>() where T : OptionSet => _optionSets.TryGetValue(typeof(T), out OptionSet optionSet) ? (T)optionSet : null;

        /// <summary>
        /// Set or update the <see cref="BooleanConvertOptions"/> with the provided parameters
        /// </summary>
        /// <param name="trueStrings">Strings to convert to true</param>
        /// <param name="falseStrings">Strings to convert to false</param>
        /// <returns>A new <see cref="ConvertOptionsBuilder"/> with updated settings</returns>
        /// <seealso cref="BooleanConvertOptions"/>
        public ConvertOptionsBuilder WithBoolOptions(IEnumerable<string> trueStrings, IEnumerable<string> falseStrings)
            => new ConvertOptionsBuilder(this, new BooleanConvertOptions(trueStrings, falseStrings));


        /// <summary>
        /// Set or update the <see cref="BooleanConvertOptions"/> with the provided <paramref name="trueStrings"/>
        /// </summary>
        /// <param name="trueStrings">Strings to convert to true</param>
        /// <returns>A new <see cref="ConvertOptionsBuilder"/> with updated settings</returns>
        /// <seealso cref="BooleanConvertOptions"/>
        public ConvertOptionsBuilder WithTrueStrings(params string[] trueStrings)
        {
            var boolOptions = GetOptions<BooleanConvertOptions>();
            return new ConvertOptionsBuilder(this, new BooleanConvertOptions(
                trueStrings,
                boolOptions?.FalseStrings ?? Enumerable.Empty<string>()
            ));
        }

        /// <summary>
        /// Set or update the <see cref="BooleanConvertOptions"/> with the provided <paramref name="falseStrings"/>
        /// </summary>
        /// <param name="falseStrings">Strings to convert to false</param>
        /// <returns>A new <see cref="ConvertOptionsBuilder"/> with updated settings</returns>
        /// <seealso cref="BooleanConvertOptions"/>
        public ConvertOptionsBuilder WithFalseStrings(params string[] falseStrings)
        {
            var boolOptions = GetOptions<BooleanConvertOptions>();
            return new ConvertOptionsBuilder(this, new BooleanConvertOptions(
                boolOptions?.TrueStrings ?? Enumerable.Empty<string>(),
                falseStrings
            ));
        }

        /// <summary>
        /// Set or update the <see cref="EnumConvertOptions"/> with the provider parameters
        /// </summary>
        /// <param name="undefinedNames"></param>
        /// <param name="undefinedValues"></param>
        /// <returns>A new <see cref="ConvertOptionsBuilder"/> with updated settings</returns>
        /// <seealso cref="EnumConvertOptions"/>
        public ConvertOptionsBuilder WithEnumOptions(UndefinedValueOption undefinedNames, UndefinedValueOption undefinedValues)
            => new ConvertOptionsBuilder(this, new EnumConvertOptions(undefinedNames, undefinedValues));

        /// <summary>
        /// Set or update the <see cref="NumberConvertOptions"/> with the provided parameters
        /// </summary>
        /// <param name="parseFlags">Flags controlling how to parse numeric strings</param>
        /// <returns>A new <see cref="ConvertOptionsBuilder"/> with updated settings</returns>
        /// <see cref="NumberConvertOptions"/>
        /// <see cref="ParseNumericStringFlags"/>
        public ConvertOptionsBuilder WithNumberOptions(ParseNumericStringFlags parseFlags)
            => new ConvertOptionsBuilder(this, new NumberConvertOptions(parseFlags));

        /// <summary>
        /// Set or update the <see cref="StringConvertOptions"/> with the provided parameters
        /// </summary>
        /// <param name="asNullOption">Determines which strings to treat as null</param>
        /// <param name="trimFlags">Flags for indicating whether strings should be trimmed</param>
        /// <returns>A new <see cref="ConvertOptionsBuilder"/> with updated settings</returns>
        /// <see cref="StringConvertOptions"/>
        public ConvertOptionsBuilder WithStringOptions(StringAsNullOption asNullOption, TrimStringFlags trimFlags)
            => new ConvertOptionsBuilder(this, new StringConvertOptions(asNullOption, trimFlags));

        /// <summary>
        /// Set or update the <see cref="ValueTypeConvertOptions"/> with the provided parameters
        /// </summary>
        /// <param name="convertFlags">Settings for converting to and from value types</param>
        /// <returns>A new <see cref="ConvertOptionsBuilder"/> with updated settings</returns>
        /// <see cref="ValueTypeConvertOptions"/>
        /// <see cref="ValueTypeConvertFlags"/>
        public ConvertOptionsBuilder WithValueTypeOptions(ValueTypeConvertFlags convertFlags)
            => new ConvertOptionsBuilder(this, new ValueTypeConvertOptions(convertFlags));

        /// <summary>
        /// Add or replace an <see cref="OptionSet"/> on this <see cref="ConvertOptionsBuilder"/>
        /// </summary>
        /// <param name="options">Any <see cref="OptionSet"/></param>
        /// <returns>A new <see cref="ConvertOptionsBuilder"/> with updated settings</returns>
        public ConvertOptionsBuilder WithOptions(OptionSet options)
            => new ConvertOptionsBuilder(this, options);

        /// <summary>
        /// Enumerate the <see cref="OptionSet"/>s in this <see cref="ConvertOptionsBuilder"/>
        /// </summary>
        public IEnumerator<OptionSet> GetEnumerator() => _optionSets.Values.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

}
