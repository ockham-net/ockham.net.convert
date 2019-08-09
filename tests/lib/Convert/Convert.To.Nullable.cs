﻿using Ockham.Data.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace Ockham.Data.Tests
{
    using static Factories;

    public partial class ConvertToNullableTests
    {
        [Fact]
        public static void NullToNull()
        {
            ConvertAssert.ConvertsToNull<int?>(null);
            ConvertAssert.ConvertsToNull<int?>(DBNull.Value);
        }

        [Fact]
        public static void EmptyToNull()
        {
            ConvertAssert.ConvertsToNull<int?>("", OptionsVariant.EmptyStringAsNull);
        }

        [Fact]
        public static void WhitespaceToNull()
        {
            ConvertAssert.ConvertsToNull<int?>("  \r  \t ", OptionsVariant.WhitespaceAsNull);
        }


        [Theory]
        [MemberData(nameof(ConvertToNumberTests.Decimal42s_Strict), MemberType = typeof(ConvertToNumberTests))]
        public static void ValuesToValue(object value)
        {
            ConvertAssert.Converts(value, (int?)42);
        }
    }
}
