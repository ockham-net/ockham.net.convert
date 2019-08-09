using Ockham.Data.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace Ockham.Data.Tests
{
    using static Factories;

    public static class ConvertToSelfTests
    {

        public static IEnumerable<object[]> SelfValues = Sets(
            Set(typeof(int), 42),
            Set(typeof(string), " Hello   "),
            Set(typeof(Star), new NeutronStar()),
            Set(typeof(Amount), (Amount)20),
            Set(typeof(Encoding), Encoding.UTF8)
        );

        [Theory]
        [MemberData(nameof(SelfValues))]
        public static void ConvertToSelf(Type targetType, object expected)
        {
            ConvertAssert.Converts(targetType, expected, expected);
        }

    }
}
