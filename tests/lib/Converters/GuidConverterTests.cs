using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Ockham.Data.Tests
{
    [ExcludeFromCodeCoverage]
    public class GuidConverterTests
    {
        public static Guid ExpectedGuid = new Guid("96ec026beffa42beabd539c21f5f7aeb");

        public static IEnumerable<object[]> Guids = (
            new object[] {
                " 96ec026beffa42beabd539c21f5f7aeb    ",
                "96ec026beffa42beabd539c21f5f7aeb",
                "96EC026BEFFA42BEABD539C21F5F7AEB",
                "96ec026b-effa-42be-abd5-39c21f5f7aeb",
                "96EC026B-EFFA-42BE-ABD5-39C21F5F7AEB",
                "{96ec026b-effa-42be-abd5-39c21f5f7aeb}",
                "{96EC026B-EFFA-42BE-ABD5-39C21F5F7AEB}",
                "(96ec026b-effa-42be-abd5-39c21f5f7aeb)",
                "(96EC026B-EFFA-42BE-ABD5-39C21F5F7AEB)",
                "{0x96ec026b,0xeffa,0x42be,{0xab,0xd5,0x39,0xc2,0x1f,0x5f,0x7a,0xeb}}",
                "{0X96EC026B,0XEFFA,0X42BE,{0XAB,0XD5,0X39,0XC2,0X1F,0X5F,0X7A,0XEB}}",
                ExpectedGuid,
                new byte[]{ 107, 2, 236, 150, 250, 239, 190, 66, 171, 213, 57, 194, 31, 95, 122, 235 }
        }).AsObjectArray();

        public static IEnumerable<object[]> Unconvertible = (
            new object[] {
                "hello",
                22,
                new object(),
                new byte[]{ 107, 2, 236 },
                System.Text.Encoding.UTF8
        }).AsObjectArray();

        [Theory]
        [MemberData(nameof(Guids))]
        public void ToGuid(object value)
        {
            Assert.Equal(ExpectedGuid, GuidConverter.ToGuid(value, ConvertOptions.Default));
        }

        [Theory]
        [MemberData(nameof(Unconvertible))]
        public void Invalid(object value)
        {
            Assert.ThrowsAny<SystemException>(() => GuidConverter.ToGuid(value, ConvertOptions.Default));
        }
    }
}
