using System.Collections.Generic;
using Xunit;

namespace Ockham.Data.Tests
{
    using static ConvertTestRunner;
    using static Factories;

    // Test that ConvertOptions.Booleans settings have the intended effect
    public partial class ConvertToBoolTests
    {

        private static readonly ConvertOptions TrueTYes
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
            TestCustomOverloads<bool>(null, true, value, TrueTYes, (opts, invoke) =>
            {
                var result = invoke();
                Assert.IsType<bool>(result);
                Assert.True((bool)invoke());
            });
        }

        [Theory]
        [MemberData(nameof(TrueStringInvalidData))]
        public static void TrueStringInvalid(string value)
        {
            TestCustomOverloads<bool>(ConvertOverload.To, true, value, TrueTYes, (opts, invoke) =>
            {
                ThrowAssert.ThrowsAny(invoke);
            });
        }

        private static readonly ConvertOptions FalseFNo
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
            TestCustomOverloads<bool>(null, true, value, FalseFNo, (opts, invoke) =>
            {
                var result = invoke();
                Assert.IsType<bool>(result);
                Assert.False((bool)invoke());
            });
        }

        [Theory]
        [MemberData(nameof(FalseStringInvalidData))]
        public static void FalseStringInvalid(string value)
        {
            TestCustomOverloads<bool>(ConvertOverload.To, true, value, FalseFNo, (opts, invoke) =>
            {
                ThrowAssert.ThrowsAny(invoke);
            });
        }
    }
}
