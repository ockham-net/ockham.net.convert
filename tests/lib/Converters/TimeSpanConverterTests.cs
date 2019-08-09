using Ockham.Data.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ockham.Data.Tests
{
    using static Factories;

    public class TimeSpanConverterTests
    {
        public static IEnumerable<object[]> Unconvertible => (new object[]
        {
            "Hello",
            new object(),
            (Func<int>)(() => 42)
        }).AsObjectArray();

        public static IEnumerable<object[]> ComplexStrings => new[]
        {
            Set("0:02.231", new TimeSpan(0, 0, 0, 2, 231)),
            Set("5:14", new TimeSpan(0, 0, 5, 14, 0)),
            Set("5:14.129", new TimeSpan(0, 0, 5, 14, 129)),
            Set("01:00.010", new TimeSpan(0, 0, 1, 0, 10)),
            Set("3:12:00.000", new TimeSpan(0, 3, 12, 0, 0)),
            Set("231.3:12:00.000", new TimeSpan(231, 3, 12, 0, 0))
        };

        public static IEnumerable<object[]> DecimalStrings => Values(
            "3423.2423",
            "432.3432",
            "0.006565",
            "3.209213",
            "001.0"
        );

        public static IEnumerable<object[]> WholeNumeric => Values(
            0,
            0m,
            0.0f,
            0.0d,
            0UL,
            (byte)0,
            (uint)0,
            23409234234,
            3242.00,
            168124m,
            "0",
            "23",
            "42342"
         );

        public static IEnumerable<object[]> Timespans => ComplexStrings.Select(arr => arr[1]).AsObjectArray();

        [Theory]
        [MemberData(nameof(Timespans))]
        public void PreserveTimeSpan(TimeSpan input)
        {
            Assert.Equal(input, TimeSpanConverter.ToTimeSpan(input, ConvertOptions.Default));
        }

        [Theory]
        [MemberData(nameof(DecimalStrings))]
        public void DecimalSecondsToTimespan(object input)
        {
            double dblVal = Convert.To<double>(input);
            Assert.Equal(TimeSpan.FromSeconds(dblVal), TimeSpanConverter.ToTimeSpan(input, ConvertOptions.Default));
        }

        [Fact]
        public void DateTimeToTimespan()
        {
            DateTime dateTime = DateTime.UtcNow;
            Assert.Equal(TimeSpan.FromTicks(dateTime.Ticks), TimeSpanConverter.ToTimeSpan(dateTime, ConvertOptions.Default));
        }

        [Theory]
        [MemberData(nameof(WholeNumeric))]
        public void TicksToTimespan(object input)
        {
            long lngVal = Convert.To<long>(input);
            Assert.Equal(TimeSpan.FromTicks(lngVal), TimeSpanConverter.ToTimeSpan(input, ConvertOptions.Default));
        }

        [Theory]
        [MemberData(nameof(ComplexStrings))]
        public void TimestringToTimespan(object input, TimeSpan expected)
        {
            Assert.Equal(expected, TimeSpanConverter.ToTimeSpan(input, ConvertOptions.Default));
        }

        [Theory]
        [MemberData(nameof(Unconvertible))]
        public void GarbageInThrows(object input)
        {
            Assert.ThrowsAny<SystemException>(() => TimeSpanConverter.ToTimeSpan(input, ConvertOptions.Default));
        }
    }
}
