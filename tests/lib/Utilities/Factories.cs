using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Ockham.Data.Tests
{
    [ExcludeFromCodeCoverage]
    public static class Factories
    {
        public static object[] Pair(object item1, object item2)
        {
            return new object[] { item1, item2 };
        }

        public static IEnumerable<object[]> Values(params object[] values) => values.AsObjectArray();

        public static IEnumerable<object[]> Sets(params object[][] values) => values;

        public static IEnumerable<Func<object>> Funcs(params Func<object>[] tests) => tests;


        public static IEnumerable<object[]> Sets<T1, T2>(params (T1, T2)[] values) => values.Select(o => new object[] { o.Item1, o.Item2 });
        public static IEnumerable<object[]> Sets<T1, T2, T3>(params (T1, T2, T3)[] values) => values.Select(o => new object[] { o.Item1, o.Item2, o.Item3 });
        public static IEnumerable<object[]> Sets<T1, T2, T3, T4>(params (T1, T2, T3, T4)[] values) => values.Select(o => new object[] { o.Item1, o.Item2, o.Item3, o.Item4 });


        public static object[] Set(params object[] values) => values;

        public static object[] Set<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4) => new object[] { arg1, arg2, arg3, arg4 };
    }
}
