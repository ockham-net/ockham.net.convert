using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit.Sdk;
using Xunit;

namespace Ockham.Data.Tests
{
    using static ConvertTestRunner;

    public static class ConvertAssert
    {
        public static void Increments(ref long value, Action action)
        {
            long before = Interlocked.Read(ref value);
            action();
            long result = Interlocked.Read(ref value);
            Assert.Equal(before + 1, result);
        }

        public static void Increments(ref long value, Action action, bool ignoreException)
        {
            long before = Interlocked.Read(ref value);
            if (ignoreException)
            {
                try { action(); } catch { }
            }
            else
            {
                action();
            }
            long result = Interlocked.Read(ref value);
            Assert.Equal(before + 1, result);
        }

        public static void DoesNotIncrement(ref long value, Action action)
        {
            long before = Interlocked.Read(ref value);
            action();
            long result = Interlocked.Read(ref value);
            Assert.Equal(before, result);
        }

        public static void DoesNotIncrement(ref long value, Action action, bool ignoreException)
        {
            long before = Interlocked.Read(ref value);
            if (ignoreException)
            {
                try { action(); } catch { }
            }
            else
            {
                action();
            }
            long result = Interlocked.Read(ref value);
            Assert.Equal(before, result);
        }

        /// <summary>
        /// Assert that provided <paramref name="value"/> matches the type and value of <paramref name="expected"/>
        /// </summary> 
        public static void Equal<T>(T expected, object value)
        {
            Assert.IsAssignableFrom<T>(value);
            Assert.Equal<T>(expected, (T)value);
        }

        /// <summary>
        /// Test that all possible conversion methods successfully convert <paramref name="value"/>
        /// to <paramref name="expected"/> using default options 
        /// </summary>
        public static void Converts(Type targetType, object value, object expected)
            => Converts(ConvertOverload.None, targetType, value, expected, ConvertOptions.Default);

        /// <summary>
        /// Test that all possible conversion methods successfully convert <paramref name="value"/>
        /// to <paramref name="expected"/>, using default options
        /// </summary>
        public static void Converts(ConvertOverload filter, Type targetType, object value, object expected)
            => Converts(filter, targetType, value, expected, ConvertOptions.Default);

        /// <summary>
        /// Test that all possible conversion methods successfully convert <paramref name="value"/>
        /// to <paramref name="expected"/>, when using the provided <paramref name="options"/>
        /// </summary> 
        public static void Converts(Type targetType, object value, object expected, ConvertOptions options)
            => Converts(ConvertOverload.OptionsOrInstance, targetType, value, expected, options);

        /// <summary>
        /// Test that all possible conversion methods successfully convert <paramref name="value"/>
        /// to <paramref name="expected"/>, given provided <paramref name="options"/>
        /// </summary> 
        private static void Converts(ConvertOverload filter, Type targetType, object value, object expected, ConvertOptions options)
        {
            bool toNull = expected == null;
            if (!toNull && !targetType.IsInstanceOfType(expected)) throw new InvalidCastException($"Expected value {expected} is not of target type {targetType.FullName}");

            TestOverloads(filter, targetType, value, options, convert =>
            {
                var result = convert();
                if (toNull)
                {
                    Assert.Null(result);
                }
                else
                {
                    Assert.IsAssignableFrom(targetType, result);
                    Assert.Equal(expected, result);
                }
            });
        }


        /// <summary>
        /// Test that all possible conversion methods successfully convert <paramref name="value"/>
        /// to <paramref name="expected"/> using default options
        /// </summary> 
        public static void Converts<T>(object value, T expected)
        {
            TestOverloads<T>(value, ConvertOptions.Default, invoke =>
            {
                var result = invoke();
                Assert.IsAssignableFrom<T>(result);
                Assert.Equal(expected, (T)result);
            });
        }

        /// <summary>
        /// Test that all possible conversion methods successfully convert <paramref name="value"/>
        /// to null using default options
        /// </summary> 
        public static void ConvertsToNull<T>(object value)
        {
            TestOverloads<T>(value, ConvertOptions.Default, invoke =>
            {
                var result = invoke();
                Assert.Null(result);
            });
        }

        /// <summary>
        /// Test that all possible conversion methods successfully convert <paramref name="value"/>
        /// to <paramref name="expected"/>, given provided <paramref name="options"/>
        /// </summary> 
        public static void Converts<T>(object value, T expected, ConvertOptions options)
        {
            TestCustomOverloads<T>(value, options, convert =>
            {
                var result = convert();
                Assert.IsType<T>(result);
                Assert.Equal(expected, (T)result);
            });
        }


        /// <summary>
        /// Test that all possible conversion methods convert <paramref name="value"/>
        /// to null, given provided <paramref name="options"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public static void ConvertsToNull<T>(object value, ConvertOptions options)
        {
            TestCustomOverloads<T>(value, options, convert =>
            {
                var result = convert();
                Assert.Null(result);
            });
        }

        /// <summary>
        ///  Test the all static and instance overloads of Force(..) convert <paramref name="value"/>
        /// to null, given provided <paramref name="options"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public static void ForcesToNull<T>(object value, ConvertOptions options)
        {
            TestCustomOverloads<T>(ConvertOverload.Force, value, options, convert =>
            {
                var result = convert();
                Assert.Null(result);
            });
        }

        /// <summary>
        /// Test the all static and instance overloads of To(..) throw an exception
        /// when trying to convert <paramref name="value"/> to type <typeparamref name="T"/>,
        /// when using the provided <paramref name="options"/>
        /// </summary> 
        public static void ConvertFails<T>(object value, ConvertOptions options)
        {
            TestCustomOverloads<T>(ConvertOverload.To, value, options, convert =>
            {
                Assert.ThrowsAny<SystemException>(() => convert());
            });
        }

        /// <summary>
        /// Test the all conversion methods (excluding Force) throw an exception with a <see cref="Exception.Message"/>
        /// matching <paramref name="messagePattern"/>, when trying to convert <paramref name="value"/> to type <typeparamref name="T"/>,
        /// when using the provided <paramref name="options"/>
        /// </summary> 
        public static void ConvertFails<T>(object value, ConvertOptions options, string messagePattern)
        {
            TestCustomOverloads<T>(ConvertOverload.To, value, options, convert =>
            {
                ThrowAssert.Throws<SystemException>(() => convert(), messagePattern);
            });
        }

        /// <summary>
        /// Test the all static and instance overloads of To(..) throw an exception
        /// when trying to convert <paramref name="value"/> to type <paramref name="targetType"/>
        /// when using the provided <paramref name="options"/>
        /// </summary> 
        public static void ConvertFails(Type targetType, object value, ConvertOptions options)
        {
            TestCustomOverloads(ConvertOverload.To, targetType, value, options, convert =>
           {
               Assert.ThrowsAny<SystemException>(() => convert());
           });
        }

    }
}
