using Ockham.Data.Tests.Fixtures;
using System;
using Xunit;

namespace Ockham.Data.Tests
{
    public partial class ValueTests
    {
        [Fact]
        public void IsDefault()
        {
            TestStruct testStruct = default;
            Point3D point = default;
            DBNull dBNull = default;

            Assert.True(Value.IsDefault(null));
            Assert.True(Value.IsDefault(0));
            Assert.True(Value.IsDefault(testStruct));
            Assert.True(Value.IsDefault(point));
            Assert.True(Value.IsDefault(dBNull));

            Assert.False(Value.IsDefault(DBNull.Value));
            Assert.False(Value.IsDefault(""));
            Assert.False(Value.IsDefault("  \t  \r \n "));
            Assert.False(Value.IsDefault(1));
            Assert.False(Value.IsDefault(new object()));
        }
    }
}
