using Ockham.Data.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;
using System.Linq;

namespace Ockham.Data.Tests
{
    using static Factories;
    using static ConvertTestRunner;
    using Fixtures;

    public partial class ValueTypeOptionsTests
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
            TestCustomOverloads(ConvertOverload.To, targetType, null, ConvertOptions.Default, convert =>
            {
                Assert.ThrowsAny<SystemException>(() => convert());
            });

            TestCustomOverloads(ConvertOverload.To, targetType, DBNull.Value, ConvertOptions.Default, convert =>
            {
                Assert.ThrowsAny<SystemException>(() => convert());
            });
        }

        [Theory]
        [MemberData(nameof(Defaults))]
        public static void NullToValueDefault(Type targetType, object expected)
        {
            TestCustomOverloads(ConvertOverload.To, targetType, null, OptionsVariant.NullToValueDefault, convert =>
            {
                var result = convert();
                Assert.IsType(targetType, result);
                Assert.IsType(targetType, expected);
                Assert.Equal(expected, result);
            });

            TestCustomOverloads(ConvertOverload.To, targetType, DBNull.Value, OptionsVariant.NullToValueDefault, convert =>
            {
                var result = convert();
                Assert.IsType(targetType, result);
                Assert.IsType(targetType, expected);
                Assert.Equal(expected, result);
            });
        }
    }
}
