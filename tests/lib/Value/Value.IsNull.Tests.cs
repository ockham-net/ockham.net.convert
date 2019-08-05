using Ockham.Data.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Ockham.Data.Tests
{
    public partial class ValueTests
    {
        [Fact]
        public void IsNull_Strict()
        {
            Assert.True(Value.IsNull(null));
            Assert.True(Value.IsNull(DBNull.Value));

            TestStruct testStruct = default;

            Assert.False(Value.IsNull(""));
            Assert.False(Value.IsNull("  \t  \r \n "));
            Assert.False(Value.IsNull(0));
            Assert.False(Value.IsNull(testStruct));
        }

        [Fact]
        public void IsNull_EmptyString()
        {
            Assert.True(Value.IsNull(null, true));
            Assert.True(Value.IsNull(DBNull.Value, true));
            Assert.True(Value.IsNull("", true));

            TestStruct testStruct = default;

            Assert.False(Value.IsNull("  \t  \r \n ", true));
            Assert.False(Value.IsNull(0, true));
            Assert.False(Value.IsNull(testStruct, true));
        }

        [Fact]
        public void IsNull_EmptyStringOptions()
        {
            var opts = ConvertOptionsBuilder.Default.WithStringOptions(StringAsNullOption.EmptyString, TrimStringFlags.TrimAll).Options;

            Assert.True(Value.IsNull(null, opts));
            Assert.True(Value.IsNull(DBNull.Value, opts));
            Assert.True(Value.IsNull("", opts));

            TestStruct testStruct = default;

            Assert.False(Value.IsNull("  \t  \r \n ", opts));
            Assert.False(Value.IsNull(0, opts));
            Assert.False(Value.IsNull(testStruct, opts));
        }

        [Fact]
        public void IsNull_WhitespaceOptions()
        {
            var opts = ConvertOptionsBuilder.Default.WithStringOptions(StringAsNullOption.Whitespace, TrimStringFlags.TrimAll).Options;

            Assert.True(Value.IsNull(null, opts));
            Assert.True(Value.IsNull(DBNull.Value, opts));
            Assert.True(Value.IsNull("", opts));
            Assert.True(Value.IsNull("  \t  \r \n ", opts));

            TestStruct testStruct = default;

            Assert.False(Value.IsNull(0, opts));
            Assert.False(Value.IsNull(testStruct, opts));
        }

        [Fact]
        public void IsNullOrEmpty()
        {
            Assert.True(Value.IsNullOrEmpty(null));
            Assert.True(Value.IsNullOrEmpty(DBNull.Value));
            Assert.True(Value.IsNullOrEmpty(""));

            TestStruct testStruct = default;

            Assert.False(Value.IsNullOrEmpty("  \t  \r \n "));
            Assert.False(Value.IsNullOrEmpty(0));
            Assert.False(Value.IsNullOrEmpty(testStruct));
        }

        [Fact]
        public void IsNullOrWhitespace()
        {
            Assert.True(Value.IsNullOrWhitespace(null));
            Assert.True(Value.IsNullOrWhitespace(DBNull.Value));
            Assert.True(Value.IsNullOrWhitespace(""));
            Assert.True(Value.IsNullOrWhitespace("  \t  \r \n "));

            TestStruct testStruct = default;

            Assert.False(Value.IsNullOrWhitespace(0));
            Assert.False(Value.IsNullOrWhitespace(testStruct));
        }
    }
}
