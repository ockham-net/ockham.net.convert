using System;

namespace Ockham.Data
{
    /// <summary>
    /// Settings for converting to numbers
    /// </summary>
    public class NumberConvertOptions : OptionSet
    {
        /// <summary>
        /// <see cref="NumberConvertOptions"/> which matches BCL behavior. Equivalent to <see cref="ParseNumericStringFlags.None"/>
        /// </summary>
        public static NumberConvertOptions Default { get; }
            = new NumberConvertOptions(ParseNumericStringFlags.None);

        /// <summary>
        /// Create a new <see cref="NumberConvertOptions"/> based on the provided flags
        /// </summary>
        public NumberConvertOptions(ParseNumericStringFlags parseFlags)
        {
            this.ParseFlags = parseFlags;

            this.ParseHex = ParseFlags.HasFlag(ParseNumericStringFlags.HexString);
            this.ParseOctal = ParseFlags.HasFlag(ParseNumericStringFlags.OctalString);
            this.ParseBinary = ParseFlags.HasFlag(ParseNumericStringFlags.BinaryString);
            this.AllowDigitSeparator = ParseFlags.HasFlag(ParseNumericStringFlags.AllowDigitSeparator);
        }

        /// <summary>
        /// Flags controlling how to parse numeric strings 
        /// </summary>
        public ParseNumericStringFlags ParseFlags { get; }

        /// <summary>
        /// Recognize and parse 0x-prefixed hex strings
        /// </summary>
        public bool ParseHex { get; }

        /// <summary>
        /// Recognized and parse 0o-prefixed octal strings
        /// </summary>
        public bool ParseOctal { get; }

        /// <summary>
        /// Recognize and parse 0b-prefixed binary strings
        /// </summary>
        public bool ParseBinary { get; }

        /// <summary>
        /// Allow underscore _ digit separator
        /// </summary>
        public bool AllowDigitSeparator { get; }
    }

    /// <summary>
    /// Flags controlling how to parse numeric strings
    /// </summary>
    [Flags]
    public enum ParseNumericStringFlags
    {
        /// <summary>
        /// No non-standard numeric strings recognized
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Recognize and parse 0x-prefixed hex strings
        /// </summary>
        HexString = 0x1,

        /// <summary>
        /// Recognized and parse 0o-prefixed octal strings
        /// </summary>
        OctalString = 0x2,

        /// <summary>
        /// Recognize and parse 0b-prefixed binary strings
        /// </summary>
        BinaryString = 0x4,

        /// <summary>
        /// Allow underscore _ digit separator
        /// </summary>
        AllowDigitSeparator = 0x10
    }
}
