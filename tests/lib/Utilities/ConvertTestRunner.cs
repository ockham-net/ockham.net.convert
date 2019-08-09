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
        DefaultValue = 0x10000
    }

    public delegate void ConvertTestCallback(Func<object> convert);

    public class ConvertTestRunner
    {
        /// <summary>
        /// Test all possible converter methods, converting to type <typeparamref name="T"/>
        /// </summary> 
        public static void TestOverloads<T>(object input, ConvertOptions options, Action<ConvertOptions, Func<object>> test)
            => TestOverloads<T>(GetOverloads(), input, options, test);

        /// <summary>
        /// Test only converter methods that use custom options, converting to type <paramref name="targetType"/>
        /// </summary> 
        public static void TestCustomOverloads(Type targetType, object input, ConvertOptions options, ConvertTestCallback test)
            => TestCustomOverloads(ConvertOverload.None, targetType, input, options, test);

        /// <summary>
        /// Test only converter methods that use custom options, converting to type <paramref name="targetType"/>, filtered by <paramref name="flags"/>
        /// </summary> 
        public static void TestCustomOverloads(ConvertOverload flags, Type targetType, object input, ConvertOptions options, ConvertTestCallback test)
            => (GetCustomTestInvoker(targetType))(flags, input, options, test);

        /// <summary>
        /// Test only converter methods that use custom options, converting to type <typeparamref name="T"/>
        /// </summary> 
        public static void TestCustomOverloads<T>(object input, ConvertOptions options, ConvertTestCallback test)
            => TestCustomOverloads<T>(ConvertOverload.None, input, options, test);

        /// <summary>
        /// Test only converter methods that use custom options, converting to type <typeparamref name="T"/>, filtered by <paramref name="filter"/>
        /// </summary> 
        public static void TestCustomOverloads<T>(ConvertOverload filter, object input, ConvertOptions options, ConvertTestCallback test)
        {
            IEnumerable<ConvertOverload> overloads;
            overloads = GetOverloads(ConvertOverload.Instance)
                    .Concat(GetOverloads(ConvertOverload.Options));

            if (filter != ConvertOverload.None) overloads = overloads.Where(flag => flag.HasBitFlag(filter));

            TestOverloads<T>(overloads.ToArray(), input, options, (opts, invoke) => test(invoke));
        }

        #region BuildDelegates
        // =======================================================================================
        //  Get wrapped generic delegates for ConvertOverload filter and target type
        // =======================================================================================
        private delegate void CustomTestInvoker(ConvertOverload filter, object input, ConvertOptions options, ConvertTestCallback test);

        private static readonly Dictionary<Type, CustomTestInvoker> _testCustomDelegates = new Dictionary<Type, CustomTestInvoker>();

        private static CustomTestInvoker GetCustomTestInvoker(Type targetType)
        {
            var mapForType = _testCustomDelegates;

            if (mapForType.TryGetValue(targetType, out CustomTestInvoker @delegate)) return @delegate;
            lock (mapForType)
            {
                if (mapForType.TryGetValue(targetType, out @delegate)) return @delegate;

                var t_self = typeof(ConvertTestRunner);
                var m_open = t_self.GetMethod("TestCustomOverloads", BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(ConvertOverload), typeof(object), typeof(ConvertOptions), typeof(ConvertTestCallback) }, null);
                var m_Generic = m_open.MakeGenericMethod(targetType);

                List<ParameterExpression> parameters = new List<ParameterExpression>();
                parameters.Add(Expression.Parameter(typeof(ConvertOverload), "filter"));
                parameters.Add(Expression.Parameter(typeof(object), "input"));
                parameters.Add(Expression.Parameter(typeof(ConvertOptions), "options"));
                parameters.Add(Expression.Parameter(typeof(ConvertTestCallback), "test"));
                var lambda = Expression.Lambda<CustomTestInvoker>(
                    Expression.Call(m_Generic, parameters.ToArray()),
                    parameters.ToArray()
                );

                return (mapForType[targetType] = lambda.Compile());
            }
        }
        // =======================================================================================
        #endregion

        public static void TestOverloads<T>(ConvertOverload? flags, object input, ConvertOptions options, Action<ConvertOptions, Func<object>> test)
        {
            var overloads = flags.HasValue ? GetOverloads(flags.Value) : GetOverloads();
            TestOverloads<T>(overloads, input, options, test);
        }

        private static ConvertOverload[] GetOverloads(bool? instance = null)
        {
            if (instance.HasValue)
            {
                if (instance.Value)
                {
                    return new[] {
                    ConvertOverload.Instance | ConvertOverload.To,
                    ConvertOverload.Instance | ConvertOverload.To | ConvertOverload.Generic,
                    ConvertOverload.Instance | ConvertOverload.Force,
                    ConvertOverload.Instance | ConvertOverload.Force | ConvertOverload.Generic,
                    ConvertOverload.Instance | ConvertOverload.Force | ConvertOverload.Generic | ConvertOverload.DefaultValue
                };
                }
                else
                {
                    return new[] {
                    ConvertOverload.To,
                    ConvertOverload.To | ConvertOverload.Options,
                    ConvertOverload.To | ConvertOverload.Generic,
                    ConvertOverload.To | ConvertOverload.Generic | ConvertOverload.Options,
                    ConvertOverload.Force,
                    ConvertOverload.Force | ConvertOverload.Generic,
                    ConvertOverload.Force | ConvertOverload.Generic | ConvertOverload.DefaultValue
                };
                }
            }
            else
            {
                return GetOverloads(true).Concat(GetOverloads(false)).ToArray();
            }
        }

        public static ConvertOverload[] GetOverloads(ConvertOverload flags)
            => GetOverloads().Where(x => x.HasBitFlag(flags)).ToArray();

        private static ConvertOptions GetOptionsUsed(ConvertOverload overload, ConvertOptions options)
            => (overload.HasBitFlag(ConvertOverload.Options) || overload.HasBitFlag(ConvertOverload.Instance)) ? options : ConvertOptions.Default;

        private static void TestOverloads<T>(IEnumerable<ConvertOverload> overloads, object input, ConvertOptions options, Action<ConvertOptions, Func<object>> test)
        {
            foreach (var overload in overloads)
            {
                try
                {
                    var useOptions = GetOptionsUsed(overload, options);
                    test(useOptions, () => InvokeConvert<T>(input, overload, useOptions));
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

        private static object InvokeConvert<T>(object input, ConvertOverload overload, ConvertOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (overload.HasBitFlag(ConvertOverload.Instance))
            {
                var converter = new Converter(options);
                if (overload.HasBitFlag(ConvertOverload.To))
                {
                    if (overload.HasBitFlag(ConvertOverload.Generic))
                    {
                        return converter.To<T>(input);
                    }
                    else
                    {
                        return converter.To(input, typeof(T));
                    }
                }
                else if (overload.HasBitFlag(ConvertOverload.Force))
                {
                    if (overload.HasBitFlag(ConvertOverload.Generic))
                    {
                        if (overload.HasBitFlag(ConvertOverload.DefaultValue))
                        {
                            return converter.Force<T>(input, default);
                        }
                        else
                        {
                            return converter.Force<T>(input);
                        }
                    }
                    else
                    {
                        return converter.Force(input, typeof(T));
                    }
                }

                throw new NotImplementedException();
            }

            // Static
            if (overload.HasBitFlag(ConvertOverload.To))
            {
                if (overload.HasBitFlag(ConvertOverload.Generic))
                {
                    if (overload.HasBitFlag(ConvertOverload.Options))
                    {
                        return Convert.To<T>(input, options);
                    }
                    else
                    {
                        return Convert.To<T>(input);
                    }
                }
                else
                {
                    if (overload.HasBitFlag(ConvertOverload.Options))
                    {
                        return Convert.To(input, typeof(T), options);
                    }
                    else
                    {
                        return Convert.To(input, typeof(T));
                    }
                }
            }
            else if (overload.HasBitFlag(ConvertOverload.Force))
            {
                if (overload.HasBitFlag(ConvertOverload.Generic))
                {
                    if (overload.HasBitFlag(ConvertOverload.DefaultValue))
                    {
                        return Convert.Force<T>(input, default);
                    }
                    else
                    {
                        return Convert.Force<T>(input);
                    }
                }
                else
                {
                    return Convert.Force(input, typeof(T));
                }
            }

            throw new NotImplementedException();
        }


        private static string FormatOverload(ConvertOverload overload)
        {
            return _overloadSignatures.TryGetValue(overload, out string signature) ? signature : throw new NotImplementedException();
        }

        private static Dictionary<ConvertOverload, string> _overloadSignatures = new Dictionary<ConvertOverload, string>()
        {
            {
                ConvertOverload.To,
                "static method Convert.To(object value, Type targetType)"
            },
            {
                ConvertOverload.To | ConvertOverload.Options,
                "static method Convert.To(object value, Type targetType, ConvertOptions options)"
            },
            {
                ConvertOverload.To | ConvertOverload.Generic,
                "static method Convert.To<T>(object value)"
            },
            {
                ConvertOverload.To | ConvertOverload.Generic | ConvertOverload.Options,
                "static method Convert.To<T>(object value, ConvertOptions options)"
            },
            {
                ConvertOverload.Force,
                "static method Convert.Force(object value, Type targetType)"
            },
            {
                ConvertOverload.Force | ConvertOverload.Generic,
                "static method Convert.Force<T>(object value)"
            },
            {
                ConvertOverload.Force | ConvertOverload.Generic | ConvertOverload.DefaultValue,
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

    }
}
