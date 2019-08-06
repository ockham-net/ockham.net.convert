using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Ockham.Data.Tests
{
    [ExcludeFromCodeCoverage]
    public class StringConvertOptionsTests
    {
        [Fact]
        public void Ctor()
        {
            var opts = new StringConvertOptions(StringAsNullOption.EmptyString, TrimStringFlags.TrimStart);
            Assert.NotNull(opts);
        }

        [Fact]
        public void Default() => TestDefault(StringConvertOptions.Default);

        [Fact]
        public void DefaultFromBuilder() => TestDefault(ConvertOptionsBuilder.Default.GetOptions<StringConvertOptions>());

        [Fact]
        public void DefaultFromOptions() => TestDefault(ConvertOptions.Default.Strings);

        private void TestDefault(StringConvertOptions opts)
        {
            // Returns a value
            Assert.NotNull(opts);

            // Always the same instance
            Assert.Same(opts, StringConvertOptions.Default);

            // Expected configuration
            Assert.Equal(StringAsNullOption.NullReference, opts.AsNullOption);
            Assert.Equal(TrimStringFlags.TrimAll, opts.TrimFlags);
        }

        [Fact]
        public void AsNullOption()
        {
            var opts = new StringConvertOptions(StringAsNullOption.EmptyString, TrimStringFlags.None);
            Assert.Equal(StringAsNullOption.EmptyString, opts.AsNullOption);

            Assert.True(opts.EmptyStringAsNull);
            Assert.False(opts.WhitespaceAsNull);

            opts = new StringConvertOptions(StringAsNullOption.NullReference, TrimStringFlags.None);
            Assert.Equal(StringAsNullOption.NullReference, opts.AsNullOption);

            Assert.False(opts.EmptyStringAsNull);
            Assert.False(opts.WhitespaceAsNull);

            opts = new StringConvertOptions(StringAsNullOption.Whitespace, TrimStringFlags.None);
            Assert.Equal(StringAsNullOption.Whitespace, opts.AsNullOption);

            Assert.True(opts.EmptyStringAsNull);
            Assert.True(opts.WhitespaceAsNull);
        }


        [Fact]
        public void TrimFlags()
        {
            var opts = new StringConvertOptions(StringAsNullOption.NullReference, TrimStringFlags.TrimStart);
            Assert.Equal(TrimStringFlags.TrimStart, opts.TrimFlags);

            Assert.False(opts.TrimNone);
            Assert.True(opts.TrimStart);
            Assert.False(opts.TrimEnd);
            Assert.False(opts.TrimAll);


            opts = new StringConvertOptions(StringAsNullOption.NullReference, TrimStringFlags.TrimEnd);
            Assert.Equal(TrimStringFlags.TrimEnd, opts.TrimFlags);

            Assert.False(opts.TrimNone);
            Assert.False(opts.TrimStart);
            Assert.True(opts.TrimEnd);
            Assert.False(opts.TrimAll);


            opts = new StringConvertOptions(StringAsNullOption.NullReference, TrimStringFlags.None);
            Assert.Equal(TrimStringFlags.None, opts.TrimFlags);

            Assert.True(opts.TrimNone);
            Assert.False(opts.TrimStart);
            Assert.False(opts.TrimEnd);
            Assert.False(opts.TrimAll);


            opts = new StringConvertOptions(StringAsNullOption.NullReference, TrimStringFlags.TrimAll);
            Assert.Equal(TrimStringFlags.TrimAll, opts.TrimFlags);

            Assert.False(opts.TrimNone);
            Assert.True(opts.TrimStart);
            Assert.True(opts.TrimEnd);
            Assert.True(opts.TrimAll);
        }
    }
}
