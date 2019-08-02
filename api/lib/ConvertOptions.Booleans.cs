using System.Collections.Generic;

namespace Ockham.Data
{
    // *Immutable* set of bool convert options
    public class BooleanConvertOptions : OptionSet
    {
        public BooleanConvertOptions(IEnumerable<string> trueStrings, IEnumerable<string> falseStrings) => throw null;
        public IReadOnlyCollection<string> TrueStrings { get; }
        public IReadOnlyCollection<string> FalseStrings { get; }
    }
}
