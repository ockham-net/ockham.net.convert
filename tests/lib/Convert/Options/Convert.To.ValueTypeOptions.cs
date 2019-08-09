using System;
using System.Collections.Generic;
using Xunit;

namespace Ockham.Data.Tests
{
    using Fixtures;
    using static Factories;

    public class ValueTypeOptionsTests
    {
        public static readonly IEnumerable<object[]> Defaults = Sets(
            Set(typeof(int), 0),
            Set(typeof(TimeSpan), TimeSpan.Zero),
            Set(typeof(Amount), default(Amount)),
            Set(typeof(DateTime), default(DateTime))
        );

        public static readonly IEnumerable<object[]> Types = Values(
            typeof(int),
            typeof(TimeSpan),
            typeof(Amount),
            typeof(DateTime)
        );

        [Theory]
        [MemberData(nameof(Types))]
        public static void NullToValueThrows(Type targetType)
        {
            ConvertAssert.ConvertFails(targetType, null);
            ConvertAssert.ConvertFails(targetType, DBNull.Value);
        }

        [Theory]
        [MemberData(nameof(Defaults))]
        public static void NullToValueDefault(Type targetType, object expected)
        {
            ConvertAssert.Converts(targetType, null, expected, OptionsVariant.NullToValueDefault);
            ConvertAssert.Converts(targetType, DBNull.Value, expected, OptionsVariant.NullToValueDefault);
        }
    }
}
