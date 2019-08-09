using Xunit;

namespace Ockham.Data.Tests
{
    public class NumberConvertOptionsTests
    {
        [Fact]
        public void Ctor()
        {
            var opts = new NumberConvertOptions(ParseNumericStringFlags.AllowDigitSeparator);
            Assert.NotNull(opts);
        }

        [Fact]
        public void Default() => TestDefault(NumberConvertOptions.Default);

        [Fact]
        public void DefaultFromBuilder() => TestDefault(ConvertOptionsBuilder.Default.GetOptions<NumberConvertOptions>());

        [Fact]
        public void DefaultFromOptions() => TestDefault(ConvertOptions.Default.Numbers);

        private void TestDefault(NumberConvertOptions opts)
        {
            // Returns a value
            Assert.NotNull(opts);

            // Always the same instance
            Assert.Same(opts, NumberConvertOptions.Default);

            // Expected configuration
            Assert.Equal(ParseNumericStringFlags.None, opts.ParseFlags);
        }

        [Fact]
        public void Flags()
        {
            var opts = new NumberConvertOptions(ParseNumericStringFlags.AllowDigitSeparator | ParseNumericStringFlags.OctalString);
            Assert.Equal(ParseNumericStringFlags.AllowDigitSeparator | ParseNumericStringFlags.OctalString, opts.ParseFlags);

            // Validate memoized flags
            Assert.False(opts.ParseHex);
            Assert.True(opts.ParseOctal);
            Assert.False(opts.ParseBinary);
            Assert.True(opts.AllowDigitSeparator);

            opts = new NumberConvertOptions(ParseNumericStringFlags.HexString | ParseNumericStringFlags.BinaryString);
            Assert.Equal(ParseNumericStringFlags.HexString | ParseNumericStringFlags.BinaryString, opts.ParseFlags);

            Assert.True(opts.ParseHex);
            Assert.False(opts.ParseOctal);
            Assert.True(opts.ParseBinary);
            Assert.False(opts.AllowDigitSeparator);
        }
    }
}
