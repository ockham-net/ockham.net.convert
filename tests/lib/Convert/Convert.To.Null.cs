using Ockham.Data.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace Ockham.Data.Tests
{
    using static Factories;

    public class ConvertToNullTests
    {
        public static readonly IEnumerable<object[]> Nulls = Values(null, DBNull.Value);
        public static readonly IEnumerable<object[]> NullsAndEmpty = Values(null, DBNull.Value, string.Empty);
        public static readonly IEnumerable<object[]> NullsAndWhitespace = Values(null, DBNull.Value, string.Empty, "  \r\n  \t");
        public static readonly IEnumerable<object[]> NotEmptyValues = Values("hi", 'A', 3438, new Star(), 0, Guid.Empty);

        public static readonly Converter DefaultConverter = Converter.Default;
        public static readonly Converter EmptyStringConverter = OptionsVariant.EmptyStringAsNull.GetConverter();
        public static readonly Converter WhitespaceConverter = OptionsVariant.WhitespaceAsNull.GetConverter();

        [Theory]
        [MemberData(nameof(Nulls))]
        public static void NullToNull(object value)
        {
            Assert.Null(Convert.ToNull(value));
            Assert.Null(Convert.ToNull(value, true));
            Assert.Null(Convert.ToNull(value, false));

            Assert.Null(DefaultConverter.ToNull(value));
            Assert.Null(EmptyStringConverter.ToNull(value));
            Assert.Null(WhitespaceConverter.ToNull(value));
        }

        [Theory]
        [MemberData(nameof(NullsAndEmpty))]
        public static void EmptyToNull(object value)
        {
            Assert.Null(Convert.ToNull(value, true));
            Assert.Null(Convert.ToNull(value, OptionsVariant.EmptyStringAsNull));
            Assert.Null(Convert.ToNull(value, OptionsVariant.WhitespaceAsNull));

            Assert.Null(EmptyStringConverter.ToNull(value));
            Assert.Null(WhitespaceConverter.ToNull(value));
        }

        [Theory]
        [MemberData(nameof(NullsAndWhitespace))]
        public static void WhitespaceToNull(object value)
        {
            Assert.Null(Convert.ToNull(value, OptionsVariant.WhitespaceAsNull));

            Assert.Null(WhitespaceConverter.ToNull(value));
        }

        [Theory]
        [MemberData(nameof(Nulls))]
        public static void NullToDBNull(object value)
        {
            Assert.Same(DBNull.Value, Convert.ToDBNull(value));
            Assert.Same(DBNull.Value, Convert.ToDBNull(value, true));
            Assert.Same(DBNull.Value, Convert.ToDBNull(value, false));

            Assert.Same(DBNull.Value, DefaultConverter.ToDBNull(value));
            Assert.Same(DBNull.Value, EmptyStringConverter.ToDBNull(value));
            Assert.Same(DBNull.Value, WhitespaceConverter.ToDBNull(value));
        }

        [Theory]
        [MemberData(nameof(NullsAndEmpty))]
        public static void EmptyToDBNull(object value)
        {
            Assert.Same(DBNull.Value, Convert.ToDBNull(value, true));
            Assert.Same(DBNull.Value, Convert.ToDBNull(value, OptionsVariant.EmptyStringAsNull));
            Assert.Same(DBNull.Value, Convert.ToDBNull(value, OptionsVariant.WhitespaceAsNull));

            Assert.Same(DBNull.Value, EmptyStringConverter.ToDBNull(value));
            Assert.Same(DBNull.Value, WhitespaceConverter.ToDBNull(value));
        }

        [Theory]
        [MemberData(nameof(NullsAndWhitespace))]
        public static void WhitespaceToDBNull(object value)
        {
            Assert.Same(DBNull.Value, Convert.ToDBNull(value, OptionsVariant.WhitespaceAsNull));

            Assert.Same(DBNull.Value, WhitespaceConverter.ToDBNull(value));
        }

        [Fact]
        public static void PreserveEmpty()
        {
            Assert.Equal(string.Empty, Convert.ToNull(string.Empty));
            Assert.Equal(string.Empty, Convert.ToNull(string.Empty, false));
            Assert.Equal(string.Empty, Convert.ToNull(string.Empty, ConvertOptions.Default));

            Assert.Equal(string.Empty, DefaultConverter.ToNull(string.Empty));

            Assert.Equal(string.Empty, Convert.ToDBNull(string.Empty));
            Assert.Equal(string.Empty, Convert.ToDBNull(string.Empty, false));
            Assert.Equal(string.Empty, Convert.ToDBNull(string.Empty, ConvertOptions.Default));

            Assert.Equal(string.Empty, DefaultConverter.ToDBNull(string.Empty));
        }

        [Fact]
        public static void PreserveWhitespace()
        {
            string whitespace = "  \r\n ";
            Assert.Equal(whitespace, Convert.ToNull(whitespace));
            Assert.Equal(whitespace, Convert.ToNull(whitespace, false));
            Assert.Equal(whitespace, Convert.ToNull(whitespace, ConvertOptions.Default));

            Assert.Equal(whitespace, DefaultConverter.ToNull(whitespace));
            Assert.Equal(whitespace, EmptyStringConverter.ToNull(whitespace));

            Assert.Equal(whitespace, Convert.ToDBNull(whitespace));
            Assert.Equal(whitespace, Convert.ToDBNull(whitespace, false));
            Assert.Equal(whitespace, Convert.ToDBNull(whitespace, ConvertOptions.Default));

            Assert.Equal(whitespace, DefaultConverter.ToDBNull(whitespace));
            Assert.Equal(whitespace, EmptyStringConverter.ToDBNull(whitespace));
        }

        [Theory]
        [MemberData(nameof(NotEmptyValues))]
        public static void PreserveValues(object value)
        {
            Assert.Equal(value, Convert.ToNull(value));
            Assert.Equal(value, Convert.ToNull(value, false));
            Assert.Equal(value, Convert.ToNull(value, true));
            Assert.Equal(value, Convert.ToNull(value, ConvertOptions.Default));
            Assert.Equal(value, Convert.ToNull(value, OptionsVariant.EmptyStringAsNull));
            Assert.Equal(value, Convert.ToNull(value, OptionsVariant.WhitespaceAsNull));

            Assert.Equal(value, DefaultConverter.ToNull(value));
            Assert.Equal(value, EmptyStringConverter.ToNull(value));
            Assert.Equal(value, WhitespaceConverter.ToNull(value));


            Assert.Equal(value, Convert.ToDBNull(value));
            Assert.Equal(value, Convert.ToDBNull(value, false));
            Assert.Equal(value, Convert.ToDBNull(value, true));
            Assert.Equal(value, Convert.ToDBNull(value, ConvertOptions.Default));
            Assert.Equal(value, Convert.ToDBNull(value, OptionsVariant.EmptyStringAsNull));
            Assert.Equal(value, Convert.ToDBNull(value, OptionsVariant.WhitespaceAsNull));

            Assert.Equal(value, DefaultConverter.ToDBNull(value));
            Assert.Equal(value, EmptyStringConverter.ToDBNull(value));
            Assert.Equal(value, WhitespaceConverter.ToDBNull(value));
        }

        [Theory]
        [MemberData(nameof(Nulls))]
        public static void NullIsNull(object value)
        {
            Assert.True(DefaultConverter.IsNull(value));
            Assert.True(EmptyStringConverter.IsNull(value));
            Assert.True(WhitespaceConverter.IsNull(value));
        }

        [Theory]
        [InlineData("")]
        public static void EmptyIsNull(object value)
        {
            Assert.False(DefaultConverter.IsNull(value));

            Assert.True(EmptyStringConverter.IsNull(value));
            Assert.True(WhitespaceConverter.IsNull(value));
        }

        [Theory]
        [InlineData("  \r\n \t ")]
        public static void WhitespaceIsNull(object value)
        {
            Assert.False(DefaultConverter.IsNull(value));
            Assert.False(EmptyStringConverter.IsNull(value));

            Assert.True(WhitespaceConverter.IsNull(value));
        }
    }
}
