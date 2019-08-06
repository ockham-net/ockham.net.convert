using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Ockham.Data
{
    /// <summary>
    /// Settings for conversion to boolean values. This class is immutable.
    /// </summary>
    public class BooleanConvertOptions : OptionSet
    {
        /// <summary>
        /// A <see cref="BooleanConvertOptions"/> which only converts 'true' to true and 'false' to false
        /// </summary>
        public static BooleanConvertOptions Default { get; }
            = new BooleanConvertOptions(new[] { bool.TrueString }, new[] { bool.FalseString });

        /// <summary>
        /// Create a new <see cref="BooleanConvertOptions"/> 
        /// </summary>
        /// <param name="trueStrings">Strings to convert to true</param>
        /// <param name="falseStrings">Strings to convert to false</param>
        public BooleanConvertOptions(IEnumerable<string> trueStrings, IEnumerable<string> falseStrings)
        {
            this.TrueStrings = trueStrings.ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);
            this.FalseStrings = falseStrings.ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Strings to convert to true. Comparison is always case-insensitive
        /// </summary>
        public IImmutableSet<string> TrueStrings { get; }

        /// <summary>
        /// Strings to convert to false. Comparison is always case-insensitive
        /// </summary>
        public IImmutableSet<string> FalseStrings { get; }
    }
}
