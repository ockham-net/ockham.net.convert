using Ockham.Data.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

#if INSTRUMENT
namespace Ockham.Data.Tests
{
    using static ConvertTestRunner;

    public partial class InstrumentationTests
    {
        public static readonly ConvertOptions CustomOptions
            = ConvertOptionsBuilder.Default
                .WithTrueStrings("t", "y")
                .WithFalseStrings("n", "f")
                .Options;

        public static readonly object _instrumentLock = new object();

        public static IEnumerable<object[]> NotNull = Factories.Values(
            new object(),
            0m,
            "",
            DBNull.Value
        );

        public static IEnumerable<object[]> NotIntegers = Factories.Values(
            "2",
            2m,
            2.0d,
            TestShortEnum.Two
        );


        // CountPastNullRef is incremented as long as input is not a null reference 
        [Theory]
        [MemberData(nameof(NotNull))]
        public void CountPastNullRef_To_Reference(object value)
        {
            foreach (var variant in OptionsVariant.GetAll(new object(), true))
            {
                var options = variant.ConvertOptions;
                ConvertAssert.Increments(
                    ref options.CountPastNullRef,
                    () => Convert.To(value, typeof(object), variant.ConvertOptions, variant.IgnoreError, variant.DefaultValue)
                );
            }
        }

        // CountPastNullRef is incremented as long as input is not a null reference 
        [Theory]
        [MemberData(nameof(NotNull))]
        public void CountPastNullRef_To_Struct(object value)
        {
            foreach (var variant in OptionsVariant.GetAll(new object(), true))
            {
                var options = variant.ConvertOptions;
                ConvertAssert.Increments(
                    ref options.CountPastNullRef,
                    () => Convert.To(value, typeof(int), variant.ConvertOptions, variant.IgnoreError, variant.DefaultValue),
                    ignoreException: true
                );
            }
        }

        // CountPastNullRef is incremented as long as input is not a null reference 
        [Theory]
        [MemberData(nameof(NotNull))]
        public void CountPastNullRef_To_Nullable(object value)
        {
            foreach (var variant in OptionsVariant.GetAll(new object(), true))
            {
                var options = variant.ConvertOptions;
                ConvertAssert.Increments(
                    ref options.CountPastNullRef,
                    () => Convert.To(value, typeof(int?), variant.ConvertOptions, variant.IgnoreError, variant.DefaultValue),
                    ignoreException: true
                );
            }
        }

        // CountPastNullRef is incremented as long as input is not a null reference 
        [Theory]
        [MemberData(nameof(NotNull))]
        public void CountPastNullRef_ToStruct(object value)
        {
            foreach (var variant in OptionsVariant.GetAll(new object(), true))
            {
                var options = variant.ConvertOptions;
                ConvertAssert.Increments(
                    ref options.CountPastNullRef,
                    () => Convert.ToStruct<int>(value, variant.ConvertOptions, variant.IgnoreError, variant.DefaultValue as int?),
                    ignoreException: true
                );
            }
        }

        // CountPastSameType is incremented as long as input is not an instance of the target type 
        [Theory]
        [MemberData(nameof(NotIntegers))]
        public void CountSameType(object value)
        {
            TestOverloads<int>(value, CustomOptions, (options, invoke) =>
            {
               lock (_instrumentLock)
                   ConvertAssert.Increments(
                       ref options.CountPastSameType,
                       () => invoke(),
                       true
                   );
            });
        }


        // Verify performance-critical path: Immediately return null for null input and target type is a reference type
        [Fact]
        public void ImmediateReturnForNullReference()
        {
            foreach (var variant in OptionsVariant.GetAll(new object(), true))
            {
                var options = variant.ConvertOptions;
                ConvertAssert.DoesNotIncrement(
                    ref options.CountPastNullRef,
                    () => Convert.To(null, typeof(object), variant.ConvertOptions, variant.IgnoreError, variant.DefaultValue)
                );
            }
        }

        [Fact]
        public void ImmediateReturn_IsSelf_String()
        {
            TestOverloads<string>("hi", CustomOptions, (options, invoke) =>
            {
                lock (_instrumentLock)
                    ConvertAssert.DoesNotIncrement(
                        ref options.CountPastSameType,
                        () => invoke(),
                        true
                    );
            });
        }

        [Fact]
        public void ImmediateReturn_IsSelf_Int()
        {
            TestOverloads<int>(42, CustomOptions, (options, invoke) =>
            {
                lock (_instrumentLock)
                    ConvertAssert.DoesNotIncrement(
                        ref options.CountPastSameType,
                        () => invoke(),
                        true
                    );
            });
        }

        [Fact]
        public void ImmediateReturn_IsSelf_Class()
        {
            // Function should immediatley return
            TestOverloads<CelestialBody>(new NeutronStar(), CustomOptions, (options, invoke) =>
            {
                lock (_instrumentLock)
                    ConvertAssert.DoesNotIncrement(
                        ref options.CountPastSameType,
                        () => invoke(),
                        true
                    );
            });
        }
    }
}

#endif