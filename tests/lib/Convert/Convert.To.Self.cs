using Ockham.Data.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace Ockham.Data.Tests
{
    public partial class ConvertTests
    {

#if INSTRUMENT

        // Verify performance-critical path: Immediately return null for null input and target type is a reference type
        [Fact]
        public void ImmediateReturnForNullReference()
        {
            foreach (var variant in OptionsVariant.GetAll(new object(), true))
            {
                var options = variant.ConvertOptions;
                ConvertAssert.DoesNotIncrement(
                    ref options.CountPastNullRef,
                    () => Convert.To(null, typeof(object), variant.ConvertOptions, variant.IgnoreError, variant.DefaultValue, null)
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

#endif
    }
}
