namespace Ockham.Data.Tests.Fixtures
{
    internal class OptionsVariant
    {
        public static ConvertOptions ParseBaseN { get; }
            = ConvertOptionsBuilder.Default
                .WithNumberOptions(ParseNumericStringFlags.HexString | ParseNumericStringFlags.OctalString | ParseNumericStringFlags.BinaryString).Options;

        public static ConvertOptions EmptyStringAsNull { get; }
            = ConvertOptionsBuilder.Default
                .WithStringOptions(StringAsNullOption.EmptyString, TrimStringFlags.TrimAll).Options;

        public static ConvertOptions WhitespaceAsNull { get; }
            = ConvertOptionsBuilder.Default
                .WithStringOptions(StringAsNullOption.Whitespace, TrimStringFlags.TrimAll).Options;

        public static ConvertOptions NullToValueDefault { get; }
            = ConvertOptionsBuilder.Default
                .WithValueTypeOptions(ValueTypeConvertFlags.NullToValueDefault).Options;

        public static ConvertOptions AllowSeparator { get; }
            = ConvertOptionsBuilder.Default
                .WithNumberOptions(ParseNumericStringFlags.AllowDigitSeparator).Options;

        public static ConvertOptions ParseHex { get; }
            = ConvertOptionsBuilder.Default
                .WithNumberOptions(ParseNumericStringFlags.HexString).Options;

        public static ConvertOptions ParseOctal { get; }
            = ConvertOptionsBuilder.Default
                .WithNumberOptions(ParseNumericStringFlags.OctalString).Options;

        public static ConvertOptions ParseBinary { get; }
            = ConvertOptionsBuilder.Default
                .WithNumberOptions(ParseNumericStringFlags.BinaryString).Options;

    }

}
