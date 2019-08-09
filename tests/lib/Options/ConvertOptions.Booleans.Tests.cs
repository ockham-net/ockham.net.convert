using Xunit;

namespace Ockham.Data.Tests
{
    public class BooleanConvertOptionsTests
    {
        [Fact]
        public void Ctor()
        {
            var opts = new BooleanConvertOptions(new[] { bool.TrueString }, new[] { bool.FalseString });
            Assert.NotNull(opts);
        }

        [Fact]
        public void Default() => TestDefault(BooleanConvertOptions.Default);

        [Fact]
        public void DefaultFromBuilder() => TestDefault(ConvertOptionsBuilder.Default.GetOptions<BooleanConvertOptions>());

        [Fact]
        public void DefaultFromOptions() => TestDefault(ConvertOptions.Default.Booleans);

        private void TestDefault(BooleanConvertOptions opts)
        {
            // Returns a value
            Assert.NotNull(opts);

            // Always the same instance
            Assert.Same(opts, BooleanConvertOptions.Default);

            // Expected configuration
            Assert.Equal(1, opts.TrueStrings.Count);
            Assert.Equal(1, opts.FalseStrings.Count);
            Assert.True(opts.TrueStrings.Contains(bool.TrueString));
            Assert.True(opts.FalseStrings.Contains(bool.FalseString));
        }

        [Fact]
        public void TrueStrings()
        {
            var opts = new BooleanConvertOptions(new[] { bool.TrueString, "T", "yes", "t" }, new[] { bool.FalseString });

            Assert.Equal(3, opts.TrueStrings.Count);        // T and t are de-duplicated
            Assert.True(opts.TrueStrings.Contains("YES"));  // Case insensitive
            Assert.True(opts.TrueStrings.Contains("t"));
            Assert.True(opts.TrueStrings.Contains(bool.TrueString));
        }

        [Fact]
        public void FalseStrings()
        {
            var opts = new BooleanConvertOptions(new[] { bool.TrueString }, new[] { bool.FalseString, "", "f", "no", "NO" });

            Assert.Equal(4, opts.FalseStrings.Count);                // NO and no are de-duplicated
            Assert.True(opts.FalseStrings.Contains("No"));           // Case insensitive
            Assert.True(opts.FalseStrings.Contains("f"));
            Assert.True(opts.FalseStrings.Contains(string.Empty));   // Empty string works
            Assert.True(opts.FalseStrings.Contains(bool.FalseString));
        }

    }
}
