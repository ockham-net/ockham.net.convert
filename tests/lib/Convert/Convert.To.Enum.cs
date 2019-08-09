using Ockham.Data.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

using static Ockham.Data.Tests.Factories;

namespace Ockham.Data.Tests
{
    using static ConvertTestRunner;

    public partial class StringAsNullTests
    {
        public enum OtherEnum
        {
            NineAndForty = 49
        }

        public static IEnumerable<object[]> ConvertToEnumData = Sets<object, ConvertOptions, TestShortEnum>(
            (49, ConvertOptions.Default, TestShortEnum.FortyNine),
            ("49", ConvertOptions.Default, TestShortEnum.FortyNine),
            ("FortyNine", ConvertOptions.Default, TestShortEnum.FortyNine),
            (OtherEnum.NineAndForty, ConvertOptions.Default, TestShortEnum.FortyNine),
            (new ComplexNumber(49, 0), ConvertOptions.Default, TestShortEnum.FortyNine),
            (null, OptionsVariant.NullToValueDefault, TestShortEnum.Zero),
            (DBNull.Value, OptionsVariant.NullToValueDefault, TestShortEnum.Zero),
            ("0x31", OptionsVariant.ParseBaseN, TestShortEnum.FortyNine),
            ("0O61", OptionsVariant.ParseBaseN, TestShortEnum.FortyNine),
            ("0b110001", OptionsVariant.ParseBaseN, TestShortEnum.FortyNine) 
        );

        [Theory]
        [MemberData(nameof(ConvertToEnumData))]
        public static void ConvertToEnum(object value, ConvertOptions options, TestShortEnum expected)
        {
            TestCustomOverloads<TestShortEnum>(value, options, invoke =>
            {
                var result = invoke();
                Assert.IsType<TestShortEnum>(result);
                Assert.Equal(expected, (TestShortEnum)result);
            });
        }
    }
}
