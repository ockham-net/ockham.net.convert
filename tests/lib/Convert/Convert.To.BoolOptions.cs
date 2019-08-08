using Ockham.Data.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace Ockham.Data.Tests
{
    using static Factories;

    public partial class ConvertTests
    {

        private static readonly ConvertOptions TrueTYes
            = ConvertOptionsBuilder.Default.WithTrueStrings("t", "yes").Options;

        public static IEnumerable<object[]> TrueStringValidData = Values(
            "t", "yes", "T", "Yes"
        );

        public static IEnumerable<object[]> TrueStringInvalidData = Values(
            "true", "asdlasd", "x", ""
        );

        [Theory]
        [MemberData(nameof(TrueStringValidData))]
        public static void TrueStringValid(string value)
        { 
            TestOverloads<bool>(null, true, value, TrueTYes, (opts, invoke) =>
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
            TestOverloads<bool>(null, true, value, FalseFNo, (opts, invoke) =>
            {
                ThrowAssert.ThrowsAny(invoke);
            });
        }

        private static readonly ConvertOptions FalseFNo
         = ConvertOptionsBuilder.Default.WithFalseStrings("f", "no").Options;

        public static IEnumerable<object[]> FalseStringValidData = Values(
            "t", "yes", "T", "Yes"
        );

        public static IEnumerable<object[]> FalseStringInvalidData = Values(
            "true", "asdlasd", "x", ""

        );
        [Theory]
        [MemberData(nameof(FalseStringValidData))]
        public static void FalseStringValid(string value)
        {
            TestOverloads<bool>(null, true, value, FalseFNo, (opts, invoke) =>
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
            TestOverloads<bool>(null, true, value, TrueTYes, (opts, invoke) =>
            {
                ThrowAssert.ThrowsAny(invoke);
            });
        }
    }
}
