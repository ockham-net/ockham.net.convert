using Ockham.Data.Tests.Fixtures;
using System;
using Xunit;

namespace Ockham.Data.Tests
{
    public partial class StringAsNullTests
    {
        [Fact]
        public void NullToNull()
        {
            foreach (var options in new[] { ConvertOptions.Default, OptionsVariant.EmptyStringAsNull, OptionsVariant.WhitespaceAsNull })
            {
                ConvertAssert.ConvertsToNull<CelestialBody>(null, options); 
            }
        }

        [Fact]
        public void DBNullToNull()
        {
            foreach (var options in new[] { ConvertOptions.Default, OptionsVariant.EmptyStringAsNull, OptionsVariant.WhitespaceAsNull })
            {
                ConvertAssert.ConvertsToNull<CelestialBody>(DBNull.Value, options); 
            }
        }

        [Fact]
        public void EmptyToNull()
        {
            string input = "";

            foreach (var options in new[] { ConvertOptions.Default, OptionsVariant.EmptyStringAsNull, OptionsVariant.WhitespaceAsNull })
            {
                // Force always converts to null
                ConvertAssert.ForcesToNull<CelestialBody>(input, options); 
            }

            foreach (var options in new[] { ConvertOptions.Default })
            {
                // To fails because empty string is not considered null
                ConvertAssert.ConvertFails<CelestialBody>(input, options); 
            }

            foreach (var options in new[] { OptionsVariant.EmptyStringAsNull, OptionsVariant.WhitespaceAsNull })
            {
                ConvertAssert.ConvertsToNull<CelestialBody>(input, options);
            }
        }

        [Fact]
        public void WhitespaceToNull()
        {
            string input = "  \r\n  \t";

            foreach (var options in new[] { ConvertOptions.Default, OptionsVariant.EmptyStringAsNull })
            {
                // Force always converts to null
                ConvertAssert.ForcesToNull<CelestialBody>(input, options);

                // To fails because whitespace string is not considered null
                ConvertAssert.ConvertFails<CelestialBody>(input, options);
            }

            foreach (var options in new[] { OptionsVariant.WhitespaceAsNull })
            {
                ConvertAssert.ConvertsToNull<CelestialBody>(input, options);
            }
        }
    }
}
