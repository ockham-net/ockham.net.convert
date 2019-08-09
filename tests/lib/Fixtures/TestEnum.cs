using System;

namespace Ockham.Data.Tests.Fixtures
{
    public enum TestShortEnum : short
    {
        Zero = 0,
        One = 1,
        Two = 2,
        FortyTwo = 42,
        FortyNine = 49
    }

    [Flags]
    public enum TestFlags
    {
        None = 0x0,
        Bit1 = 0x1,
        Bit2 = 0x2,
        Bit3 = 0x4
    }
}
