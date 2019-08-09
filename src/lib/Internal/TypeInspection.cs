using System;

namespace Ockham.Data
{
    /// <summary>
    /// NOT part of the public API of this project. Type inspection per se is not the goal of the Ockham.NET.Convert module
    /// </summary>
    internal class TypeInspection
    {
        /// <summary>
        /// Determine if the specified type inherits from <see cref="System.Nullable{T}"/>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullableOfT(Type type)
        {
            return type.IsGenericType && !type.IsGenericTypeDefinition && (type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        /// Determine if the specified type is a numeric (integer, float, or decimal) type. Returns true for enums.
        /// </summary> 
        public static bool IsNumberType(Type type) => type.IsEnum || IsNumberType(type, out _);

        /// <summary>
        /// Determine if the specified type is a numeric (integer, float, or decimal) type, and wether 
        /// the numeric type is a signed number (can represent negative values)
        /// </summary> 
        public static bool IsNumberType(Type type, out TypeCode typeCode)
        {
            switch (typeCode = Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return true;

                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
            }

            return false;
        }
    }
}