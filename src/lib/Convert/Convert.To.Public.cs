using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using VBConvert = Microsoft.VisualBasic.CompilerServices.Conversions;
using System.Threading;

namespace Ockham.Data
{
    public static partial class Convert
    {
        /// <summary>
        /// Attempt to convert the input value to an equilavant representation in the target type, 
        /// using <see cref="Ockham.Data.ConvertOptions.Default"/>
        /// </summary>
        /// <typeparam name="T">The Type to which the value should be converted</typeparam>
        /// <param name="value">Any value</param>
        /// <remarks>If the target type is not known at compile time, use the non-generic overload of Convert.To (<see cref="Convert.To(Object, Type)"/>)</remarks>
        public static T To<T>(object value)
        {
            if (value is T) return (T)value;
#if INSTRUMENT
            Interlocked.Increment(ref ConvertOptions.Default.CountPastSameType);
#endif
            return (T)To(value, typeof(T), ConvertOptions.Default, false, null, Converter.Default.Converters);
        }
        /// <summary>
        /// Attempt to convert the input value to an equilavant representation in the target type. 
        /// </summary>
        /// <typeparam name="T">The Type to which the value should be converted</typeparam>
        /// <param name="value">Any value</param>
        /// <param name="options">See <see cref="Ockham.Data.ConvertOptions"/></param>
        /// <remarks>If the target type is not known at compile time, use the non-generic overload of Convert.To (<see cref="Convert.To(object, Type, ConvertOptions)"/>)</remarks> 
        public static T To<T>(object value, ConvertOptions options)
        {
            if (value is T) return (T)value;
#if INSTRUMENT
            Interlocked.Increment(ref options.CountPastSameType);
#endif
            return (T)To(value, typeof(T), options, false, null, Converter.Default.Converters);
        }

        /// <summary>
        /// Attempt to convert the input value to an equilavant representation in the target type, 
        /// using <see cref="ConvertOptions.Default"/>
        /// </summary>
        /// <param name="value">Any value</param>
        /// <param name="targetType">The Type to which the value should be converted</param>
        /// <remarks>If the target type is known at compile time, use the generic overload of Convert.To (<see cref="Convert.To{T}(object)"/>)</remarks> 
        public static object To(object value, Type targetType)
        {
            if (targetType.IsInstanceOfType(value)) return value;
#if INSTRUMENT
            Interlocked.Increment(ref ConvertOptions.Default.CountPastSameType);
#endif
            return To(value, targetType, ConvertOptions.Default, false, null, Converter.Default.Converters);
        }

        /// <summary>
        /// Attempt to convert the input value to an equilavant representation in the target type. 
        /// </summary>
        /// <param name="value">Any value</param>
        /// <param name="targetType">The Type to which the value should be converted</param>
        /// <param name="options">See <see cref="ConvertOptions"/></param>
        /// <remarks>If the target type is known at compile time, use the generic overload of Convert.To (<see cref="Convert.To{T}(object, ConvertOptions)"/>)</remarks>
        public static object To(object value, Type targetType, ConvertOptions options)
        {
            if (targetType.IsInstanceOfType(value)) return value;
#if INSTRUMENT
            Interlocked.Increment(ref options.CountPastSameType);
#endif
            return To(value, targetType, options, false, null, Converter.Default.Converters);
        }

        /// <summary>
        /// Force any value to the target type. If the input is an empty value
        /// or cannot be converted to the target type, the default value of the target
        /// type is returned. This method will never raise a conversion exception.
        /// </summary>
        /// <typeparam name="T">The target type</typeparam>
        /// <param name="value">Any value</param> 
        /// <returns>The equivalent of the input value in the target type, or the default value of the target type if the input has not equivalent value</returns>
        /// <remarks>If the target type is not known at compile time, use the non-generic overload of Force (<see cref="Convert.Force(Object, Type)"/>)</remarks>
        /// <example>
        /// Calling Force on invalid input will not raise an exception, but will instead
        /// return the default value of the target type:
        /// <code lang="cs">
        /// int intValue         = Convert.Force&lt;int&gt;("Foo bar"); // returns 0
        /// DataTable tableValue = Convert.Force&lt;DataTable&gt;(352); // returns null
        /// </code>
        /// <code lang="vb">
        /// Dim intValue   As Integer   = Convert.Force(Of Integer)("Foo bar")  ' Returns 0
        /// Dim tableValue As DataTable = Convert.Force(Of DataTable)(352)      ' Returns Nothing
        /// </code> 
        /// </example>
        public static T Force<T>(object value)
        {
            if (value is T) return (T)value;
#if INSTRUMENT
            Interlocked.Increment(ref ConvertOptions.Default.CountPastSameType);
#endif
            return (T)To(value, typeof(T), ConvertOptions.Default, true, null, Converter.Default.Converters);
        }

        /// <summary>
        /// Force any value to the target type. If the input cannot be converted to the target type, 
        /// or the input is empty and the target type is a value type, the provided default 
        /// value is returned. This method will never raise a conversion exception.
        /// </summary>
        /// <typeparam name="T">The target type</typeparam>
        /// <param name="value">Any value</param>
        /// <param name="defaultValue">The value to return if the input is empty or conversion fails</param>
        /// <returns>The equivalent of the input value in the target type, or the default value of the target type if the input has not equivalent value</returns>
        /// <remarks>If the target type is not known at compile time, use the non-generic overload of Force (<see cref="Convert.Force(Object, Type)"/>)</remarks>
        /// <example>
        /// Calling Force on invalid input will not raise an exception, but will instead
        /// return the default value of the target type:
        /// <code lang="cs">
        /// int intValue       = Convert.Force&lt;int&gt;("Foo bar", 3); // returns 3
        /// int intValue       = Convert.Force&lt;int&gt;(null, 3); // returns 3
        /// DateTime dateValue = Convert.Force&lt;DateTime&gt;(-352, DateTime.UtcNow); // returns DateTime.UtcNow
        /// </code>
        /// <code lang="vb">
        /// Dim intValue  As Integer = Convert.Force(Of Integer)("Foo bar", 3)       ' Returns 3
        /// Dim intValue  As Integer = Convert.Force(Of Integer)(Nothing, 3)         ' Returns 3
        /// Dim dateValue As Date    = Convert.Force(Of Date)(-352, DateTime.UtcNow) ' Returns DateTime.UtcNow
        /// </code> 
        /// </example>
        public static T Force<T>(object value, T defaultValue)
        {
            if (value is T) return (T)value;
#if INSTRUMENT
            Interlocked.Increment(ref ConvertOptions.Default.CountPastSameType);
#endif
            return (T)To(value, typeof(T), ConvertOptions.Default, true, defaultValue, Converter.Default.Converters);
        }


        /// <summary>
        /// Force any value to the target type. If the input is an empty value
        /// or cannot be converted to the target type, the default value of the target 
        /// type is returned. This method will never raise a conversion exception. This is
        /// a non-generic method, so the return type is Object. However, the method is
        /// guaranteed to return a valid value of the target type.
        /// </summary>
        /// <param name="value">Any value</param>
        /// <param name="targetType">The System.Type that the input value should be converted to</param>
        /// <returns>The equivalent of the input value in the target type, or the default value of the target type if the input has not equivalent value</returns>
        /// <remarks>If the target type is known at compile time, use the generic overload of Force (<see cref="Convert.Force{T}(Object)"/>)</remarks>
        /// <example>
        /// Calling Force on invalid input will not raise an exception, but will instead
        /// return the default value of the target type:
        /// <code lang="cs">
        /// object intObject   = Convert.Force("Foo bar", typeof(int)); // returns 0
        /// object tableObject = Convert.Force(352, typeof(DataTable)); // returns null
        /// </code>
        /// <code lang="vb">
        /// Dim intObject   As Object = Convert.Force("Foo bar", GetType(Integer))  ' Returns 0
        /// Dim tableObject As Object = Convert.Force(352, GetType(DataTable))      ' Returns Nothing
        /// </code> 
        /// </example>
        public static object Force(object value, Type targetType)
        {
            if (targetType.IsInstanceOfType(value)) return value;
#if INSTRUMENT
            Interlocked.Increment(ref ConvertOptions.Default.CountPastSameType);
#endif
            return To(value, targetType, ConvertOptions.Default, true, null, Converter.Default.Converters);
        }

    }
}
