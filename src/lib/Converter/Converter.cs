using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;

namespace Ockham.Data
{
    /// <summary>
    /// A wrapper for the static <see cref="Convert"/> conversion functions, which always uses the specific <see cref="ConvertOptions"/> flags
    /// as provided to the constructor
    /// </summary>
    public partial class Converter
    {
        /// <summary>
        /// Singleton instance of <see cref="Converter"/> intialized with <see cref="ConvertOptions.Default"/>
        /// </summary>
        public static Converter Default { get; } = new Converter(ConvertOptions.Default);

        /// <summary>
        /// Create a new <see cref="Converter" /> with the provided <see cref="ConvertOptions"/> settings
        /// </summary> 
        public Converter(ConvertOptions options) { this.Options = options; }

        /// <summary>
        /// Flags controlling conversion logic for this <see cref="Converter"/> instance
        /// </summary>
        public ConvertOptions Options { get; }

        /// <summary>
        /// Force any value to the target type. If the underlying conversion fails for any reason, 
        /// the default value of the target type is returned. This method will never raise a conversion exception. This is
        /// a non-generic method, so the return type is Object. However, the method is
        /// guaranteed to return a valid value of the target type.
        /// </summary>
        /// <param name="value">Any value</param>
        /// <param name="targetType">The <see cref="Type"/> that the input value should be converted to</param>
        /// <returns>The equivalent of the input value in the target type, or the default value of the target type if the input has no equivalent value</returns>
        /// <remarks>If the target type is known at compile-time, use the generic overload <see cref="Converter.Force{T}(Object)"/></remarks> 
        public object Force(object value, Type targetType)
        {
            if (targetType.IsInstanceOfType(value)) return value;
#if INSTRUMENT
            Interlocked.Increment(ref this.Options.CountPastSameType);
#endif
            return Convert.To(value, targetType, this.Options, true, null);
        }

        /// <summary>
        /// Force any value to the target type. If the underlying conversion fails for any reason, the default value of the target
        /// type is returned. This method will never raise a conversion exception.
        /// </summary>
        /// <typeparam name="T">The target type</typeparam>
        /// <param name="value">Any value</param> 
        /// <returns>The equivalent of the input value in the target type, or the default value of the target type if the input has no equivalent value</returns>
        /// <remarks>If the target type is not known at compile-type, use the non-generic overload (<see cref="Converter.Force(Object, Type)"/>)</remarks> 
        public T Force<T>(object value)
        {
            if (value is T) return (T)value;
#if INSTRUMENT
            Interlocked.Increment(ref this.Options.CountPastSameType);
#endif
            return (T)Convert.To(value, typeof(T), this.Options, true, null);
        }

        /// <summary>
        /// Force any value to the target type. If the underlying conversion fails for any reason, the provided default 
        /// value is returned. This method will never raise a conversion exception.
        /// </summary>
        /// <typeparam name="T">The target type</typeparam>
        /// <param name="value">Any value</param>
        /// <param name="defaultValue">The value to return if the input is empty or conversion fails</param>
        /// <returns>The equivalent of the input value in the target type, or the default value of the target type if the input has no equivalent value</returns>
        /// <remarks>If the target type is not known at compile-type, use the non-generic overload <see cref="Converter.Force(Object, Type)"/></remarks> 
        public T Force<T>(object value, T defaultValue)
        {
            if (value is T) return (T)value;
#if INSTRUMENT
            Interlocked.Increment(ref this.Options.CountPastSameType);
#endif
            return (T)(Convert.To(value, typeof(T), this.Options, true, defaultValue));
        }

        /// <summary>
        /// Convert the input value to an equivalent representation in the target type 
        /// </summary>
        /// <typeparam name="T">The Type to which the value should be converted</typeparam>
        /// <param name="value">Any value</param>
        /// <remarks>If the target type is not known at compile-time, use the non-generic overload (<see cref="Converter.To(Object, Type)"/>)</remarks>
        public T To<T>(object value)
        {
            if (value is T) return (T)value;
#if INSTRUMENT
            Interlocked.Increment(ref this.Options.CountPastSameType);
#endif
            return (T)(Convert.To(value, typeof(T), this.Options, false, null));
        }

        /// <summary>
        /// Attempt to convert the input value to an equivalent representation in the target type. This is
        /// a non-generic method, so the return type is Object. However, the method is
        /// guaranteed to return a valid value of the target type.
        /// </summary>
        /// <param name="value">Any value</param>
        /// <param name="targetType">The Type to which the value should be converted</param>
        /// <remarks>If the target type is known at compile-time, use the generic overload (<see cref="Converter.To{T}(object)"/>)</remarks> 
        public object To(object value, Type targetType)
        {
            if (targetType.IsInstanceOfType(value)) return value;
#if INSTRUMENT
            Interlocked.Increment(ref this.Options.CountPastSameType);
#endif
            return Convert.To(value, targetType, this.Options, false, null);
        }

        /// <summary>
        /// Test wether the provided value can be treated as a number
        /// </summary>
        /// <seealso cref="Value.IsNumeric(object, ConvertOptions)"/>
        /// <seealso cref="NumberConvertOptions"/>
        public bool IsNumeric(object value) => Value.IsNumeric(value, this.Options);
    }
}
