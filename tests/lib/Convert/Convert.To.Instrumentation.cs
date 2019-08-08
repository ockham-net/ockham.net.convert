using Ockham.Data.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace Ockham.Data.Tests
{

#if INSTRUMENT
    public partial class ConvertTests
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
          
    }
#endif
}
