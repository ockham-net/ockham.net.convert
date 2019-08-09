using Ockham.Data.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ockham.Data.Tests
{
    using static Factories;

    // Test that ConvertOptions.Numbers settings have the intended effect
    public partial class ConvertToNumberTests
    {
        private const sbyte SBYTE_42 = 42;
        private const short INT16_42 = 42;
        private const int INT32_42 = 42;
        private const long INT64_42 = 42;

        private const byte BYTE_42 = 42;
        private const ushort UINT16_42 = 42;
        private const uint UINT32_42 = 42;
        private const ulong UINT64_42 = 42;

        private const float FLOAT_42 = 42;
        private const double DOUBLE_42 = 42;
        private const decimal DEC_42 = 42;

        private const TestShortEnum ENUM_42 = TestShortEnum.FortyTwo;

        private const string STR_INT_42 = "42";
        private const string STR_INT_42_INT = "0042";
        private const string STR_INT_42_SEP = "0_042";
        private const string STR_INT_42_EXP = "4.2e+01";

        private const string HEX_42 = "0x2A";
        private const string HEX_42_ALT = "0X2a";
        private const string HEX_42_SEP = "0x00_2A";

        private const string OCT_42 = "0o52";
        private const string OCT_42_ALT = "0O52";
        private const string OCT_42_SEP = "0o00_52";

        private const string BIN_42 = "0b101010";
        private const string BIN_42_ALT = "0B00101010";
        private const string BIN_42_SEP = "0b0010_1010";

        public static readonly IEnumerable<object[]> Number42s = Values(
                SBYTE_42,
                INT16_42,
                INT32_42,
                INT64_42,
                BYTE_42,
                UINT16_42,
                UINT32_42,
                UINT64_42,
                FLOAT_42,
                DOUBLE_42,
                DEC_42,
                ENUM_42
            );

        public static readonly IEnumerable<object[]> String42s = Values(
                STR_INT_42,
                STR_INT_42_INT,
                STR_INT_42_SEP,
                STR_INT_42_EXP
            );

        private static readonly Type[] NumberTypes = {
            typeof(sbyte),
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(byte),
            typeof(ushort),
            typeof(uint),
            typeof(ulong),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(TestShortEnum)
        };

        public static readonly IEnumerable<object[]> String42s_Strict = Values(STR_INT_42, STR_INT_42_INT, STR_INT_42_EXP);

        public static IEnumerable<object[]> Decimal42s => Number42s.Concat(String42s);

        public static IEnumerable<object[]> Decimal42s_Strict => Number42s.Concat(String42s_Strict);

        public static readonly ConvertOptions ParseAllOptions = ConvertOptions.Default.GetBuilder()
                .WithNumberOptions(
                    ParseNumericStringFlags.HexString
                  | ParseNumericStringFlags.OctalString
                  | ParseNumericStringFlags.BinaryString
                  | ParseNumericStringFlags.AllowDigitSeparator
                ).Options;


        public static IEnumerable<object[]> Decimal42s_x_NumberTypes()
        {
            object[] numbers = Set(
                SBYTE_42,
                INT16_42,
                INT32_42,
                INT64_42,
                BYTE_42,
                UINT16_42,
                UINT32_42,
                UINT64_42,
                FLOAT_42,
                DOUBLE_42,
                DEC_42,
                ENUM_42,
                STR_INT_42,
                STR_INT_42_INT,
                STR_INT_42_EXP,
                (Amount)42
            );

            var q = from t in NumberTypes
                    from n in numbers
                    select Set(n, t);

            return q;
        }

        [Theory]
        [MemberData(nameof(Decimal42s_x_NumberTypes))]
        public static void ParseAll(object value, Type targetType)
        {
            var expected = System.Convert.ChangeType(42, targetType);
            ConvertAssert.Converts(targetType, value, expected, ConvertOptions.Default);
        }

        public static IEnumerable<object[]> All8Bits = Values(
            "0xFF",
            "0o377",
            "0b11111111"
        );

        [Theory]
        [MemberData(nameof(All8Bits))]
        public static void ParseSByte(string value)
        {
            ConvertAssert.Converts(value, (sbyte)-1, ParseAllOptions);
        }

        [Theory]
        [MemberData(nameof(All8Bits))]
        public static void ParseByte(string value)
        {
            ConvertAssert.Converts(value, byte.MaxValue, ParseAllOptions);
        }

        public static IEnumerable<object[]> All16Bits = Values(
            "0xFFFF",
            "0o177_777",
            "0b11111111_11111111"
        );

        [Theory]
        [MemberData(nameof(All16Bits))]
        public static void ParseShort(string value)
        {
            ConvertAssert.Converts(value, (short)-1, ParseAllOptions);
        }

        [Theory]
        [MemberData(nameof(All16Bits))]
        public static void ParseUShort(string value)
        {
            ConvertAssert.Converts(value, ushort.MaxValue, ParseAllOptions);
        }

        public static IEnumerable<object[]> All32Bits = Values(
            "0xFFFF_FFFF",
            "0o37_777_777_777",
            "0b11111111_11111111_11111111_11111111"
        );

        [Theory]
        [MemberData(nameof(All32Bits))]
        public static void ParseInt(string value)
        {
            ConvertAssert.Converts(value, -1, ParseAllOptions);
        }

        [Theory]
        [MemberData(nameof(All32Bits))]
        public static void ParseUInt(string value)
        {
            ConvertAssert.Converts(value, uint.MaxValue, ParseAllOptions);
        }

        public static IEnumerable<object[]> All64Bits = Values(
            "0xFFFF_FFFF_FFFF_FFFF",
            "0o1_777_777_777_777_777_777_777",
            "0b11111111_11111111_11111111_11111111_11111111_11111111_11111111_11111111"
        );

        [Theory]
        [MemberData(nameof(All64Bits))]
        public static void ParseLong(string value)
        {
            ConvertAssert.Converts(value, -1L, ParseAllOptions);
        }

        [Theory]
        [MemberData(nameof(All64Bits))]
        public static void ParseULong(string value)
        {
            ConvertAssert.Converts(value, ulong.MaxValue, ParseAllOptions);
        }

        [Theory]
        [MemberData(nameof(All64Bits))]
        public static void ParseDecimal(string value)
        {
            var expected = System.Convert.ToDecimal(ulong.MaxValue);
            ConvertAssert.Converts(value, expected, ParseAllOptions);
        }

        [Theory]
        [MemberData(nameof(All64Bits))]
        public static void ParseSingle(string value)
        {
            var expected = System.Convert.ToSingle(ulong.MaxValue);
            ConvertAssert.Converts(value, expected, ParseAllOptions);
        }

        [Theory]
        [MemberData(nameof(All64Bits))]
        public static void ParseDouble(string value)
        {
            var expected = System.Convert.ToDouble(ulong.MaxValue);
            ConvertAssert.Converts(value, expected, ParseAllOptions);
        }
    }
}
