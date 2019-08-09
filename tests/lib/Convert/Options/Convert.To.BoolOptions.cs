using System.Collections.Generic;
using Xunit;

namespace Ockham.Data.Tests
{
    using static ConvertTestRunner;
    using static Factories;

    // Test that ConvertOptions.Booleans settings have the intended effect
    public partial class ConvertToBoolTests
    {

        private static readonly ConvertOptions TrueStringOptions
            = ConvertOptionsBuilder.Default.WithTrueStrings("t", "yes").Options;

        public static IEnumerable<object[]> TrueStringValidData = Values(
            "t", "yes", "T", "Yes"
        );

        public static IEnumerable<object[]> TrueStringInvalidData = Values(
            bool.TrueString, "asdlasd", "x", ""
        );

        [Theory]
        [MemberData(nameof(TrueStringValidData))]
        public static void TrueStringValid(string value)
        {
            ConvertAssert.Converts(value, true, TrueStringOptions);
        }

        [Theory]
        [MemberData(nameof(TrueStringInvalidData))]
        public static void TrueStringInvalid(string value)
        {
            ConvertAssert.ConvertFails<bool>(value, TrueStringOptions);
        }

        private static readonly ConvertOptions FalseStringOptions
         = ConvertOptionsBuilder.Default.WithFalseStrings("f", "no").Options;

        public static IEnumerable<object[]> FalseStringValidData = Values(
            "f", "no", "F", "No"
        );

        public static IEnumerable<object[]> FalseStringInvalidData = Values(
            bool.FalseString, "asdlasd", "x", ""
        );

        [Theory]
        [MemberData(nameof(FalseStringValidData))]
        public static void FalseStringValid(string value)
        {
            ConvertAssert.Converts(value, false, FalseStringOptions);
        }

        [Theory]
        [MemberData(nameof(FalseStringInvalidData))]
        public static void FalseStringInvalid(string value)
        {
            ConvertAssert.ConvertFails<bool>(value, FalseStringOptions);
        }
    }
}
