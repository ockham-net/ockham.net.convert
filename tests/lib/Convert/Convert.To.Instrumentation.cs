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
        public static readonly object _instrumentLock = new object();

        public static IEnumerable<object[]> NotNull = Factories.Values(
            new object(),
            0m,
            Encoding.UTF32,
            DBNull.Value
        );

        public static IEnumerable<object[]> NotIntegers = Factories.Values(
            "2",
            2m,
            2.0d,
            TestShortEnum.Two
        );


        // CountPastNullRef is incremented as long as input is not a null reference (and NOT the same type)
        [Theory]
        [MemberData(nameof(NotNull))]
        public void CountPastNullRef_To_Reference(object value)
        {
            TestOverloads<string>(value, ConvertOptions.Default, (options, convert) =>
            {
                lock (_instrumentLock)
                    ConvertAssert.Increments(ref options.CountPastNullRef, convert.AsAction(true));
            });
        }

        // CountPastNullRef is incremented as long as input is not a null reference (and NOT the same type)
        [Theory]
        [MemberData(nameof(NotNull))]
        public void CountPastNullRef_To_Struct(object value)
        {
            TestOverloads<int>(value, ConvertOptions.Default, (options, convert) =>
            {
                lock (_instrumentLock)
                    ConvertAssert.Increments(ref options.CountPastNullRef, convert.AsAction(true));
            });
        }

        // CountPastNullRef is incremented as long as input is not a null reference 
        [Theory]
        [MemberData(nameof(NotNull))]
        public void CountPastNullRef_To_Nullable(object value)
        {
            TestOverloads<int?>(value, ConvertOptions.Default, (options, convert) =>
            {
                lock (_instrumentLock)
                    ConvertAssert.Increments(ref options.CountPastNullRef, convert.AsAction(true));
            });
        }

        // CountPastSameType is incremented as long as input is not an instance of the target type 
        [Theory]
        [MemberData(nameof(NotIntegers))]
        public void CountSameType(object value)
        {
            TestOverloads<int>(value, ConvertOptions.Default, (options, invoke) =>
            {
                lock (_instrumentLock)
                    ConvertAssert.Increments(ref options.CountPastSameType, invoke.AsAction(true));
            });
        }

        // Verify performance-critical path: Immediately return null for null input and target type is a reference type
        [Fact]
        public void ImmediateReturnForNullReference()
        {
            TestOverloads<Star>(null, ConvertOptions.Default, (options, convert) =>
            {
                lock (_instrumentLock)
                    ConvertAssert.DoesNotIncrement(ref options.CountPastNullRef, convert.AsAction());
            });
        }

        [Fact]
        public void ImmediateReturn_IsSelf_String()
        {
            TestOverloads<string>("hi", ConvertOptions.Default, (options, convert) =>
            {
                lock (_instrumentLock)
                    ConvertAssert.DoesNotIncrement(ref options.CountPastSameType, convert.AsAction());
            });
        }

        [Fact]
        public void ImmediateReturn_IsSelf_Int()
        {
            TestOverloads<int>(42, ConvertOptions.Default, (options, convert) =>
            {
                lock (_instrumentLock)
                    ConvertAssert.DoesNotIncrement(ref options.CountPastSameType, convert.AsAction());
            });
        }

        [Fact]
        public void ImmediateReturn_IsSelf_Class()
        {
            // Function should immediatley return
            TestOverloads<CelestialBody>(new NeutronStar(), ConvertOptions.Default, (options, invoke) =>
            {
                lock (_instrumentLock)
                    ConvertAssert.DoesNotIncrement(ref options.CountPastSameType, invoke.AsAction(true));
            });
        } 
    }
}

#endif