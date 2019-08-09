using Ockham.Data.Tests.Fixtures;
using System;
using System.Reflection;
using Xunit;

namespace Ockham.Data.Tests
{
    using static ConvertTestRunner;

    // Test that ConvertOptions.Enums settings have the intended effect
    public partial class ConvertToEnumTests
    {
        [Fact]
        public static void ThrowNames()
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithEnumOptions(UndefinedValueOption.Throw, UndefinedValueOption.Coerce).Options;

            TestCustomOverloads<TestShortEnum>(ConvertOverload.To, true, "Foo", options, (opts, invoke) =>
            {
                Assert.ThrowsAny<SystemException>(() => invoke());
            });
        }

        [Fact]
        public static void IgnoreNames()
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithEnumOptions(UndefinedValueOption.Ignore, UndefinedValueOption.Coerce).Options;

            TestCustomOverloads<TestShortEnum>(ConvertOverload.To, true, "Foo", options, (opts, invoke) =>
            {
                var result = invoke();
                Assert.IsType<TestShortEnum>(result);
                Assert.Equal(default(TestShortEnum), (TestShortEnum)result);
            });
        }

        [Fact]
        public static void ThrowFlagNames()
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithEnumOptions(UndefinedValueOption.Throw, UndefinedValueOption.Coerce).Options;

            TestCustomOverloads<BindingFlags>(ConvertOverload.To, true, "Public, NonPublic, Noodles", options, (opts, invoke) =>
            {
                Assert.ThrowsAny<SystemException>(() => invoke());
            });
        }

        [Fact]
        public static void IgnoreFlagName()
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithEnumOptions(UndefinedValueOption.Ignore, UndefinedValueOption.Coerce).Options;

            TestCustomOverloads<BindingFlags>(ConvertOverload.To, true, "Foo", options, (opts, invoke) =>
            {
                var result = invoke();
                Assert.IsType<BindingFlags>(result);
                Assert.Equal(default, (BindingFlags)result);
            });
        }

        [Fact]
        public static void IgnoreFlagNames()
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithEnumOptions(UndefinedValueOption.Ignore, UndefinedValueOption.Coerce).Options;

            string input = " Public, NonPublic, STATIC, Noodles";
            var expected = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

            TestCustomOverloads<BindingFlags>(ConvertOverload.To, true, input, options, (opts, invoke) =>
            {
                var result = invoke();
                Assert.IsType<BindingFlags>(result);
                Assert.Equal(expected, (BindingFlags)result);
            });
        }

        [Fact]
        public static void ThrowValues()
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithEnumOptions(UndefinedValueOption.Throw, UndefinedValueOption.Throw).Options;

            TestCustomOverloads<TestShortEnum>(ConvertOverload.To, true, 1234, options, (opts, invoke) =>
            {
                ThrowAssert.Throws<InvalidCastException>(() => invoke(), "^Value 1234 is not defined");
            });
        }

        [Fact]
        public static void IgnoreValues()
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithEnumOptions(UndefinedValueOption.Throw, UndefinedValueOption.Ignore).Options;

            short input = 3424;

            TestCustomOverloads<TestShortEnum>(ConvertOverload.To, true, input, options, (opts, invoke) =>
            {
                var result = invoke();
                Assert.IsType<TestShortEnum>(result);
                Assert.Equal(default, (TestShortEnum)result);
            });
        }

        [Fact]
        public static void CoerceValues()
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithEnumOptions(UndefinedValueOption.Throw, UndefinedValueOption.Coerce).Options;

            short input = 3424;

            TestCustomOverloads<TestShortEnum>(ConvertOverload.To, true, input, options, (opts, invoke) =>
            {
                var result = invoke();
                Assert.IsType<TestShortEnum>(result);
                Assert.Equal(input, (short)result);
            });
        }

        [Fact]
        public static void ThrowFlagValues()
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithEnumOptions(UndefinedValueOption.Throw, UndefinedValueOption.Throw).Options;

            var input = (int)(TestFlags.Bit1 | TestFlags.Bit3) | 0x20;

            TestCustomOverloads<TestFlags>(ConvertOverload.To, true, input, options, (opts, invoke) =>
            {
                ThrowAssert.Throws<InvalidCastException>(() => invoke(), "^Bit 0x20 is not defined");
            });
        }


        [Fact]
        public static void IgnoreFlagValues()
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithEnumOptions(UndefinedValueOption.Throw, UndefinedValueOption.Ignore).Options;

            int input = (int)(TestFlags.Bit1 | TestFlags.Bit3) | 0x20;
            var expected = TestFlags.Bit1 | TestFlags.Bit3;

            TestCustomOverloads<TestFlags>(ConvertOverload.To, true, input, options, (opts, invoke) =>
            {
                var result = invoke();
                Assert.IsType<TestFlags>(result);
                Assert.Equal(expected, (TestFlags)result);
            });
        }

        [Fact]
        public static void CoerceFlagValues()
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithEnumOptions(UndefinedValueOption.Throw, UndefinedValueOption.Coerce).Options;

            int input = 34324;

            TestCustomOverloads<TestFlags>(ConvertOverload.To, true, input, options, (opts, invoke) =>
            {
                var result = invoke();
                Assert.IsType<TestFlags>(result);
                Assert.Equal(input, (int)result);
            });
        }

    }
}
