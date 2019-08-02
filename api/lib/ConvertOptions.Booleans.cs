using System.Collections.Generic;
using System.Collections.Immutable;

namespace Ockham.Data
{
    // *Immutable* set of bool convert options
    public class BooleanConvertOptions : OptionSet
    {
        // Settings that match BCL behavior
        public static BooleanConvertOptions Default { get; }

        public BooleanConvertOptions(IEnumerable<string> trueStrings, IEnumerable<string> falseStrings) => throw null;
        public IImmutableSet<string> TrueStrings { get; }
        public IImmutableSet<string> FalseStrings { get; }
    }
}
