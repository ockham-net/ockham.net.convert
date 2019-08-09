using Ockham.Data.Tests.Fixtures;
using System;
using System.Reflection;
using Xunit;
using System.Linq;
using System.Collections.Generic;

namespace Ockham.Data.Tests
{
    using static ConvertTestRunner;

    // Test that ConvertOptions.Enums settings have the intended effect
    public partial class ConvertFromStringTests
    {
        public static readonly IEnumerable<object[]> StartPadded42s = ConvertToNumberTests.String42s_Strict.Select(o => " \r " + (o[0] as string)).AsObjectArray().ToArray();
        public static readonly IEnumerable<object[]> EndPadded42s = ConvertToNumberTests.String42s_Strict.Select(o => (o[0] as string) + " \t ").AsObjectArray().ToArray();
        public static readonly IEnumerable<object[]> BothPadded42s = ConvertToNumberTests.String42s_Strict.Select(o => " \r " + (o[0] as string) + " \t ").AsObjectArray().ToArray();
        public static readonly IEnumerable<object[]> AllPadded42s = StartPadded42s.Concat(EndPadded42s).Concat(BothPadded42s).ToArray();

        [Theory]
        [MemberData(nameof(StartPadded42s))]
        public static void AllowStartPadding_Numbers(string value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithStringOptions(StringAsNullOption.NullReference, TrimStringFlags.TrimStart).Options;

            TestCustomOverloads<int>(value, options, convert =>
            {
                var result = convert();
                Assert.IsType<int>(result);
                Assert.Equal(42, (int)result);
            });
        }

        [Theory]
        [MemberData(nameof(EndPadded42s))]
        public static void AllowEndPadding_Numbers(string value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithStringOptions(StringAsNullOption.NullReference, TrimStringFlags.TrimEnd).Options;

            TestCustomOverloads<int>(value, options, convert =>
            {
                var result = convert();
                Assert.IsType<int>(result);
                Assert.Equal(42, (int)result);
            });
        }

        [Theory]
        [MemberData(nameof(AllPadded42s))]
        public static void AllowBothPadding_Numbers(string value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithStringOptions(StringAsNullOption.NullReference, TrimStringFlags.TrimAll).Options;

            TestCustomOverloads<int>(value, options, convert =>
            {
                var result = convert();
                Assert.IsType<int>(result);
                Assert.Equal(42, (int)result);
            });
        }


        [Theory]
        [MemberData(nameof(StartPadded42s))]
        public static void RejectStartPadding_Numbers_None(string value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithStringOptions(StringAsNullOption.NullReference, TrimStringFlags.None).Options;

            TestCustomOverloads<int>(ConvertOverload.To, value, options, convert =>
            {
                Assert.ThrowsAny<SystemException>(() => convert());
            });
        }


        [Theory]
        [MemberData(nameof(StartPadded42s))]
        public static void RejectStartPadding_Numbers_End(string value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithStringOptions(StringAsNullOption.NullReference, TrimStringFlags.TrimEnd).Options;

            TestCustomOverloads<int>(ConvertOverload.To, value, options, convert =>
            {
                Assert.ThrowsAny<SystemException>(() => convert());
            });
        }


        [Theory]
        [MemberData(nameof(EndPadded42s))]
        public static void RejectEndPadding_Numbers_None(string value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithStringOptions(StringAsNullOption.NullReference, TrimStringFlags.None).Options;

            TestCustomOverloads<int>(ConvertOverload.To, value, options, convert =>
            {
                Assert.ThrowsAny<SystemException>(() => convert());
            });
        }


        [Theory]
        [MemberData(nameof(EndPadded42s))]
        public static void RejectEndPadding_Numbers_Start(string value)
        {
            var options = ConvertOptions.Default.GetBuilder()
                .WithStringOptions(StringAsNullOption.NullReference, TrimStringFlags.TrimStart).Options;

            TestCustomOverloads<int>(ConvertOverload.To, value, options, convert =>
            {
                Assert.ThrowsAny<SystemException>(() => convert());
            });
        }
    }
}
