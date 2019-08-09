using Ockham.Data.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace Ockham.Data.Tests
{
    using static Factories;
    using static ConvertTestRunner;
    using Fixtures;

    public partial class ValueTypeOptionsTests
    {

        public static IEnumerable<object[]> Defaults = Sets(
            Set(typeof(int), DBNull.Value, 0),
            Set(typeof(TimeSpan), null, TimeSpan.Zero),
            Set(typeof(Amount), null, default(Amount)),
            Set(typeof(DateTime), null, default(DateTime))
        );

        [Theory]
        [MemberData(nameof(Defaults))]
        public static void NullToValueThrows(Type targetType, object value, object _)
        {
            TestCustomOverloads(ConvertOverload.To, targetType, value, ConvertOptions.Default, convert =>
            {
                Assert.ThrowsAny<SystemException>(() => convert());
            });
        }


        [Theory]
        [MemberData(nameof(Defaults))]
        public static void NullToValueDefault(Type targetType, object value, object expected)
        {
            TestCustomOverloads(ConvertOverload.To, targetType, value, OptionsVariant.NullToValueDefault, convert =>
            {
                var result = convert();
                Assert.IsType(targetType, result);
                Assert.IsType(targetType, expected);
                Assert.Equal(expected, result);
            });
        }

    }
}
