using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Ockham.Data.Tests
{
    [ExcludeFromCodeCoverage]
    public class ValueTypeConvertOptionsTests
    {
        [Fact]
        public void Ctor()
        {
            var opts = new ValueTypeConvertOptions(ValueTypeConvertFlags.NullToValueDefault);
            Assert.NotNull(opts);
        }

        [Fact]
        public void Default() => TestDefault(ValueTypeConvertOptions.Default);

        [Fact]
        public void DefaultFromBuilder() => TestDefault(ConvertOptionsBuilder.Default.GetOptions<ValueTypeConvertOptions>());

        [Fact]
        public void DefaultFromOptions() => TestDefault(ConvertOptions.Default.ValueTypes);
         
        private void TestDefault(ValueTypeConvertOptions opts)
        {  // Returns a value
            Assert.NotNull(opts);

            // Always the same instance
            Assert.Same(opts, ValueTypeConvertOptions.Default);

            // Expected configuration
            Assert.Equal(ValueTypeConvertFlags.None, opts.ConvertFlags);
        }

        [Fact]
        public void Flags()
        {
            var opts = new ValueTypeConvertOptions(ValueTypeConvertFlags.NullToValueDefault);
            Assert.Equal(ValueTypeConvertFlags.NullToValueDefault, opts.ConvertFlags);
            Assert.True(opts.NullToValueDefault);

            opts = new ValueTypeConvertOptions(ValueTypeConvertFlags.None);
            Assert.Equal(ValueTypeConvertFlags.None, opts.ConvertFlags);
            Assert.False(opts.NullToValueDefault);
        }
    }
}
