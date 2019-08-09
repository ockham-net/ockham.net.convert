using Ockham.Data.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Xunit;

namespace Ockham.Data.Tests
{
    using static Factories;
    using static ConvertTestRunner;

    // Test that ConvertOptions.Numbers settings have the intended effect
    public partial class ConvertToNumberTests
    {

        [Theory]
        [MemberData(nameof(Decimal42s))]
        public static void AllowSeparator(object value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithNumberOptions(ParseNumericStringFlags.AllowDigitSeparator).Options;

            TestCustomOverloads<int>(value, options, convert =>
            {
                var result = convert();
                Assert.IsType<int>(result);
                Assert.Equal(42, (int)result);
            });
        }

        [Theory]
        [InlineData(HEX_42)]
        [InlineData(HEX_42_ALT)]
        public static void ParseHex(string value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithNumberOptions(ParseNumericStringFlags.HexString).Options;

            TestCustomOverloads<int>(value, options, convert =>
            {
                var result = convert();
                Assert.IsType<int>(result);
                Assert.Equal(42, (int)result);
            });
        }

        [Theory]
        [InlineData(HEX_42)]
        [InlineData(HEX_42_ALT)]
        [InlineData(HEX_42_SEP)]
        public static void ParseHex_WithSep(string value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithNumberOptions(ParseNumericStringFlags.HexString | ParseNumericStringFlags.AllowDigitSeparator).Options;

            TestCustomOverloads<int>(value, options, convert =>
            {
                var result = convert();
                Assert.IsType<int>(result);
                Assert.Equal(42, (int)result);
            });
        }


        [Theory]
        [InlineData(OCT_42)]
        [InlineData(OCT_42_ALT)]
        public static void ParseOct(string value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithNumberOptions(ParseNumericStringFlags.OctalString).Options;

            TestCustomOverloads<int>(value, options, convert =>
            {
                var result = convert();
                Assert.IsType<int>(result);
                Assert.Equal(42, (int)result);
            });
        }

        [Theory]
        [InlineData(OCT_42)]
        [InlineData(OCT_42_ALT)]
        [InlineData(OCT_42_SEP)]
        public static void ParseOct_WithSep(string value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithNumberOptions(ParseNumericStringFlags.OctalString | ParseNumericStringFlags.AllowDigitSeparator).Options;

            TestCustomOverloads<int>(value, options, convert =>
            {
                var result = convert();
                Assert.IsType<int>(result);
                Assert.Equal(42, (int)result);
            });
        }


        [Theory]
        [InlineData(BIN_42)]
        [InlineData(BIN_42_ALT)]
        public static void ParseBin(string value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithNumberOptions(ParseNumericStringFlags.BinaryString).Options;

            TestCustomOverloads<int>(value, options, convert =>
            {
                var result = convert();
                Assert.IsType<int>(result);
                Assert.Equal(42, (int)result);
            });
        }

        [Theory]
        [InlineData(BIN_42)]
        [InlineData(BIN_42_ALT)]
        [InlineData(BIN_42_SEP)]
        public static void ParseBin_WithSep(string value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithNumberOptions(ParseNumericStringFlags.BinaryString | ParseNumericStringFlags.AllowDigitSeparator).Options;

            TestCustomOverloads<int>(value, options, convert =>
            {
                var result = convert();
                Assert.IsType<int>(result);
                Assert.Equal(42, (int)result);
            });
        }

        [Theory]
        [InlineData("1_234")]
        [InlineData("0_042")]
        public static void RejectSeparator_Decimal(string value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithNumberOptions(ParseNumericStringFlags.None).Options;

            TestCustomOverloads<int>(ConvertOverload.To, value, options, convert =>
            {
                Assert.ThrowsAny<SystemException>(() => convert());
            });
        }

        [Theory]
        [InlineData(HEX_42_SEP)]
        public static void RejectSeparator_Hex(string value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithNumberOptions(ParseNumericStringFlags.HexString).Options;

            TestCustomOverloads<int>(ConvertOverload.To, value, options, convert =>
            {
                Assert.ThrowsAny<SystemException>(() => convert());
            });
        }

        [Theory]
        [InlineData(OCT_42_SEP)]
        public static void RejectSeparator_Oct(string value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithNumberOptions(ParseNumericStringFlags.OctalString).Options;

            TestCustomOverloads<int>(ConvertOverload.To, value, options, convert =>
            {
                Assert.ThrowsAny<SystemException>(() => convert());
            });
        }

        [Theory]
        [InlineData(BIN_42_SEP)]
        public static void RejectSeparator_Bin(string value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithNumberOptions(ParseNumericStringFlags.BinaryString).Options;

            TestCustomOverloads<int>(ConvertOverload.To, value, options, convert =>
            {
                Assert.ThrowsAny<SystemException>(() => convert());
            });
        }


    }
}
