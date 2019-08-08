using Ockham.Data.Tests.Fixtures;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace Ockham.Data.Tests
{
    [ExcludeFromCodeCoverage]
    public class ConvertOptionsBuilderTests
    {
        [Fact]
        public void Ctor_Enumerable()
        {
            var complexOptions = new ComplexNumberConvertOptions(ComplexNumberElement.Real);
            var options = new OptionSet[]
            {
                BooleanConvertOptions.Default,
                EnumConvertOptions.Default,
                complexOptions
            };

            var builder = new ConvertOptionsBuilder(options, null);
            Assert.NotNull(builder);
            Assert.Contains(BooleanConvertOptions.Default, builder);
            Assert.Contains(EnumConvertOptions.Default, builder);
            Assert.Contains(complexOptions, builder);
            Assert.Equal(3, builder.Count());
        }

        [Fact]
        public void Ctor_ReplaceOptions()
        {
            // Second ComplexNumberConvertOptions should replace the first
            var complexOptions1 = new ComplexNumberConvertOptions(ComplexNumberElement.Real);
            var complexOptions2 = new ComplexNumberConvertOptions(ComplexNumberElement.Imaginary);
            var options = new OptionSet[]
            {
                BooleanConvertOptions.Default,
                EnumConvertOptions.Default,
                complexOptions1
            };

            var builder1 = new ConvertOptionsBuilder(options, null);
            var builder = new ConvertOptionsBuilder(builder1, complexOptions2);
            Assert.NotNull(builder);
            Assert.Contains(BooleanConvertOptions.Default, builder);
            Assert.Contains(EnumConvertOptions.Default, builder);
            Assert.DoesNotContain(complexOptions1, builder);
            Assert.Contains(complexOptions2, builder);
            Assert.Equal(3, builder.Count());
        }


        [Fact]
        public void Empty()
        {
            var builder = ConvertOptionsBuilder.Empty;
            Assert.NotNull(builder);
            Assert.Empty(builder);
        }

        [Fact]
        public void Default()
        {
            var builder = ConvertOptionsBuilder.Default;
            Assert.NotNull(builder);

            Assert.Same(BooleanConvertOptions.Default, builder.GetOptions<BooleanConvertOptions>());
            Assert.Same(EnumConvertOptions.Default, builder.GetOptions<EnumConvertOptions>());
            Assert.Same(NumberConvertOptions.Default, builder.GetOptions<NumberConvertOptions>());
            Assert.Same(StringConvertOptions.Default, builder.GetOptions<StringConvertOptions>());
            Assert.Same(ValueTypeConvertOptions.Default, builder.GetOptions<ValueTypeConvertOptions>());
        }

        [Fact]
        public void Options()
        {
            var builder = ConvertOptionsBuilder.Empty;

            var options = builder
                .WithOptions(BooleanConvertOptions.Default)
                .WithOptions(EnumConvertOptions.Default)
                .WithOptions(NumberConvertOptions.Default)
                .WithOptions(StringConvertOptions.Default)
                .WithOptions(ValueTypeConvertOptions.Default)
                .Options;

            Assert.NotNull(options);
        }

        [Fact]
        public void WithOptions()
        {
            var complexOptions = new ComplexNumberConvertOptions(ComplexNumberElement.Real);
            var builder = ConvertOptionsBuilder.Empty;
            var newBuilder = builder.WithOptions(complexOptions);

            // Returns new instance
            Assert.NotSame(builder, newBuilder);

            // Original builder is not modified
            Assert.Empty(builder);

            // New builder does contain the provided options
            Assert.NotEmpty(newBuilder);
            Assert.Contains(complexOptions, newBuilder);
        }

        [Fact]
        public void WithBoolOptions()
        {
            var builder = ConvertOptionsBuilder.Default;
            var newBuilder = builder.WithBoolOptions(new[] { "t", "y" }, new[] { "n", "f" });

            // Returns new instance
            Assert.NotSame(builder, newBuilder);

            // New options set instance
            var newBoolOptions = newBuilder.GetOptions<BooleanConvertOptions>();
            Assert.NotNull(newBoolOptions);
            Assert.NotSame(BooleanConvertOptions.Default, newBoolOptions);

            // Verify settings were set correctly
            Assert.Equal(2, newBoolOptions.TrueStrings.Count);
            Assert.Equal(2, newBoolOptions.FalseStrings.Count);

            Assert.True(newBoolOptions.TrueStrings.Contains("T"));
            Assert.True(newBoolOptions.TrueStrings.Contains("Y"));
            Assert.False(newBoolOptions.TrueStrings.Contains(bool.TrueString));

            Assert.True(newBoolOptions.FalseStrings.Contains("F"));
            Assert.True(newBoolOptions.FalseStrings.Contains("N"));
            Assert.False(newBoolOptions.FalseStrings.Contains(bool.FalseString));
        }

        [Fact]
        public void WithTrueStrings()
        {
            var builder = ConvertOptionsBuilder.Default;
            var newBuilder = builder.WithTrueStrings(new[] { "t", "y" });

            // Returns new instance
            Assert.NotSame(builder, newBuilder);

            // New options set instance
            var defaultBoolOptions = BooleanConvertOptions.Default;
            var newBoolOptions = newBuilder.GetOptions<BooleanConvertOptions>();
            Assert.NotNull(newBoolOptions);
            Assert.NotSame(BooleanConvertOptions.Default, newBoolOptions);

            // Verify settings were set correctly
            Assert.Equal(2, newBoolOptions.TrueStrings.Count);
            Assert.Equal(defaultBoolOptions.FalseStrings.Count, newBoolOptions.FalseStrings.Count);

            Assert.True(newBoolOptions.TrueStrings.Contains("T"));
            Assert.True(newBoolOptions.TrueStrings.Contains("Y"));
            Assert.False(newBoolOptions.TrueStrings.Contains(bool.TrueString));

            // False strings are still default
            Assert.True(newBoolOptions.FalseStrings.Contains(bool.FalseString));
        }

        [Fact]
        public void WithFalseStrings()
        {
            var builder = ConvertOptionsBuilder.Default;
            var newBuilder = builder.WithFalseStrings(new[] { "n", "f" });

            // Returns new instance
            Assert.NotSame(builder, newBuilder);

            // New options set instance
            var defaultBoolOptions = BooleanConvertOptions.Default;
            var newBoolOptions = newBuilder.GetOptions<BooleanConvertOptions>();
            Assert.NotNull(newBoolOptions);
            Assert.NotSame(BooleanConvertOptions.Default, newBoolOptions);

            // Verify settings were set correctly
            Assert.Equal(defaultBoolOptions.TrueStrings.Count, newBoolOptions.TrueStrings.Count);
            Assert.Equal(2, newBoolOptions.FalseStrings.Count);

            // True strings are still default
            Assert.True(newBoolOptions.TrueStrings.Contains(bool.TrueString));

            Assert.True(newBoolOptions.FalseStrings.Contains("F"));
            Assert.True(newBoolOptions.FalseStrings.Contains("N"));
            Assert.False(newBoolOptions.FalseStrings.Contains(bool.FalseString));
        }

        [Fact]
        public void WithEnumOptions()
        {
            var builder = ConvertOptionsBuilder.Default;
            var newBuilder = builder.WithEnumOptions(UndefinedValueOption.Ignore, UndefinedValueOption.Ignore);

            // Returns new instance
            Assert.NotSame(builder, newBuilder);

            // New options set instance
            var newEnumOptions = newBuilder.GetOptions<EnumConvertOptions>();
            Assert.NotNull(newEnumOptions);
            Assert.NotSame(EnumConvertOptions.Default, newEnumOptions);

            // Verify settings were set correctly
            Assert.Equal(UndefinedValueOption.Ignore, newEnumOptions.UndefinedNames);
            Assert.Equal(UndefinedValueOption.Ignore, newEnumOptions.UndefinedValues);
        }

        [Fact]
        public void WithNumberOptions()
        {
            var builder = ConvertOptionsBuilder.Default;
            var newBuilder = builder.WithNumberOptions(ParseNumericStringFlags.AllowDigitSeparator | ParseNumericStringFlags.OctalString);

            // Returns new instance
            Assert.NotSame(builder, newBuilder);

            // New options set instance
            var newNumberOptions = newBuilder.GetOptions<NumberConvertOptions>();
            Assert.NotNull(newNumberOptions);
            Assert.NotSame(NumberConvertOptions.Default, newNumberOptions);

            // Verify settings were set correctly
            Assert.Equal(ParseNumericStringFlags.AllowDigitSeparator | ParseNumericStringFlags.OctalString, newNumberOptions.ParseFlags);
        }

        [Fact]
        public void WithStringOptions()
        {
            var builder = ConvertOptionsBuilder.Default;
            var newBuilder = builder.WithStringOptions(StringAsNullOption.Whitespace, TrimStringFlags.None);

            // Returns new instance
            Assert.NotSame(builder, newBuilder);

            // New options set instance
            var newStringOptions = newBuilder.GetOptions<StringConvertOptions>();
            Assert.NotNull(newStringOptions);
            Assert.NotSame(StringConvertOptions.Default, newStringOptions);

            // Verify settings were set correctly
            Assert.Equal(StringAsNullOption.Whitespace, newStringOptions.AsNullOption);
            Assert.Equal(TrimStringFlags.None, newStringOptions.TrimFlags);
        }

        [Fact]
        public void WithValueTypeOptions()
        {
            var builder = ConvertOptionsBuilder.Default;
            var newBuilder = builder.WithValueTypeOptions(ValueTypeConvertFlags.NullToValueDefault);

            // Returns new instance
            Assert.NotSame(builder, newBuilder);

            // New options set instance
            var newValOptions = newBuilder.GetOptions<ValueTypeConvertOptions>();
            Assert.NotNull(newValOptions);
            Assert.NotSame(ValueTypeConvertOptions.Default, newValOptions);

            // Verify settings were set correctly
            Assert.Equal(ValueTypeConvertFlags.NullToValueDefault, newValOptions.ConvertFlags);
        }


        [Fact]
        public void FromConvertOptions()
        {
            var stringOpts = new StringConvertOptions(StringAsNullOption.NullReference, TrimStringFlags.None);
            var valOpts = new ValueTypeConvertOptions(ValueTypeConvertFlags.None);
            var complexOpts = new ComplexNumberConvertOptions(ComplexNumberElement.Real);
            var boolOpts = new BooleanConvertOptions(new[] { bool.TrueString }, new[] { bool.FalseString });
            var enumOpts = new EnumConvertOptions(UndefinedValueOption.Throw, UndefinedValueOption.Throw);
            var numberOpts = new NumberConvertOptions(ParseNumericStringFlags.None);

            var opts = new ConvertOptions(new OptionSet[] { stringOpts, valOpts, complexOpts, boolOpts, enumOpts, numberOpts });

            var builder = ConvertOptionsBuilder.FromConvertOptions(opts);

            Assert.Equal(6, builder.Count());
            Assert.Contains(stringOpts, builder);
            Assert.Contains(valOpts, builder);
            Assert.Contains(complexOpts, builder);
            Assert.Contains(boolOpts, builder);
            Assert.Contains(enumOpts, builder);
            Assert.Contains(numberOpts, builder);
        }
    }
}
