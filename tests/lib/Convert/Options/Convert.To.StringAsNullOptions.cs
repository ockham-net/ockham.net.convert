using Ockham.Data.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace Ockham.Data.Tests
{
    using static ConvertTestRunner;

    public partial class StringAsNullTests
    {
        [Fact]
        public void NullToNull()
        {
            foreach (var options in new[] { ConvertOptions.Default, OptionsVariant.EmptyStringAsNull, OptionsVariant.WhitespaceAsNull })
            {
                TestOverloads<CelestialBody>(null, options, (opts, invoke) =>
                {
                    Assert.Null(invoke());
                });
            }
        }

        [Fact]
        public void DBNullToNull()
        {
            foreach (var options in new[] { ConvertOptions.Default, OptionsVariant.EmptyStringAsNull, OptionsVariant.WhitespaceAsNull })
            {
                TestOverloads<CelestialBody>(DBNull.Value, options, (opts, invoke) =>
                {
                    Assert.Null(invoke());
                });
            }
        }

        [Fact]
        public void EmptyToNull()
        {
            foreach (var options in new[] { ConvertOptions.Default, OptionsVariant.EmptyStringAsNull, OptionsVariant.WhitespaceAsNull })
            {
                // Force always converts to null
                TestOverloads<CelestialBody>(ConvertOverload.Force, "", options, invoke =>
               {
                   Assert.Null(invoke());
               });
            }

            foreach (var options in new[] { ConvertOptions.Default })
            {
                // To fails because empty string is not considered null
                TestCustomOverloads<CelestialBody>(ConvertOverload.To, "", options, invoke =>
                {
                    Assert.ThrowsAny<SystemException>(() => invoke());
                });
            }

            foreach (var options in new[] { OptionsVariant.EmptyStringAsNull, OptionsVariant.WhitespaceAsNull })
            {
                TestCustomOverloads<CelestialBody>(ConvertOverload.To, "", options, invoke =>
                {
                    Assert.Null(invoke());
                });
            }
        }

        [Fact]
        public void WhitespaceToNull()
        {
            string whitespace = "  \r\n  \t";
            foreach (var options in new[] { ConvertOptions.Default, OptionsVariant.EmptyStringAsNull })
            {
                // Force always converts to null
                TestOverloads<CelestialBody>(ConvertOverload.Force, whitespace, options, invoke =>
                {
                    Assert.Null(invoke());
                });

                // To fails because whitespace string is not considered null
                TestCustomOverloads<CelestialBody>(ConvertOverload.To, whitespace, options, invoke =>
                {
                    Assert.ThrowsAny<SystemException>(() => invoke());
                });
            }

            foreach (var options in new[] { OptionsVariant.WhitespaceAsNull })
            {
                TestCustomOverloads<CelestialBody>(ConvertOverload.To, whitespace, options, invoke =>
                {
                    Assert.Null(invoke());
                });
            }
        }
    }
}
