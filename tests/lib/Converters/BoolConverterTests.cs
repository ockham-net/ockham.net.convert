using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Ockham.Data.Tests
{
    [ExcludeFromCodeCoverage]
    public class BoolConverterTests
    {
        public static ConvertOptions Options
            = ConvertOptionsBuilder.Default
                .WithTrueStrings("t", "y")
                .WithFalseStrings("f", "n")
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
        public void ToTrue(object value)
        {
            Assert.True(BoolConverter.ToBool(value, Options));
        }
         
        [Theory]
        [MemberData(nameof(FalseValues))]
        public void ToFalse(object value)
        {
            Assert.False(BoolConverter.ToBool(value, Options));
        }

        [Theory]
        [MemberData(nameof(Unconvertible))]
        public void Invalid(object value)
        {
            Assert.Throws<InvalidCastException>(() => BoolConverter.ToBool(value, Options));
        }
    }
}
