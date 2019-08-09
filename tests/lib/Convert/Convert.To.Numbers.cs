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

        private static readonly Dictionary<Type, Action<object>> Parse42Funcs;

        static ConvertToNumberTests()
        {
            var m = typeof(ConvertToNumberTests).GetMethod("_ParseAll", BindingFlags.Static | BindingFlags.NonPublic);

            Parse42Funcs = NumberTypes.ToDictionary(
               t => t,
               t => (Action<object>)m.MakeGenericMethod(t).CreateDelegate(typeof(Action<object>))
            );
        }

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
            var func = Parse42Funcs[targetType];
            func(value);
        }

        private static void _ParseAll<T>(object value)
        {
            var expected = Convert.To<T>(42);
            TestOverloads<T>(value, ConvertOptions.Default, (opts, convert) =>
            {
                var result = convert();
                Assert.IsType<T>(result);
                Assert.Equal(expected, (T)result);
            });
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
            TestCustomOverloads<sbyte>(value, ParseAllOptions, convert =>
            {
                var result = convert();
                Assert.IsType<sbyte>(result);
                Assert.Equal<sbyte>(-1, (sbyte)result);
            });
        }

        [Theory]
        [MemberData(nameof(All8Bits))]
        public static void ParseByte(string value)
        {
            TestCustomOverloads<byte>(value, ParseAllOptions, convert =>
            {
                var result = convert();
                Assert.IsType<byte>(result);
                Assert.Equal<byte>(255, (byte)result);
            });
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
            TestCustomOverloads<short>(value, ParseAllOptions, convert =>
            {
                var result = convert();
                Assert.IsType<short>(result);
                Assert.Equal<short>(-1, (short)result);
            });
        }

        [Theory]
        [MemberData(nameof(All16Bits))]
        public static void ParseUShort(string value)
        {
            TestCustomOverloads<ushort>(value, ParseAllOptions, convert =>
            {
                var result = convert();
                Assert.IsType<ushort>(result);
                Assert.Equal(ushort.MaxValue, (ushort)result);
            });
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
            TestCustomOverloads<int>(value, ParseAllOptions, convert =>
            {
                var result = convert();
                Assert.IsType<int>(result);
                Assert.Equal(-1, (int)result);
            });
        }

        [Theory]
        [MemberData(nameof(All32Bits))]
        public static void ParseUInt(string value)
        {
            TestCustomOverloads<uint>(value, ParseAllOptions, convert =>
            {
                var result = convert();
                Assert.IsType<uint>(result);
                Assert.Equal(uint.MaxValue, (uint)result);
            });
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
            TestCustomOverloads<long>(value, ParseAllOptions, convert =>
            {
                var result = convert();
                Assert.IsType<long>(result);
                Assert.Equal(-1, (long)result);
            });
        }

        [Theory]
        [MemberData(nameof(All64Bits))]
        public static void ParseULong(string value)
        {
            TestCustomOverloads<ulong>(value, ParseAllOptions, convert =>
            {
                var result = convert();
                Assert.IsType<ulong>(result);
                Assert.Equal(ulong.MaxValue, (ulong)result);
            });
        }

        [Theory]
        [MemberData(nameof(All64Bits))]
        public static void ParseDecimal(string value)
        {
            var expected = System.Convert.ToDecimal(ulong.MaxValue);
            TestCustomOverloads<decimal>(value, ParseAllOptions, convert =>
            {
                var result = convert();
                Assert.IsType<decimal>(result);
                Assert.Equal(expected, (decimal)result);
            });
        }

        [Theory]
        [MemberData(nameof(All64Bits))]
        public static void ParseSingle(string value)
        {
            var expected = System.Convert.ToSingle(ulong.MaxValue);
            TestCustomOverloads<float>(value, ParseAllOptions, convert =>
            {
                var result = convert();
                Assert.IsType<float>(result);
                Assert.Equal(expected, (float)result);
            });
        }

        [Theory]
        [MemberData(nameof(All64Bits))]
        public static void ParseDouble(string value)
        {
            var expected = System.Convert.ToDouble(ulong.MaxValue);
            TestCustomOverloads<double>(value, ParseAllOptions, convert =>
            {
                var result = convert();
                Assert.IsType<double>(result);
                Assert.Equal(expected, (double)result);
            });
        }

    }
}
