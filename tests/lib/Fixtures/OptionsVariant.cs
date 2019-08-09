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
    }

}
