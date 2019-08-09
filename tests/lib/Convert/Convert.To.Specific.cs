using Ockham.Data.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ockham.Data.Tests
{
    using static Factories;

    public partial class ConvertToSpecificTests
    {
        public static readonly Converter DefaultConverter = Converter.Default;

        public static readonly Converter NullToDefaultConverter = OptionsVariant.NullToValueDefault.GetConverter();

        public static IEnumerable<object[]> TrueValues = Values(
            "true",
            "TRUE",
            true,
            1,
            1.0m,
            -23
        );

        public static IEnumerable<object[]> FalseValues = Values(
            "false",
            "FALSE",
            false,
            0,
            0.0m
        );

        public static readonly IEnumerable<object[]> Number42s = Values(
            ConvertToNumberTests.SBYTE_42,
            ConvertToNumberTests.INT16_42,
            ConvertToNumberTests.INT32_42,
            ConvertToNumberTests.INT64_42,
            ConvertToNumberTests.BYTE_42,
            ConvertToNumberTests.UINT16_42,
            ConvertToNumberTests.UINT32_42,
            ConvertToNumberTests.UINT64_42,
            ConvertToNumberTests.FLOAT_42,
            ConvertToNumberTests.DOUBLE_42,
            ConvertToNumberTests.DEC_42
         );

        public static IEnumerable<object[]> FortyTwos => ConvertToNumberTests.Decimal42s_Strict;

        [Theory]
        [MemberData(nameof(TrueValues))]
        public static void ToTrue(object value)
        {
            Assert.True(Convert.ToBoolean(value));
            Assert.True(DefaultConverter.ToBoolean(value));
        }

        [Theory]
        [MemberData(nameof(FalseValues))]
        public static void ToFalse(object value)
        {
            Assert.False(Convert.ToBoolean(value));
            Assert.False(DefaultConverter.ToBoolean(value));
        }

        [Theory]
        [MemberData(nameof(FortyTwos))]
        public static void ToInt32(object value)
        {
            Assert.Equal(42, Convert.ToInt32(value));
            Assert.Equal(42, DefaultConverter.ToInt32(value));
        }

        [Theory]
        [MemberData(nameof(FortyTwos))]
        public static void ToInt64(object value)
        {
            Assert.Equal(42L, Convert.ToInt64(value));
            Assert.Equal(42L, DefaultConverter.ToInt64(value));
        }

        [Theory]
        [MemberData(nameof(FortyTwos))]
        public static void ToDecimal(object value)
        {
            Assert.Equal(42m, Convert.ToDecimal(value));
            Assert.Equal(42m, DefaultConverter.ToDecimal(value));
        }

        [Theory]
        [MemberData(nameof(FortyTwos))]
        public static void ToDouble(object value)
        {
            Assert.Equal(42.0, Convert.ToDouble(value));
            Assert.Equal(42.0, DefaultConverter.ToDouble(value));
        }

        [Theory]
        [MemberData(nameof(Number42s))]
        public static void ToStringTest(object value)
        {
            Assert.Equal("42", Convert.ToString(value));
            Assert.Equal("42", DefaultConverter.ToString(value));
        }

        [Fact]
        public static void NullToDefault()
        {
            var converter = NullToDefaultConverter;
            ConvertAssert.Equal(false, converter.ToBoolean(null));
            ConvertAssert.Equal(default(DateTime), converter.ToDateTime(null));
            ConvertAssert.Equal(0.0, converter.ToDouble(null));
            ConvertAssert.Equal(0m, converter.ToDecimal(null));
            ConvertAssert.Equal(Guid.Empty, converter.ToGuid(null));
            ConvertAssert.Equal(0, converter.ToInt32(null));
            ConvertAssert.Equal(0L, converter.ToInt64(null));
            ConvertAssert.Equal(TimeSpan.Zero, converter.ToTimeSpan(null));
        }


        public static readonly IEnumerable<object[]> Empty = Values(null, DBNull.Value);
        public static readonly IEnumerable<object[]> Invalid = Values(new NeutronStar(), "foo", Encoding.ASCII);

        [Theory]
        [MemberData(nameof(Empty))]
        public static void NullThrows(object value)
        {
            var converter = DefaultConverter;
            Assert.ThrowsAny<Exception>(() => converter.ToBoolean(value));
            Assert.ThrowsAny<Exception>(() => converter.ToDateTime(value));
            Assert.ThrowsAny<Exception>(() => converter.ToDecimal(value));
            Assert.ThrowsAny<Exception>(() => converter.ToDouble(value));
            Assert.ThrowsAny<Exception>(() => converter.ToGuid(value));
            Assert.ThrowsAny<Exception>(() => converter.ToInt32(value));
            Assert.ThrowsAny<Exception>(() => converter.ToInt64(value));
            Assert.ThrowsAny<Exception>(() => converter.ToTimeSpan(value));

            Assert.ThrowsAny<Exception>(() => Convert.ToBoolean(value));
            Assert.ThrowsAny<Exception>(() => Convert.ToDateTime(value));
            Assert.ThrowsAny<Exception>(() => Convert.ToDecimal(value));
            Assert.ThrowsAny<Exception>(() => Convert.ToDouble(value));
            Assert.ThrowsAny<Exception>(() => Convert.ToGuid(value));
            Assert.ThrowsAny<Exception>(() => Convert.ToInt32(value));
            Assert.ThrowsAny<Exception>(() => Convert.ToInt64(value));
            Assert.ThrowsAny<Exception>(() => Convert.ToTimeSpan(value));
        }

        [Theory]
        [MemberData(nameof(Invalid))]
        public static void InvalidThrows(object invalid)
        {
            var converter = NullToDefaultConverter;
            Assert.ThrowsAny<Exception>(() => converter.ToBoolean(invalid));
            Assert.ThrowsAny<Exception>(() => converter.ToDateTime(invalid));
            Assert.ThrowsAny<Exception>(() => converter.ToDecimal(invalid));
            Assert.ThrowsAny<Exception>(() => converter.ToDouble(invalid));
            Assert.ThrowsAny<Exception>(() => converter.ToGuid(invalid));
            Assert.ThrowsAny<Exception>(() => converter.ToInt32(invalid));
            Assert.ThrowsAny<Exception>(() => converter.ToInt64(invalid));
            Assert.ThrowsAny<Exception>(() => converter.ToTimeSpan(invalid));

            Assert.ThrowsAny<Exception>(() => Convert.ToBoolean(invalid));
            Assert.ThrowsAny<Exception>(() => Convert.ToDateTime(invalid));
            Assert.ThrowsAny<Exception>(() => Convert.ToDecimal(invalid));
            Assert.ThrowsAny<Exception>(() => Convert.ToDouble(invalid));
            Assert.ThrowsAny<Exception>(() => Convert.ToGuid(invalid));
            Assert.ThrowsAny<Exception>(() => Convert.ToInt32(invalid));
            Assert.ThrowsAny<Exception>(() => Convert.ToInt64(invalid));
            Assert.ThrowsAny<Exception>(() => Convert.ToTimeSpan(invalid));
        }

        [Fact]
        public static void ToSelf()
        {
            var converter = DefaultConverter;
            var date = DateTime.UtcNow;
            var guid = Guid.NewGuid();
            var timespan = date.TimeOfDay;

            ConvertAssert.Equal(true, converter.ToBoolean(true));
            ConvertAssert.Equal(date, converter.ToDateTime(date));
            ConvertAssert.Equal(42m, converter.ToDecimal(42m));
            ConvertAssert.Equal(42.0, converter.ToDouble(42.0));
            ConvertAssert.Equal(guid, converter.ToGuid(guid));
            ConvertAssert.Equal(42, converter.ToInt32(42));
            ConvertAssert.Equal(42L, converter.ToInt64(42L));
            ConvertAssert.Equal("hi", converter.ToString("hi"));
            ConvertAssert.Equal(timespan, converter.ToTimeSpan(timespan));

            ConvertAssert.Equal(true, Convert.ToBoolean(true));
            ConvertAssert.Equal(date, Convert.ToDateTime(date));
            ConvertAssert.Equal(42m, Convert.ToDecimal(42m));
            ConvertAssert.Equal(42.0, Convert.ToDouble(42.0));
            ConvertAssert.Equal(guid, Convert.ToGuid(guid));
            ConvertAssert.Equal(42, Convert.ToInt32(42));
            ConvertAssert.Equal(42L, Convert.ToInt64(42L));
            ConvertAssert.Equal("hi", Convert.ToString("hi"));
            ConvertAssert.Equal(timespan, Convert.ToTimeSpan(timespan));
        }

    }
}
