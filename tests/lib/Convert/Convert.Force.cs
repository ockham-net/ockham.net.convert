using Ockham.Data.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ockham.Data.Tests
{
    using static Factories;

    public partial class ConvertForceTests
    {
        public static IEnumerable<object[]> Defaults = Sets(
            Set(typeof(int), DBNull.Value, 0),
            Set(typeof(TimeSpan), null, TimeSpan.Zero),
            Set(typeof(Amount), null, default(Amount)),
            Set(typeof(DateTime), null, default(DateTime)),
            Set(typeof(Star), Encoding.UTF8, null),
            Set(typeof(int), "adlkasdlkasjdsa", 0),
            Set(typeof(TimeSpan), "adlkasdlkasjdsa", TimeSpan.Zero),
            Set(typeof(Amount), "adlkasdlkasjdsa", default(Amount)),
            Set(typeof(DateTime), "adlkasdlkasjdsa", default(DateTime)),
            Set(typeof(Star), "adlkasdlkasjdsa", null)
        );

        public static IEnumerable<object[]> Unconvertible = Values(Encoding.UTF8, new TestStruct(), "blahblah");

        [Theory]
        [MemberData(nameof(Defaults))]
        public static void ForceToTypeDefault(Type targetType, object value, object expected)
        {
            ConvertAssert.Converts(ConvertOverload.Force, targetType, value, expected, ConvertOptions.Default);
        }

        [Theory]
        [MemberData(nameof(Unconvertible))]
        public static void ForceToExplicitDefault_Int(object value)
        {
            ForceToExplicitDefault(value, 42);
        }

        [Theory]
        [MemberData(nameof(Unconvertible))]
        public static void ForceToExplicitDefault_DateTime(object value)
        {
            ForceToExplicitDefault(value, DateTime.UtcNow);
        }

        [Theory]
        [MemberData(nameof(Unconvertible))]
        public static void ForceToExplicitDefault_Amount(object value)
        {
            ForceToExplicitDefault(value, (Amount)42);
        }

        [Theory]
        [MemberData(nameof(Unconvertible))]
        public static void ForceToExplicitDefault_Class(object value)
        {
            ForceToExplicitDefault<Star>(value, new NeutronStar());
        }

        private static void ForceToExplicitDefault<T>(object input, T defaultValue)
        {
            T result = Convert.Force(input, defaultValue);
            Assert.Equal(defaultValue, result);

            result = Converter.Default.Force(input, defaultValue);
            Assert.Equal(defaultValue, result);
        }
    }
}
