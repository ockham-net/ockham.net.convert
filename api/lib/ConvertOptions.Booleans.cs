using System.Collections.Generic;
using System.Collections.Immutable;

namespace Ockham.Data
{
    // *Immutable* set of bool convert options
    public class BooleanConvertOptions : OptionSet
    {
        public BooleanConvertOptions(IEnumerable<string> trueStrings, IEnumerable<string> falseStrings) => throw null;
        public IImmutableSet<string> TrueStrings { get; }
        public IImmutableSet<string> FalseStrings { get; }
    }
}
