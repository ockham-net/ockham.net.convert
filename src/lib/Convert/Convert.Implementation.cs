using System;
using System.Collections.Generic;
using System.Threading;
using VBConvert = Microsoft.VisualBasic.CompilerServices.Conversions;

namespace Ockham.Data
{
    /// <summary>
    /// A utility for converting primitive values
    /// </summary>
    /// <seealso cref="Converter"/>
    public static partial class Convert
    {

        internal static object To(object value, Type targetType, ConvertOptions options, bool ignoreError, object valueOnError)
        {
            if (!targetType.IsValueType) return ToReference(value, targetType, options, ignoreError, valueOnError);

            // Do NOT check for same type here. That should already be done by calling functions

            if (
#if INSTRUMENT
                // Technically we're incrementing before the null check...but it has the desired effect
                ((Interlocked.Increment(ref options.CountPastNullRef) > 0) && (null == value))
#else
                (null == value)
#endif
                || Value.IsNull(value, options))
            {
                if (TypeInspection.IsNullableOfT(targetType))
                {
                    return null;
                }
                else if (options.NullToValueDefault)
                {
                    return Activator.CreateInstance(targetType);
                }
                else if (ignoreError)
                {
                    return valueOnError ?? Activator.CreateInstance(targetType);
                }
                else
                {
                    throw new InvalidCastException($"Cannot convert empty value {FormatValue(value)} to value type {FormatType(targetType)}");
                }
            }

#if INSTRUMENT
            Interlocked.Increment(ref options.CountPastEmptyValue);
#endif

            // ---------------------------------------------------------------------------
            //  Special treatment of Nullable<T>. Although both C# and VB allow assignment
            //  and comparison of null literal to Nullable<T> values, the underlying type
            //  is actually a non-nullable struct (value type), thus we handle nullable
            //  types inside the section that handles value types
            // ---------------------------------------------------------------------------
            if (TypeInspection.IsNullableOfT(targetType))
            {
                Type underlyingType = Nullable.GetUnderlyingType(targetType);
                if (!ignoreError) return ToStructValue(value, underlyingType, options, false, null);

                try
                {
                    return ToStructValue(value, underlyingType, options, false, null);
                }
                catch
                {
                    return valueOnError;
                }
            }

            return ToStructValue(value, targetType, options, ignoreError, valueOnError);
        }

        internal static object ToReference(object value, Type targetType, ConvertOptions options, bool ignoreError, object valueOnError)
        {
            // Reference types...not much to do here besides check for empty values
            if (null == value) return null;

#if INSTRUMENT
            Interlocked.Increment(ref options.CountPastNullRef);
#endif

            if (Value.IsNull(value, options)) return null;

#if INSTRUMENT
            Interlocked.Increment(ref options.CountPastEmptyValue);
#endif

            return ToReferenceValue(value, targetType, options, ignoreError, valueOnError);
        }

        private static object ToReferenceValue(object value, Type targetType, ConvertOptions options, bool ignoreError, object valueOnError)
        {
            if (ignoreError)
            {
                try
                {
                    return ToReferenceValue(value, targetType, options, false, valueOnError);
                }
                catch
                {
                    return valueOnError ?? null;
                }
            }

            // First try custom converter
            if (options.HasCustomConverters && options.Converters.TryGetValue(targetType, out ConverterDelegate converter))
            {
                return converter(value, options);
            }

            if (targetType == typeof(string))
            {
                // Will invoke IConvertible.ToString() or base object.ToString()
                return System.Convert.ToString(value);
            }

            //  Fall back to VBConvert.ChangeType
            return VBConvert.ChangeType(value, targetType);
        }

        /// <summary>
        /// Caller *guarantees* that <paramref name="targetType"/> is NOT a <see cref="Nullable{T}"/>, 
        /// that value is not empty per <paramref name="options"/>, and that value is not already <paramref name="targetType"/>
        /// </summary> 
        internal static object ToStructValue(object value, Type targetType, ConvertOptions options, bool ignoreError, object valueOnError)
        {
            if (ignoreError)
            {
                try
                {
                    return ToStructValue(value, targetType, options, false, valueOnError);
                }
                catch
                {
                    return valueOnError ?? Activator.CreateInstance(targetType);
                }
            }

            // ---------------------------------------------------------------------------
            //  Custom converters
            // ---------------------------------------------------------------------------
            if (options.HasCustomConverters && options.Converters.TryGetValue(targetType, out ConverterDelegate converter))
            {
                return converter(value, options);
            }

            // ---------------------------------------------------------------------------
            //  Special treatment of Enums
            // ---------------------------------------------------------------------------
            if (targetType.IsEnum) return EnumConverter.ToEnumValue(value, targetType, options, ignoreError, valueOnError);

            // ---------------------------------------------------------------------------
            //  Parse numeric strings
            // ---------------------------------------------------------------------------
            if ((value is string stringValue) && TypeInspection.IsNumberType(targetType, out TypeCode typeCode))
            {
                var result = NumberParser.Parse(stringValue, targetType, options, typeCode);
                if (result != null) return result;
            }

            // ---------------------------------------------------------------------------
            //  Invoke IConvertible implementation, if any
            // ---------------------------------------------------------------------------
            if (value is IConvertible iConvertible)
            {
                // Use the System.ChangeType method, which makes use of any IConvertible implementation on the target type
                try
                {
                    return System.Convert.ChangeType(value, targetType);
                }
                catch
                {
                    // Ignore exception and fall back to VBConvert implementation
                }
            }

            /*
            // ---------------------------------------------------------------------------
            //  Fall back to VBConvert methods
            // ---------------------------------------------------------------------------
            switch (Type.GetTypeCode(targetType))
            {
                case TypeCode.Boolean:
                    return VBConvert.ToBoolean(value);
                case TypeCode.Byte:
                    return VBConvert.ToByte(value);
                case TypeCode.Char:
                    return VBConvert.ToChar(value);
                case TypeCode.DateTime:
                    return VBConvert.ToDate(value);
                case TypeCode.Decimal:
                    return VBConvert.ToDecimal(value);
                case TypeCode.Double:
                    return VBConvert.ToDouble(value);
                case TypeCode.Int16:
                    return VBConvert.ToShort(value);
                case TypeCode.Int32:
                    return VBConvert.ToInteger(value);
                case TypeCode.Int64:
                    return VBConvert.ToLong(value);
                case TypeCode.SByte:
                    return VBConvert.ToSByte(value);
                case TypeCode.Single:
                    return VBConvert.ToSingle(value);
                case TypeCode.UInt16:
                    return VBConvert.ToUShort(value);
                case TypeCode.UInt32:
                    return VBConvert.ToUInteger(value);
                case TypeCode.UInt64:
                    return VBConvert.ToULong(value);
            }
            */

            // ---------------------------------------------------------------------------
            //  Fall back to VBConvert.ChangeType, which will find and invoke 
            //  custom conversion operators
            // ---------------------------------------------------------------------------
            return VBConvert.ChangeType(value, targetType);
        }

        private static Dictionary<TypeCode, string> _typeAliases
            = new Dictionary<TypeCode, string>() {
                { TypeCode.Boolean, "boolean" },
                { TypeCode.Byte, "byte" },
                { TypeCode.DateTime, "DateTime" },
                { TypeCode.Decimal, "decimal" },
                { TypeCode.Double, "double" },
                { TypeCode.Int16, "short" },
                { TypeCode.Int32, "int" },
                { TypeCode.Int64, "long" },
                { TypeCode.SByte, "sbyte" },
                { TypeCode.Single, "single" },
                { TypeCode.UInt16, "ushort" },
                { TypeCode.UInt32, "uint" },
                { TypeCode.UInt64, "ulong" }
            };


        // This is just used internally to generate exception messages
        internal static string FormatValue(object value)
        {
            if (null == value) return "null";
            if (value is DBNull) return "DBNull";
            if (value is string) return "string '" + (value as string) + "'";
            var valueType = value.GetType();

            if (valueType.IsEnum) return "enum value '" + value.ToString() + "' of type " + valueType.FullName;

            TypeCode typeCode = System.Convert.GetTypeCode(value);
            if (_typeAliases.ContainsKey(typeCode)) return _typeAliases[typeCode] + " value " + value.ToString();

            return valueType.FullName + " value";
        }

        private static string FormatType(Type type)
        {
            if (type.IsEnum) return type.FullName;

            TypeCode typeCode = Type.GetTypeCode(type);
            if (_typeAliases.ContainsKey(typeCode)) return _typeAliases[typeCode];
            return type.FullName;
        }

    }
}
