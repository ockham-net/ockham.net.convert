using System;
using Xunit;

namespace Ockham.Data.Tests
{
    public class EnumConvertOptionsTests
    {
        [Fact]
        public void Ctor()
        {
            var opts = new EnumConvertOptions(UndefinedValueOption.Throw, UndefinedValueOption.Coerce);
            Assert.NotNull(opts);
        }

        [Fact]
        public void Default() => TestDefault(EnumConvertOptions.Default);

        [Fact]
        public void DefaultFromBuilder() => TestDefault(ConvertOptionsBuilder.Default.GetOptions<EnumConvertOptions>());

        [Fact]
        public void DefaultFromOptions() => TestDefault(ConvertOptions.Default.Enums);

        private void TestDefault(EnumConvertOptions opts)
        {
            // Returns a value
            Assert.NotNull(opts);

            // Always the same instance
            Assert.Same(opts, EnumConvertOptions.Default);

            // Expected configuration
            Assert.Equal(UndefinedValueOption.Throw, opts.UndefinedNames);
            Assert.Equal(UndefinedValueOption.Coerce, opts.UndefinedValues);
        }

        [Fact]
        public void CannotCoerceNames()
        {
            Assert.Throws<ArgumentException>(() => new EnumConvertOptions(UndefinedValueOption.Coerce, UndefinedValueOption.Coerce));
        }

        [Fact]
        public void NameOptions()
        {
            var opts = new EnumConvertOptions(UndefinedValueOption.Throw, UndefinedValueOption.Coerce);
            Assert.Equal(UndefinedValueOption.Throw, opts.UndefinedNames);
            Assert.False(opts.IgnoreUndefinedNames);
        }

        [Fact]
        public void ValueOptions()
        {
            var opts = new EnumConvertOptions(UndefinedValueOption.Throw, UndefinedValueOption.Coerce);
            Assert.Equal(UndefinedValueOption.Coerce, opts.UndefinedValues);

            // Validate memoized flags
            Assert.False(opts.IgnoreUndefinedValues);
            Assert.True(opts.CoerceUndefinedValues);
        }
    }
}
