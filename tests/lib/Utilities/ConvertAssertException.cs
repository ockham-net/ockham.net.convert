using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Sdk;

namespace Ockham.Data.Tests
{
    public class ConvertAssertException : XunitException
    {
        public ConvertAssertException(string message) : base(message) { }
        public ConvertAssertException(string message, Exception innerException) : base(message, innerException) { }
    }
}
