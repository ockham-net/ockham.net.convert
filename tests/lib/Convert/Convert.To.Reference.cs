using Ockham.Data.Tests.Fixtures;
using System.Collections.Generic;
using Xunit;

using static Ockham.Data.Tests.Factories;

namespace Ockham.Data.Tests
{
    using static ConvertTestRunner;

    public class ConvertToReferenceTests
    {
        public static IEnumerable<object[]> StarData = Values("hi", 2, new object());

        public static IEnumerable<object[]> StringData = Sets(
            Set(123, "123"),
            Set(3.3333m, "3.3333"),
            Set(new NeutronStar(), "I'm a neutron star")
        );

        // Custom converter is invoked
        [Theory]
        [MemberData(nameof(StarData))]
        public void ToCustomClass(object value)
        {
            long invokeCount = 0;
            var star = new Star();
            var options = ConvertOptionsBuilder.Default
                .WithConverter<Star>((_, opts) =>
                {
                    invokeCount++;
                    return star;
                }).Options;

            TestCustomOverloads<Star>(value, options, invoke =>
            {
                object result = null;
                ConvertAssert.Increments(ref invokeCount, () => result = invoke());
                Assert.Same(result, star);
            });
        }

        [Theory]
        [MemberData(nameof(StringData))]
        public void ConvertToString(object value, string expected)
        {
            ConvertAssert.Converts(value, expected);
        }

    }
}
