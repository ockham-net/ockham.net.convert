using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Ockham.Data.Tests
{
    [ExcludeFromCodeCoverage]
    public static class BoolConverterTests
    {
        public static ConvertOptions BoolOptions
            = ConvertOptionsBuilder.Default
                .WithTrueStrings(bool.TrueString, "t", "y")
                .WithFalseStrings(bool.FalseString, "f", "n")
                .WithStringOptions(StringAsNullOption.NullReference, TrimStringFlags.None)
                .Options;

        public static IEnumerable<object[]> TrueValues = (
            new object[] {
                "true",
                "TRUE",
                "t",
                "Y",
                true,
                1,
                1.0m,
                -23
        }).AsObjectArray();

        public static IEnumerable<object[]> FalseValues = (
            new object[] {
                "false",
                "FALSE",
                "f",
                "n",
                false,
                0,
                0.0m
        }).AsObjectArray();

        public static IEnumerable<object[]> Unconvertible = (
            new object[] {
                "fasdsadalse",
                "  FALSE ",
                " true`r`n",
                new object(),
                System.Text.Encoding.UTF8
        }).AsObjectArray();

        [Theory]
        [MemberData(nameof(TrueValues))]
        public static void ToTrue(object value)
        {
            Assert.True(BoolConverter.ToBool(value, BoolOptions));
        }
         
        [Theory]
        [MemberData(nameof(FalseValues))]
        public static void ToFalse(object value)
        {
            Assert.False(BoolConverter.ToBool(value, BoolOptions));
        }

        [Theory]
        [MemberData(nameof(Unconvertible))]
        public static void Invalid(object value)
        {
            Assert.Throws<InvalidCastException>(() => BoolConverter.ToBool(value, BoolOptions));
        }

        [Fact]
        public static void TrimStart()
        {
            var options = ConvertOptionsBuilder.FromConvertOptions(BoolOptions)
                .WithStringOptions(StringAsNullOption.NullReference, TrimStringFlags.TrimStart).Options;

            Assert.True(BoolConverter.ToBool("  true", options));
            Assert.ThrowsAny<InvalidCastException>(() => BoolConverter.ToBool("true  ", options));

            Assert.False(BoolConverter.ToBool("  FALSE", options));
            Assert.ThrowsAny<InvalidCastException>(() => BoolConverter.ToBool("false  ", options));
        }

        [Fact]
        public static void TrimEnd()
        {
            var options = ConvertOptionsBuilder.FromConvertOptions(BoolOptions)
                .WithStringOptions(StringAsNullOption.NullReference, TrimStringFlags.TrimEnd).Options;

            Assert.True(BoolConverter.ToBool("true    ", options));
            Assert.ThrowsAny<InvalidCastException>(() => BoolConverter.ToBool("  true", options));

            Assert.False(BoolConverter.ToBool("FALSE\r ", options));
            Assert.ThrowsAny<InvalidCastException>(() => BoolConverter.ToBool(" false", options));
        }

        [Fact]
        public static void TrimAll()
        {
            var options = ConvertOptionsBuilder.FromConvertOptions(BoolOptions)
                .WithStringOptions(StringAsNullOption.NullReference, TrimStringFlags.TrimAll).Options;

            Assert.True(BoolConverter.ToBool(" \t true    ", options)); 
            Assert.False(BoolConverter.ToBool("  FALSE\r ", options));
        }
    }
}
