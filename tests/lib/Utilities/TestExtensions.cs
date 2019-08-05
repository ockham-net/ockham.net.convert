using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Ockham.Data.Tests
{
    [ExcludeFromCodeCoverage]
    public static class TestExtensions
    {
        public static IEnumerable<object[]> AsObjectArray<T>(this IEnumerable<T> source)
        {
            return source.Select(x => new object[] { x });
        }

        public static bool HasBitFlag<T>(this T value, T flags) where T : struct, IComparable, IConvertible, IFormattable
        {
            return ((Enum)(object)value).HasFlag((Enum)(object)flags);
        }

    }

}
