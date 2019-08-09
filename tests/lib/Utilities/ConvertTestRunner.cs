using System;
using System.Collections.Generic;
using System.Linq;

namespace Ockham.Data.Tests
{
    [Flags]
    public enum ConvertOverload
    {
        To = 0x1,
        Force = 0x2,
        Generic = 0x10,
        Options = 0x100,
        Instance = 0x1000,
        DefaultValue = 0x10000
    }

    public class ConvertTestRunner
    {
        /// <summary>
        /// Test all possible converter overloads
        /// </summary> 
        public static void TestOverloads<T>(object input, ConvertOptions options, Action<ConvertOptions, Func<object>> test)
            => TestOverloads<T>(GetOverloads(), input, options, test);

        /// <summary>
        /// Test only those overloads that use custom options (either static or implicitly b/c instance) 
        /// </summary> 
        public static void TestCustomOverloads<T>(object input, ConvertOptions options, Action<Func<object>> test)
            => TestCustomOverloads<T>(null, true, input, options, (opts, invoke) => test(invoke));

        /// <summary>
        /// Test only those overloads that use custom options (either static or implicitly b/c instance),
        /// filtered for the provided <paramref name="flags"/>
        /// </summary> 
        public static void TestCustomOverloads<T>(ConvertOverload flags, object input, ConvertOptions options, Action<Func<object>> test)
            => TestCustomOverloads<T>(flags, true, input, options, (opts, invoke) => test(invoke));


        /// <summary>
        /// Test only those overloads that use custom options (either static or implicitly b/c instance),
        /// OR only those overloads that use default options (either static or implicitly via Converter.Default)
        /// </summary> 
        public static void TestCustomOverloads<T>(ConvertOverload? flags, bool explicitOptions, object input, ConvertOptions options, Action<ConvertOptions, Func<object>> test)
        {
            IEnumerable<ConvertOverload> overloads;
            if (explicitOptions)
            {
                if (flags.HasValue)
                {
                    overloads = GetOverloads(flags.Value | ConvertOverload.Instance)
                        .Concat(GetOverloads(flags.Value | ConvertOverload.Options));
                }
                else
                {
                    overloads = GetOverloads(ConvertOverload.Instance)
                       .Concat(GetOverloads(ConvertOverload.Options));
                }
            }
            else
            {
                throw new NotImplementedException();
            }

            TestOverloads<T>(overloads, input, options, test);
        }

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
                    throw new ConvertAssertException($"Test failed for overload {overload}", ex);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException($"Test failed for overload {overload}", ex);
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



    }
}
