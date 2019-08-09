using Ockham.Data.Tests.Fixtures;
using System.Reflection;
using Xunit;

namespace Ockham.Data.Tests
{
    // Test that ConvertOptions.Enums settings have the intended effect
    public partial class ConvertToEnumTests
    {
        [Fact]
        public static void ThrowNames()
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithEnumOptions(UndefinedValueOption.Throw, UndefinedValueOption.Coerce).Options;

            ConvertAssert.ConvertFails<TestShortEnum>("Foo", options);
        }

        [Fact]
        public static void IgnoreNames()
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithEnumOptions(UndefinedValueOption.Ignore, UndefinedValueOption.Coerce).Options;

            ConvertAssert.Converts("Foo", default(TestShortEnum), options);
        }

        [Fact]
        public static void ThrowFlagNames()
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithEnumOptions(UndefinedValueOption.Throw, UndefinedValueOption.Coerce).Options;

            ConvertAssert.ConvertFails<BindingFlags>("Public, NonPublic, Noodles", options);
        }

        [Fact]
        public static void IgnoreFlagName()
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithEnumOptions(UndefinedValueOption.Ignore, UndefinedValueOption.Coerce).Options;

            ConvertAssert.Converts("Foo", default(BindingFlags), options);
        }

        [Fact]
        public static void IgnoreFlagNames()
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithEnumOptions(UndefinedValueOption.Ignore, UndefinedValueOption.Coerce).Options;

            string input = " Public, NonPublic, STATIC, Noodles";
            var expected = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

            ConvertAssert.Converts(input, expected, options);
        }

        [Fact]
        public static void ThrowValues()
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithEnumOptions(UndefinedValueOption.Throw, UndefinedValueOption.Throw).Options;

            ConvertAssert.ConvertFails<TestShortEnum>(1234, options, "^Value 1234 is not defined");
        }

        [Fact]
        public static void IgnoreValues()
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithEnumOptions(UndefinedValueOption.Throw, UndefinedValueOption.Ignore).Options;

            ConvertAssert.Converts((short)3424, default(TestShortEnum), options);
        }

        [Fact]
        public static void CoerceValues()
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithEnumOptions(UndefinedValueOption.Throw, UndefinedValueOption.Coerce).Options;

            ConvertAssert.Converts((short)3424, (TestShortEnum)3424, options);
        }

        [Fact]
        public static void ThrowFlagValues()
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithEnumOptions(UndefinedValueOption.Throw, UndefinedValueOption.Throw).Options;

            var input = (int)(TestFlags.Bit1 | TestFlags.Bit3) | 0x20;

            ConvertAssert.ConvertFails<TestFlags>(input, options, "^Bit 0x20 is not defined");
        }

        [Fact]
        public static void IgnoreFlagValues()
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithEnumOptions(UndefinedValueOption.Throw, UndefinedValueOption.Ignore).Options;

            int input = (int)(TestFlags.Bit1 | TestFlags.Bit3) | 0x20;
            var expected = TestFlags.Bit1 | TestFlags.Bit3;

            ConvertAssert.Converts(input, expected, options);
        }

        [Fact]
        public static void CoerceFlagValues()
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithEnumOptions(UndefinedValueOption.Throw, UndefinedValueOption.Coerce).Options;

            int input = 34324;

            ConvertAssert.Converts(input, (TestFlags)input, options);
        }

    }
}
