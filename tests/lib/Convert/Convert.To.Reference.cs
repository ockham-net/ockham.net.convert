using Ockham.Data.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

using static Ockham.Data.Tests.Factories;

namespace Ockham.Data.Tests
{
    public partial class ConvertTests
    {
        public static IEnumerable<object[]> StarData = Values("hi", 2, new object());

        public static IEnumerable<object[]> StringData = Sets(
            Pair(123, "123"),
            Pair(3.3333m, "3.3333"),
            Pair(new NeutronStar(), "I'm a neutron star")
        );

        // Custom converter is invoked
        [Theory]
        [MemberData(nameof(StarData))]
        public void ToCustomClass(object value)
        {
            var invokeCount = 0;
            var star = new Star();
            var converter = new Converter(ConvertOptions.Default)
                .WithConverter<Star>((_, opts) =>
                {
                    invokeCount++;
                    return star;
                });

            var result = converter.To<Star>(value);
            Assert.Same(result, star);
            Assert.Equal(1, invokeCount);
        }

        [Theory]
        [MemberData(nameof(StringData))]
        public void ConvertToString(object value, string expected)
        {
            TestOverloads<string>(value, CustomOptions, (options, invoke) =>
            {
                var result = invoke() as string;
                Assert.Equal(expected, result);
            });
        }

    }
}
