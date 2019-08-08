using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Ockham.Data
{
    internal static class EnumConverter
    {
        private static readonly Dictionary<Type, bool> _flagsEnums = new Dictionary<Type, bool>();
        private static readonly Dictionary<Type, IReadOnlyDictionary<string, ulong>> _flagNames = new Dictionary<Type, IReadOnlyDictionary<string, ulong>>();
        private static readonly Dictionary<Type, ulong> _flagMasks = new Dictionary<Type, ulong>();

        [ExcludeFromCodeCoverage]
        private static bool IsFlags(Type enumType)
        {
            if (_flagsEnums.TryGetValue(enumType, out bool isFlags)) return isFlags;
            lock (_flagsEnums)
            {
                if (_flagsEnums.TryGetValue(enumType, out isFlags)) return isFlags;
                return _flagsEnums[enumType] = enumType.GetCustomAttributes<FlagsAttribute>().Any();
            }
        }

        [ExcludeFromCodeCoverage]
        private static IReadOnlyDictionary<string, ulong> DefinedFlagNames(Type enumType)
        {
            if (_flagNames.TryGetValue(enumType, out IReadOnlyDictionary<string, ulong> flags)) return flags;
            lock (_flagNames)
            {
                if (_flagNames.TryGetValue(enumType, out flags)) return flags;
                return (_flagNames[enumType] =
                    Enum.GetNames(enumType).ToDictionary(
                        name => name,
                        name => System.Convert.ToUInt64(Enum.Parse(enumType, name)),
                        StringComparer.OrdinalIgnoreCase
                    )
                );
            }
        }

        [ExcludeFromCodeCoverage]
        private static ulong GetFlagMask(Type enumType)
        {
            if (_flagMasks.TryGetValue(enumType, out ulong mask)) return mask;
            lock (_flagMasks)
            {
                if (_flagMasks.TryGetValue(enumType, out mask)) return mask;
                return (_flagMasks[enumType] =
                    Enum.GetValues(enumType).Cast<object>().Aggregate(
                        (ulong)0,
                        (fmask, value) => fmask | System.Convert.ToUInt64(value)
                    )
                );
            }
        }

        public static object ToEnumValue(object value, Type targetType, ConvertOptions options, bool ignoreError, object valueOnError)
        {
            // Test IsNumeric BEFORE testing string so that numeric strings are
            // treated as numbers, not as enum member names
            if (Value.IsNumeric(value, options))
            {
                // If the input is a number or *numeric string*, first convert the 
                // input to an enum number value, then cast it using Enum.ToObject

                // Note: DON'T ignore error on conversion to underlying type
                var rawValue = Convert.ToStructValue(value, Enum.GetUnderlyingType(targetType), options, false, null);

                return (options.CoerceEnumValues || Enum.IsDefined(targetType, rawValue))
                    ? Enum.ToObject(targetType, rawValue)
                    : FilterEnumValues(rawValue, targetType, options);
            }
            else if (value is string)
            {
                // Input is a string but Value.IsNumeric did not recognize it
                // as a number. So, treat the input as an enum member name

                string sEnumName = Regex.Replace((string)value, @"[\s\r\n]+", "");
                if (!options.IgnoreEnumNames) return Enum.Parse(targetType, sEnumName, true);

                try
                {
                    return Enum.Parse(targetType, sEnumName, true);
                }
                catch
                {
                    return FilterEnumNames(sEnumName, targetType);
                }
            }
            else
            {
                // Fallback: Attempt to convert the input to the underlying numeric type, even if
                // Value.IsNumeric returned false

                // Note: DON'T ignore error on underlying conversion type
                var rawValue = Convert.ToStructValue(value, Enum.GetUnderlyingType(targetType), options, false, null);

                return (options.CoerceEnumValues || Enum.IsDefined(targetType, rawValue))
                    ? Enum.ToObject(targetType, rawValue)
                    : FilterEnumValues(rawValue, targetType, options);
            }
        }

        private static object FilterEnumNames(string rawNames, Type targetType)
        {
            // Not a flags enum, and name not found. Return default value.
            if (!IsFlags(targetType)) return Activator.CreateInstance(targetType);

            var flagNames = rawNames.Split(',');

            // Only one flag name, and it was not found. Return default value.
            if (flagNames.Length == 1) return Activator.CreateInstance(targetType);

            // Need to filter flags
            int lng = flagNames.Length;
            var flagValues = DefinedFlagNames(targetType);
            ulong rawFlags = 0;
            for (int i = 0; i < lng; i++)
            {
                string flag = flagNames[i].Trim();
                if (flagValues.TryGetValue(flag, out ulong flagValue))
                {
                    rawFlags |= flagValue;
                }
            }

            return Enum.ToObject(targetType, rawFlags);
        }

        private static object FilterEnumValues(object rawValue, Type targetType, ConvertOptions options)
        {
            // Enum is not a flags, and value is not defined
            if (!IsFlags(targetType))
            {
                if (options.ThrowEnumValues) throw new InvalidCastException($"Value {rawValue} is not defined for enum type {targetType.Name}");
                return Activator.CreateInstance(targetType);
            }

            // Filter flag bits
            ulong rawFlags = System.Convert.ToUInt64(rawValue);
            ulong flags = 0;
            ulong bit = 1;
            var mask = GetFlagMask(targetType);

            for (int i = 0; i < 64; i++)
            {
                // Bit is in raw flags 
                if ((rawFlags & bit) == bit)
                {
                    if ((mask & bit) == bit)
                    {
                        // Bit is defined in enum
                        flags |= bit;
                    }
                    else if (options.ThrowEnumValues)
                    {
                        // Bit is not defined
                        throw new InvalidCastException($"Bit 0x{bit:X} is not defined for flags enum type {targetType.Name}");
                    }
                }
                bit <<= 1;
            }

            return Enum.ToObject(targetType, flags);
        }
    }
}
