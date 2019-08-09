using Xunit;

namespace Ockham.Data.Tests
{
    // Test that ConvertOptions.Numbers settings have the intended effect
    public partial class ConvertToNumberTests
    {

        [Theory]
        [MemberData(nameof(Decimal42s))]
        public static void AllowSeparator(object value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithNumberOptions(ParseNumericStringFlags.AllowDigitSeparator).Options;

            ConvertAssert.Converts(value, 42, options);
        }

        [Theory]
        [InlineData(HEX_42)]
        [InlineData(HEX_42_ALT)]
        public static void ParseHex(string value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithNumberOptions(ParseNumericStringFlags.HexString).Options;

            ConvertAssert.Converts(value, 42, options);
        }

        [Theory]
        [InlineData(HEX_42)]
        [InlineData(HEX_42_ALT)]
        [InlineData(HEX_42_SEP)]
        public static void ParseHex_WithSep(string value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithNumberOptions(ParseNumericStringFlags.HexString | ParseNumericStringFlags.AllowDigitSeparator).Options;

            ConvertAssert.Converts(value, 42, options);
        }


        [Theory]
        [InlineData(OCT_42)]
        [InlineData(OCT_42_ALT)]
        public static void ParseOct(string value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithNumberOptions(ParseNumericStringFlags.OctalString).Options;

            ConvertAssert.Converts(value, 42, options);
        }

        [Theory]
        [InlineData(OCT_42)]
        [InlineData(OCT_42_ALT)]
        [InlineData(OCT_42_SEP)]
        public static void ParseOct_WithSep(string value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithNumberOptions(ParseNumericStringFlags.OctalString | ParseNumericStringFlags.AllowDigitSeparator).Options;

            ConvertAssert.Converts(value, 42, options);
        }

        [Theory]
        [InlineData(BIN_42)]
        [InlineData(BIN_42_ALT)]
        public static void ParseBin(string value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithNumberOptions(ParseNumericStringFlags.BinaryString).Options;

            ConvertAssert.Converts(value, 42, options);
        }

        [Theory]
        [InlineData(BIN_42)]
        [InlineData(BIN_42_ALT)]
        [InlineData(BIN_42_SEP)]
        public static void ParseBin_WithSep(string value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithNumberOptions(ParseNumericStringFlags.BinaryString | ParseNumericStringFlags.AllowDigitSeparator).Options;

            ConvertAssert.Converts(value, 42, options);
        }

        [Theory]
        [InlineData("1_234")]
        [InlineData("0_042")]
        public static void RejectSeparator_Decimal(string value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithNumberOptions(ParseNumericStringFlags.None).Options;

            ConvertAssert.ConvertFails<int>(value, options);
        }

        [Theory]
        [InlineData(HEX_42_SEP)]
        public static void RejectSeparator_Hex(string value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithNumberOptions(ParseNumericStringFlags.HexString).Options;

            ConvertAssert.ConvertFails<int>(value, options);
        }

        [Theory]
        [InlineData(OCT_42_SEP)]
        public static void RejectSeparator_Oct(string value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithNumberOptions(ParseNumericStringFlags.OctalString).Options;

            ConvertAssert.ConvertFails<int>(value, options);
        }

        [Theory]
        [InlineData(BIN_42_SEP)]
        public static void RejectSeparator_Bin(string value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithNumberOptions(ParseNumericStringFlags.BinaryString).Options;

            ConvertAssert.ConvertFails<int>(value, options);
        }
    }
}
