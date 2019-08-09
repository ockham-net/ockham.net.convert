using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Ockham.Data.Tests
{
    public partial class ValueTests
    {
        public static string[] _NumericStrings = {
            "1",
            "2323091320",
            "2e2",
            "2.09823e+2",
            "2.23098e02",
            "2e+002",
            "3409832.3e-9",
            "934.233e-06",
            "00000.0000342908230420",
        };

        public static string[] _NumericSeparatedStrings = {
            "2323_091_320",
            "2.09_823e+2",
            "2.230_98e02",
            "3409_832.3e-9",
            "9_34.2_33e-06",
            "0_0000.000_034_29082__30420",
        };

        public static string[] _HexStrings =
        {
            "0x23af934",
            "0XFFFFFA0",
            "0x0_0_0_0_0"
        };

        public static string[] _OctalStrings =
        {
            "0o3247243",
            "0O777",
            "0o0_015_2"
        };

        public static string[] _BinStrings = {
            "0b100110110101",
            "0b0",
            "0B01_10011_110101"
        };

        public static string[] _FakeBaseNStrings =
        {
            "0xASDG349023",
            "0O77708",
            "0B0010110201"
        };


        public static IEnumerable<object[]> HexStrings = _HexStrings.AsObjectArray();

        public static IEnumerable<object[]> OctalStrings = _OctalStrings.AsObjectArray();

        public static IEnumerable<object[]> BinaryStrings = _BinStrings.AsObjectArray();

        public static IEnumerable<object[]> FakeBaseNStrings = _FakeBaseNStrings.AsObjectArray();

        public static IEnumerable<object[]> NumericStrings = _NumericStrings.AsObjectArray();

        public static IEnumerable<object[]> NumericWithSeparator = _NumericStrings.AsObjectArray();

        public static IEnumerable<object[]> Numbers = (
            new object[] {
                (byte)42,
                (sbyte)-42,
                (short)-3432,
                (ushort)394,
                (int)-30982094,
                (uint)598230948,
                (long)-3049820_94843098234,
                (long)3483204983240982340,
                (decimal)324984.234098234098,
                (float)934.233e-06,
                (double)0_0000.000_034_29082__30420
            }).AsObjectArray();

        public static IEnumerable<object[]> NotNumeric = (
            new object[] {
                "Hello",
                new object(),
                ConvertOptions.Default,
                null,
                DBNull.Value
            }).AsObjectArray();

        public static IEnumerable<object[]> NumericValues = NumericStrings.Concat(Numbers);

        public static IEnumerable<object[]> PaddedStrings = _NumericStrings.Select(s => " \t  " + s + " \r\n ").AsObjectArray();

        public static IEnumerable<object[]> SeparatedStrings = _NumericSeparatedStrings.AsObjectArray();

        private static ConvertOptions AllowSepOptions = ConvertOptionsBuilder.Default.WithNumberOptions(ParseNumericStringFlags.AllowDigitSeparator).Options;

        private static ConvertOptions ParseAllOptions =
            ConvertOptionsBuilder.Default
            .WithNumberOptions(ParseNumericStringFlags.AllowDigitSeparator | ParseNumericStringFlags.HexString | ParseNumericStringFlags.OctalString | ParseNumericStringFlags.BinaryString)
            .Options;

        [Theory]
        [MemberData(nameof(NumericStrings))]
        public void IsNumeric_String(object value)
        {
            Assert.True(Value.IsNumeric(value));
        }

        [Theory]
        [MemberData(nameof(PaddedStrings))]
        public void IsNumeric_PaddedString(object value)
        {
            Assert.True(Value.IsNumeric(value));
        }

        [Theory]
        [MemberData(nameof(SeparatedStrings))]
        public void IsNumeric_RejectSeparatedString(object value)
        {
            Assert.False(Value.IsNumeric(value));
        }

        [Theory]
        [MemberData(nameof(SeparatedStrings))]
        public void IsNumeric_AllowSeparatedString_Flags(object value)
        {
            Assert.True(Value.IsNumeric(value, ParseNumericStringFlags.AllowDigitSeparator));
        }

        [Theory]
        [MemberData(nameof(SeparatedStrings))]
        public void IsNumeric_AllowSeparatedString_Options(object value)
        {
            Assert.True(Value.IsNumeric(value, AllowSepOptions));
        }

        [Theory]
        [MemberData(nameof(Numbers))]
        public void IsNumeric_Numbers(object value)
        {
            Assert.True(Value.IsNumeric(value));
        }

        [Fact]
        public void IsNumeric_Enums()
        {
            Assert.True(Value.IsNumeric(BindingFlags.Public));
            Assert.True(Value.IsNumeric(StringAsNullOption.Whitespace));
        }

        [Theory]
        [MemberData(nameof(NotNumeric))]
        public void IsNumeric_False(object value)
        {
            Assert.False(Value.IsNumeric(value));
        }

        [Theory]
        [MemberData(nameof(HexStrings))]
        public void IsNumeric_Hex(string value)
        {
            Assert.True(Value.IsNumeric(value, ParseNumericStringFlags.HexString | ParseNumericStringFlags.AllowDigitSeparator));
            Assert.True(Value.IsNumeric(value, ParseAllOptions));
        }

        [Theory]
        [MemberData(nameof(OctalStrings))]
        public void IsNumeric_Octal(string value)
        {
            Assert.True(Value.IsNumeric(value, ParseNumericStringFlags.OctalString | ParseNumericStringFlags.AllowDigitSeparator));
            Assert.True(Value.IsNumeric(value, ParseAllOptions));
        }

        [Theory]
        [MemberData(nameof(BinaryStrings))]
        public void IsNumeric_Binary(string value)
        {
            Assert.True(Value.IsNumeric(value, ParseNumericStringFlags.BinaryString | ParseNumericStringFlags.AllowDigitSeparator));
            Assert.True(Value.IsNumeric(value, ParseAllOptions));
        }

        [Theory]
        [MemberData(nameof(FakeBaseNStrings))]
        public void IsNumeric_BaseNFail(string value)
        {
            Assert.False(Value.IsNumeric(value, (ParseNumericStringFlags)(-1)));
            Assert.False(Value.IsNumeric(value, ParseAllOptions));
        }
    }
}
