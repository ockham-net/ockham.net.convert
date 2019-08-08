using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Ockham.Data.Tests.Fixtures
{
    public static class TestPrimitives
    {
        public static IEnumerable<object[]> Zeros => (new object[] { 0, 0m, 0.0f, 0.0d, (byte)0, (long)0, (uint)0 }).AsObjectArray();

        public static IEnumerable<object[]> Unconvertible => (new object[]
        {
            "Hello",
            DateTime.UtcNow,
            DateTime.UtcNow.TimeOfDay,
            new object(),
            (Func<int>)(() => 42)
        }).AsObjectArray();

        public static IEnumerable<object[]> WholeNumeric => Zeros.Concat((new object[] {
            23409234234,
            3242.00,
            168124m,
            "0",
            "23",
            "42342"
        }).AsObjectArray());

        public static IEnumerable<object[]> DecimalStrings => (new object[] {
            "3423.2423",
            "432.3432",
            "0.006565",
            "3.209213",
            "001.0"
        }).AsObjectArray();
    }
}
