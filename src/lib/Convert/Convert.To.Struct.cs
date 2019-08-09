using System;
using System.Collections.Generic;
using System.Threading;

namespace Ockham.Data
{
    public static partial class Convert
    {

        /// <summary>
        /// This method avoids boxing or lifting value types, but requires compile-time knowledge of target type. 
        /// </summary> 
        internal static T ToStruct<T>(object value, ConvertOptions options) where T : struct
        {
            // Do NOT check for same type here. That should already be done by calling functions

            if (
#if INSTRUMENT
                // Technically we're incrementing before the null check...but it has the desired effect
                ((Interlocked.Increment(ref options.CountPastNullRef) > 0) && (null == value))
#else
                (null == value)
#endif
                || Value.IsNull(value, options))
            {
                if (options.NullToValueDefault)
                {
                    return default(T);
                }
                else
                {
                    throw new InvalidCastException($"Cannot convert empty value {FormatValue(value)} to value type {FormatType(typeof(T))}");
                }
            }

#if INSTRUMENT
            // Allowing horrible inspection if *implementation* because performance here is so important
            Interlocked.Increment(ref options.CountPastEmptyValue);
#endif

            return (T)ToStructValue(value, typeof(T), options, false, null);
        }

        /*
       internal static T? ToNullable<T>(object value, ConvertOptions options, bool ignoreError, T? valueOnError, IReadOnlyDictionary<Type, ConverterDelegate> converters) where T : struct
       {
           if (value == null) return null;

#if INSTRUMENT
           // Allowing horrible inspection if *implementation* because performance here is so important
           Interlocked.Increment(ref options.CountPastNullRef);
#endif

           // Do NOT check for same type here. That should already be done by calling functions

           if (Value.IsNull(value, options)) return null;

#if INSTRUMENT
           Interlocked.Increment(ref options.CountPastEmptyValue);
#endif

           return (T)ToStructValue(value, typeof(T), options, ignoreError, valueOnError, converters);
       }
       */


    }
}
