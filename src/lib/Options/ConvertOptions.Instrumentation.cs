using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Ockham.Data
{
    public partial class ConvertOptions
    {
#if INSTRUMENT
        // Internal stuff used only for testing purposes
        internal bool Instrument;

        internal long CountPastNullRef;

        internal long CountPastSameType;

        internal long CountPastEmptyValue;

        internal long CountPastCustomConverter;
#endif

    }
}
