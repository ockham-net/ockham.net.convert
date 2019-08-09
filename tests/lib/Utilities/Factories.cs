using System.Collections.Generic;
using System.Linq;

namespace Ockham.Data.Tests
{
    public static class Factories
    { 
        public static object[] Set(params object[] values) => values;

        public static IEnumerable<object[]> Values(params object[] values) => values.AsObjectArray();

        public static IEnumerable<object[]> Sets(params object[][] values) => values;

        public static IEnumerable<object[]> Sets<T1, T2>(params (T1, T2)[] values) => values.Select(o => new object[] { o.Item1, o.Item2 });

        public static IEnumerable<object[]> Sets<T1, T2, T3>(params (T1, T2, T3)[] values) => values.Select(o => new object[] { o.Item1, o.Item2, o.Item3 });

    }
}
