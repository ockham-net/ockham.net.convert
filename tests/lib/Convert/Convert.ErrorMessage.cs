using Ockham.Data.Tests.Fixtures;
using System;
using System.Collections.Generic;
using Xunit;
using static Ockham.Data.Tests.Factories;

namespace Ockham.Data.Tests
{
    using static ConvertTestRunner;

    public partial class ConvertErrorTests
    {
        public static IEnumerable<object[]> ErrorData = Sets(
            Set(null, "null", typeof(int), "int"),
            Set(DBNull.Value, "DBNull", typeof(int), "int"),

            Set(null, "null", typeof(TestShortEnum), "Ockham.Data.Tests.Fixtures.TestShortEnum"),
            Set(DBNull.Value, "DBNull", typeof(TestShortEnum), "Ockham.Data.Tests.Fixtures.TestShortEnum"),

            Set(null, "null", typeof(ComplexNumber), "Ockham.Data.Tests.Fixtures.ComplexNumber"),
            Set(DBNull.Value, "DBNull", typeof(ComplexNumber), "Ockham.Data.Tests.Fixtures.ComplexNumber")
        );

        [Theory]
        [MemberData(nameof(ErrorData))]
        public static void ErrorMessage(object value, string valString, Type targetType, string typeName)
        {
            if (targetType == typeof(int))
            {
                TestErrorMessage<int>(value, valString, typeName);
            }
            else if (targetType == typeof(TestShortEnum))
            {
                TestErrorMessage<TestShortEnum>(value, valString, typeName);
            }
            else if (targetType == typeof(ComplexNumber))
            {
                TestErrorMessage<ComplexNumber>(value, valString, typeName);
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        private static void TestErrorMessage<T>(object value, string valString, string typeName)
        {
            string pattern = $"^Cannot convert empty value .*{valString}.* to value type .*{typeName}.*$";
            TestOverloads<T>(ConvertOverload.To, value, ConvertOptions.Default, ( invoke) =>
            {
                ThrowAssert.Throws(() => invoke(), pattern);
            });
        }

        public static IEnumerable<object[]> FormatValueData = Sets<object, string>(
            (null, "null"),
            (DBNull.Value, "DBNull"),
            ("", "string ''"),
            ("yo\r", "string 'yo\r'"),
            (TestShortEnum.FortyNine, "enum value 'FortyNine' of type Ockham.Data.Tests.Fixtures.TestShortEnum"),
            (132.123m, "decimal value 132.123"),
            (new NeutronStar(), "Ockham.Data.Tests.Fixtures.NeutronStar value")
         );

        [Theory]
        [MemberData(nameof(FormatValueData))]
        public static void FormatValue(object value, string valString)
        {
            Assert.Equal(valString, Convert.FormatValue(value));
        }
    }
}
