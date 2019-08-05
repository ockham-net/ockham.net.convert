using System;

namespace Ockham.Data
{
    // *Immutable* set of bool convert options
    public class NumberConvertOptions : OptionSet
    {
        // Settings that match BCL behavior
        public static NumberConvertOptions Default { get; }

        public NumberConvertOptions(ParseNumericStringFlags parseFlags) => throw null;

        public ParseNumericStringFlags ParseFlags { get; }

        public bool ParseHex => ParseFlags.HasFlag(ParseNumericStringFlags.HexString);
        public bool ParseOctal => ParseFlags.HasFlag(ParseNumericStringFlags.OctalString);
        public bool ParseBinary => ParseFlags.HasFlag(ParseNumericStringFlags.BinaryString);
        public bool AllowDigitSeparator => ParseFlags.HasFlag(ParseNumericStringFlags.AllowDigitSeparator);
    }

    [Flags]
    public enum ParseNumericStringFlags
    {
        None = 0x0,
        HexString = 0x1,
        OctalString = 0x2,
        BinaryString = 0x4,
        AllowDigitSeparator = 0x10
    }
}
