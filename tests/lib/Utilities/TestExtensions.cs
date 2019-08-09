using System;
using System.Collections.Generic;
using System.Linq;

namespace Ockham.Data.Tests
{
    public static class TestExtensions
    {
        public static Action AsAction(this Func<object> func, bool ignoreError = false)
        {
            if (ignoreError)
            {
                return () => { try { func(); } catch { } };
            }
            else
            {
                return () => func();
            }
        }

        public static IEnumerable<object[]> AsObjectArray<T>(this IEnumerable<T> source)
        {
            return source.Select(x => new object[] { x });
        }

        public static bool HasBitFlag<T>(this T value, T flags) where T : struct, IComparable, IConvertible, IFormattable
        {
            return ((Enum)(object)value).HasFlag((Enum)(object)flags);
        }

        public static ConvertOptionsBuilder GetBuilder(this ConvertOptions options)
        {
            return ConvertOptionsBuilder.FromConvertOptions(options);
        }

        private static readonly Dictionary<ConvertOptions, Converter> _converters = new Dictionary<ConvertOptions, Converter>();

        public static Converter GetConverter(this ConvertOptions options)
        {
            if (_converters.TryGetValue(options, out Converter converter)) return converter;
            lock (_converters)
            {
                if (_converters.TryGetValue(options, out converter)) return converter;
                return (_converters[options] = new Converter(options));
            }
        }
    }
}
