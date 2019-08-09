using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Ockham.Data.Tests
{
    [Flags]
    public enum ConvertOverload
    {
        None = 0x0,
        To = 0x1,
        Force = 0x2,
        Generic = 0x10,
        Options = 0x100,
        Instance = 0x1000,
        Static = 0x2000,
        DefaultValue = 0x10000,

        // All overloads that use custom options, whether 
        // a static method that accepts a ConvertOptions
        // parameter, OR any Converter instance methods
        // which implicity use the options on the Converter
        OptionsOrInstance = 0x1000000
    }

    public delegate void ConvertTestCallback(Func<object> convert);
    public delegate void TestOptionsCallback(ConvertOptions options, Func<object> convert);

    public class ConvertTestRunner
    {

        /// <summary>
        /// Test all possible converter methods, converting to type <typeparamref name="T"/>,
        /// also providing the options object to the test method
        /// </summary> 
        public static void TestOverloads<T>(object input, ConvertOptions options, TestOptionsCallback test)
            => TestOverloads<T>(AllOverloads, input, options, null, test, null);
          
        /// <summary>
        /// Test all converter methods, converting to type <paramref name="targetType"/>, filtered by <paramref name="flags"/>
        /// </summary> 
        public static void TestOverloads(ConvertOverload flags, Type targetType, object input, ConvertOptions options, ConvertTestCallback test)
            => (GetTestInvoker(targetType))(flags, input, options, test);

        /// <summary>
        /// Test only converter methods that use custom options, converting to type <typeparamref name="T"/>
        /// </summary> 
        public static void TestCustomOverloads<T>(object input, ConvertOptions options, ConvertTestCallback test)
            => TestOverloads<T>(ConvertOverload.OptionsOrInstance, input, options, test);

        /// <summary>
        /// Test only converter methods that use custom options, converting to type <typeparamref name="T"/>, filtered by <paramref name="filter"/>
        /// </summary> 
        public static void TestCustomOverloads<T>(ConvertOverload filter, object input, ConvertOptions options, ConvertTestCallback test)
            => TestOverloads<T>(filter | ConvertOverload.OptionsOrInstance, input, options, test);

        /// <summary>
        /// Test all possible converter methods, converting to type <typeparamref name="T"/>
        /// </summary> 
        public static void TestOverloads<T>(object input, ConvertOptions options, ConvertTestCallback test)
            => TestOverloads<T>(ConvertOverload.None, input, options, test);

        /// <summary>
        /// Test all possible converter methods, converting to type <typeparamref name="T"/>, filtered by <paramref name="filter"/>
        /// </summary> 
        public static void TestOverloads<T>(ConvertOverload filter, object input, ConvertOptions options, ConvertTestCallback test)
            => TestOverloads<T>(GetOverloads(filter), input, options, test, null, null);
         
        private static void TestOverloads<T>(IEnumerable<ConvertOverload> overloads, object input, ConvertOptions options, ConvertTestCallback test, TestOptionsCallback optionsTest, object defaultValue)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));


            foreach (var overload in overloads)
            {
                if (!ConvertInvoker<T>.Converters.TryGetValue(overload, out ConverterDelegate @delegate))
                    throw new NotImplementedException($"{overload} is not a valid convert method flag combination");

                try
                {
                    var useOptions = GetOptionsUsed(overload, options);
                    test?.Invoke(() => @delegate(input, useOptions));
                    optionsTest?.Invoke(useOptions, () => @delegate(input, useOptions));
                }
                catch (Xunit.Sdk.XunitException ex)
                {
                    throw new ConvertAssertException($"Test failed for {FormatOverload(overload)}", ex);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"Test failed for {FormatOverload(overload)}", ex);
                }
            }
        }


        #region BuildDelegates
        // =======================================================================================
        //  Get wrapped generic delegates for ConvertOverload filter and target type
        // =======================================================================================
        private delegate void TestInvoker(ConvertOverload filter, object input, ConvertOptions options, ConvertTestCallback test);

        private static readonly Dictionary<Type, TestInvoker> _testDelegates = new Dictionary<Type, TestInvoker>();

        private static TestInvoker GetTestInvoker(Type targetType)
        {
            var mapForType = _testDelegates;

            if (mapForType.TryGetValue(targetType, out TestInvoker @delegate)) return @delegate;
            lock (mapForType)
            {
                if (mapForType.TryGetValue(targetType, out @delegate)) return @delegate;

                var t_self = typeof(ConvertTestRunner);
                var m_open = t_self.GetMethod("TestOverloads", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(ConvertOverload), typeof(object), typeof(ConvertOptions), typeof(ConvertTestCallback) }, null);
                var m_Generic = m_open.MakeGenericMethod(targetType);

                List<ParameterExpression> parameters = new List<ParameterExpression>();
                parameters.Add(Expression.Parameter(typeof(ConvertOverload), "filter"));
                parameters.Add(Expression.Parameter(typeof(object), "input"));
                parameters.Add(Expression.Parameter(typeof(ConvertOptions), "options"));
                parameters.Add(Expression.Parameter(typeof(ConvertTestCallback), "test"));
                var lambda = Expression.Lambda<TestInvoker>(
                    Expression.Call(m_Generic, parameters.ToArray()),
                    parameters.ToArray()
                );

                return (mapForType[targetType] = lambda.Compile());
            }
        }
        // =======================================================================================
        #endregion


        private static readonly ConvertOverload[] AllOverloads =
        {
            ConvertOverload.Static | ConvertOverload.To,
            ConvertOverload.Static | ConvertOverload.To | ConvertOverload.Options,
            ConvertOverload.Static | ConvertOverload.To | ConvertOverload.Generic,
            ConvertOverload.Static | ConvertOverload.To | ConvertOverload.Generic | ConvertOverload.Options,
            ConvertOverload.Static | ConvertOverload.Force,
            ConvertOverload.Static | ConvertOverload.Force | ConvertOverload.Generic,
            ConvertOverload.Static | ConvertOverload.Force | ConvertOverload.Generic | ConvertOverload.DefaultValue,ConvertOverload.Instance | ConvertOverload.To,
            ConvertOverload.Instance | ConvertOverload.To | ConvertOverload.Generic,
            ConvertOverload.Instance | ConvertOverload.Force,
            ConvertOverload.Instance | ConvertOverload.Force | ConvertOverload.Generic,
            ConvertOverload.Instance | ConvertOverload.Force | ConvertOverload.Generic | ConvertOverload.DefaultValue
        };

        private static ConvertOverload[] GetOverloads(ConvertOverload flags)
        {
            if (flags.HasBitFlag(ConvertOverload.OptionsOrInstance))
            {
                flags &= ~ConvertOverload.OptionsOrInstance;
                return GetOverloads(flags | ConvertOverload.Instance).Concat(GetOverloads(flags | ConvertOverload.Options)).ToArray();
            }

            return AllOverloads.Where(overload => overload.HasBitFlag(flags)).ToArray();
        }

        private static ConvertOptions GetOptionsUsed(ConvertOverload overload, ConvertOptions options)
            => (overload.HasBitFlag(ConvertOverload.Options) || overload.HasBitFlag(ConvertOverload.Instance)) ? options : ConvertOptions.Default;

        private static string FormatOverload(ConvertOverload overload)
        {
            return _overloadSignatures.TryGetValue(overload, out string signature) ? signature : throw new NotImplementedException();
        }

        private static Dictionary<ConvertOverload, string> _overloadSignatures = new Dictionary<ConvertOverload, string>()
        {
            {
                ConvertOverload.Static | ConvertOverload.To,
                "static method Convert.To(object value, Type targetType)"
            },
            {
                ConvertOverload.Static |ConvertOverload.To | ConvertOverload.Options,
                "static method Convert.To(object value, Type targetType, ConvertOptions options)"
            },
            {
                ConvertOverload.Static |ConvertOverload.To | ConvertOverload.Generic,
                "static method Convert.To<T>(object value)"
            },
            {
                ConvertOverload.Static |ConvertOverload.To | ConvertOverload.Generic | ConvertOverload.Options,
                "static method Convert.To<T>(object value, ConvertOptions options)"
            },
            {
                ConvertOverload.Static |ConvertOverload.Force,
                "static method Convert.Force(object value, Type targetType)"
            },
            {
                ConvertOverload.Static |ConvertOverload.Force | ConvertOverload.Generic,
                "static method Convert.Force<T>(object value)"
            },
            {
                ConvertOverload.Static |ConvertOverload.Force | ConvertOverload.Generic | ConvertOverload.DefaultValue,
                "static method Convert.Force<T>(object value, T defaultValue)"
            },
            {
                ConvertOverload.Instance | ConvertOverload.To,
                "instance method Converter.To(object value, Type targetType)"
            },
            {
                ConvertOverload.Instance | ConvertOverload.To | ConvertOverload.Generic,
                "instance method Converter.To<T>(object value)"
            },
            {
                ConvertOverload.Instance | ConvertOverload.Force,
                "instance method Converter.Force(object value, Type targetType)"
            },
            {
                ConvertOverload.Instance | ConvertOverload.Force | ConvertOverload.Generic,
                "instance method Converter.Force<T>(object value)"
            },
            {
                ConvertOverload.Instance | ConvertOverload.Force | ConvertOverload.Generic | ConvertOverload.DefaultValue,
                "instance method Converter.Force<T>(object value, T defaultValue)"
            }
        };

        private static class ConvertInvoker<T>
        {
            public static readonly Dictionary<ConvertOverload, ConverterDelegate> Converters =
                new Dictionary<ConvertOverload, ConverterDelegate>() {
                {
                    ConvertOverload.Static | ConvertOverload.To,
                    (value, options) => Convert.To(value, typeof(T))
                },
                {
                    ConvertOverload.Static |ConvertOverload.To | ConvertOverload.Options,
                    (value, options) => Convert.To(value, typeof(T), options)
                },
                {
                    ConvertOverload.Static |ConvertOverload.To | ConvertOverload.Generic,
                    (value, options) => Convert.To<T>(value)
                },
                {
                    ConvertOverload.Static |ConvertOverload.To | ConvertOverload.Generic | ConvertOverload.Options,
                    (value, options) => Convert.To<T>(value, options)
                },
                {
                    ConvertOverload.Static |ConvertOverload.Force,
                    (value, options) => Convert.Force(value, typeof(T))
                },
                {
                    ConvertOverload.Static |ConvertOverload.Force | ConvertOverload.Generic,
                    (value, options) => Convert.Force<T>(value)
                },
                {
                    ConvertOverload.Static |ConvertOverload.Force | ConvertOverload.Generic | ConvertOverload.DefaultValue,
                    (value, options) => Convert.Force<T>(value, default(T))
                },
                {
                    ConvertOverload.Instance | ConvertOverload.To,
                    (value, options) => options.GetConverter().To(value, typeof(T))
                },
                {
                    ConvertOverload.Instance | ConvertOverload.To | ConvertOverload.Generic,
                    (value, options) => options.GetConverter().To<T>(value)
                },
                {
                    ConvertOverload.Instance | ConvertOverload.Force,
                    (value, options) => options.GetConverter().Force(value, typeof(T))
                },
                {
                    ConvertOverload.Instance | ConvertOverload.Force | ConvertOverload.Generic,
                    (value, options) => options.GetConverter().Force<T>(value)
                },
                {
                    ConvertOverload.Instance | ConvertOverload.Force | ConvertOverload.Generic | ConvertOverload.DefaultValue,
                    (value, options) => options.GetConverter().Force(value, default(T))
                }
            };
        }

    }
}
