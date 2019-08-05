using Xunit;
using Ockham.Data.Tests.Fixtures;
using System.Diagnostics.CodeAnalysis;

namespace Ockham.Data.Tests
{
    [ExcludeFromCodeCoverage]
    public class ConvertOptionsTests
    {
        [Fact]
        public void Ctor_Definite()
        {
            var opts = new ConvertOptions(
                new BooleanConvertOptions(new[] { bool.TrueString }, new[] { bool.FalseString }),
                new EnumConvertOptions(UndefinedValueOption.Throw, UndefinedValueOption.Throw),
                new NumberConvertOptions(ParseNumericStringFlags.None),
                new StringConvertOptions(StringAsNullOption.NullReference, TrimStringFlags.None),
                new ValueTypeConvertOptions(ValueTypeConvertFlags.None)
            );

            Assert.NotNull(opts);
            Assert.NotNull(opts.Booleans);
            Assert.NotNull(opts.Enums);
            Assert.NotNull(opts.Numbers);
            Assert.NotNull(opts.Strings);
            Assert.NotNull(opts.ValueTypes);
        }

        [Fact]
        public void Ctor_Params()
        {
            var opts = new ConvertOptions(
                new StringConvertOptions(StringAsNullOption.NullReference, TrimStringFlags.None),
                new ValueTypeConvertOptions(ValueTypeConvertFlags.None),
                new ComplexNumberConvertOptions(ComplexNumberElement.Real),
                new BooleanConvertOptions(new[] { bool.TrueString }, new[] { bool.FalseString }),
                new EnumConvertOptions(UndefinedValueOption.Throw, UndefinedValueOption.Throw),
                new NumberConvertOptions(ParseNumericStringFlags.None)
            );

            Assert.NotNull(opts);
            Assert.NotNull(opts.Booleans);
            Assert.NotNull(opts.Enums);
            Assert.NotNull(opts.Numbers);
            Assert.NotNull(opts.Strings);
            Assert.NotNull(opts.ValueTypes);
        }

        [Fact]
        public void Ctor_Enumerable()
        {
            var opts = new ConvertOptions(new OptionSet[] {
                new StringConvertOptions(StringAsNullOption.NullReference, TrimStringFlags.None),
                new ValueTypeConvertOptions(ValueTypeConvertFlags.None),
                new BooleanConvertOptions(new[] { bool.TrueString }, new[] { bool.FalseString }),
                new EnumConvertOptions(UndefinedValueOption.Throw, UndefinedValueOption.Throw),
                new NumberConvertOptions(ParseNumericStringFlags.None),
                new ComplexNumberConvertOptions(ComplexNumberElement.Real)
            });

            Assert.NotNull(opts);
            Assert.NotNull(opts.Booleans);
            Assert.NotNull(opts.Enums);
            Assert.NotNull(opts.Numbers);
            Assert.NotNull(opts.Strings);
            Assert.NotNull(opts.ValueTypes);
        }

        [Fact]
        public void Flatten()
        {
            var opts = ConvertOptions.Default;

            // Defaults
            var flattenedOptions = opts.FlattenedOptions;
            Assert.False(flattenedOptions.HasBitFlag(FlattenedOptions.IgnoreEnumNames));
            Assert.False(flattenedOptions.HasBitFlag(FlattenedOptions.IgnoreEnumValues));
            Assert.True(flattenedOptions.HasBitFlag(FlattenedOptions.CoerceEnumValues));

            Assert.False(flattenedOptions.HasBitFlag(FlattenedOptions.ParseHex));
            Assert.False(flattenedOptions.HasBitFlag(FlattenedOptions.ParseOctal));
            Assert.False(flattenedOptions.HasBitFlag(FlattenedOptions.ParseBinary));
            Assert.False(flattenedOptions.HasBitFlag(FlattenedOptions.AllowDigitSeparator));

            Assert.True(flattenedOptions.HasBitFlag(FlattenedOptions.TrimStart));
            Assert.True(flattenedOptions.HasBitFlag(FlattenedOptions.TrimEnd));
            Assert.True(flattenedOptions.HasBitFlag(FlattenedOptions.TrimAll));

            Assert.False(flattenedOptions.HasBitFlag(FlattenedOptions.EmptyStringAsNull));
            Assert.False(flattenedOptions.HasBitFlag(FlattenedOptions.WhitespaceAsNull));

            Assert.False(flattenedOptions.HasBitFlag(FlattenedOptions.NullToValueDefault));

            opts = ConvertOptionsBuilder.Empty
                .WithOptions(BooleanConvertOptions.Default)
                .WithEnumOptions(UndefinedValueOption.Ignore, UndefinedValueOption.Ignore)
                .WithNumberOptions(ParseNumericStringFlags.HexString | ParseNumericStringFlags.OctalString | ParseNumericStringFlags.BinaryString | ParseNumericStringFlags.AllowDigitSeparator)
                .WithStringOptions(StringAsNullOption.Whitespace, TrimStringFlags.None)
                .WithValueTypeOptions(ValueTypeConvertFlags.NullToValueDefault)
                .Options;

            flattenedOptions = opts.FlattenedOptions;
            Assert.True(flattenedOptions.HasBitFlag(FlattenedOptions.IgnoreEnumNames));
            Assert.True(flattenedOptions.HasBitFlag(FlattenedOptions.IgnoreEnumValues));
            Assert.False(flattenedOptions.HasBitFlag(FlattenedOptions.CoerceEnumValues));

            Assert.True(flattenedOptions.HasBitFlag(FlattenedOptions.ParseHex));
            Assert.True(flattenedOptions.HasBitFlag(FlattenedOptions.ParseOctal));
            Assert.True(flattenedOptions.HasBitFlag(FlattenedOptions.ParseBinary));
            Assert.True(flattenedOptions.HasBitFlag(FlattenedOptions.AllowDigitSeparator));

            Assert.False(flattenedOptions.HasBitFlag(FlattenedOptions.TrimStart));
            Assert.False(flattenedOptions.HasBitFlag(FlattenedOptions.TrimEnd));
            Assert.False(flattenedOptions.HasBitFlag(FlattenedOptions.TrimAll));

            Assert.True(flattenedOptions.HasBitFlag(FlattenedOptions.EmptyStringAsNull));
            Assert.True(flattenedOptions.HasBitFlag(FlattenedOptions.WhitespaceAsNull));

            Assert.True(flattenedOptions.HasBitFlag(FlattenedOptions.NullToValueDefault));
        }

        [Fact]
        public void StringToNull()
        {
            var opts = ConvertOptions.Default;
            Assert.False(opts.StringToNull);

            opts = ConvertOptionsBuilder.Default.WithStringOptions(StringAsNullOption.EmptyString, TrimStringFlags.TrimAll).Options;
            Assert.True(opts.StringToNull);

            opts = ConvertOptionsBuilder.Default.WithStringOptions(StringAsNullOption.Whitespace, TrimStringFlags.TrimAll).Options;
            Assert.True(opts.StringToNull);
        }

        [Fact]
        public void HasOptions()
        {
            var opts = ConvertOptions.Default;
            Assert.True(opts.HasOptions<BooleanConvertOptions>());
            Assert.False(opts.HasOptions<ComplexNumberConvertOptions>());
        }

        [Fact]
        public void TryGetOptions()
        {
            var opts = ConvertOptions.Default;
            Assert.True(opts.TryGetOptions(out BooleanConvertOptions boolOpts));
            Assert.NotNull(boolOpts);

            Assert.False(opts.TryGetOptions(out ComplexNumberConvertOptions complexOpts));
            Assert.Null(complexOpts);
        }
    }
}
