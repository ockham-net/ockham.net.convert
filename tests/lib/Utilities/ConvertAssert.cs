using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit.Sdk;
using Xunit;

namespace Ockham.Data.Tests
{
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
    }
}
