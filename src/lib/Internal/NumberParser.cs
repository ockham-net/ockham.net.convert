using System;
using System.Text.RegularExpressions;

namespace Ockham.Data
{
    using SysConvert = System.Convert;

    internal class NumberParser
    {
        public static string TrimCheck(string stringValue, ConvertOptions options)
        {
            var stringOpts = options.Strings;

            if (stringOpts.TrimStart)
            {
                stringValue = stringValue.TrimStart();
            }
            else if (Regex.IsMatch(stringValue, @"^[\s\r\n]+"))
            {
                throw new FormatException("String starts with disallowed whitespace");
            }

            if (stringOpts.TrimEnd)
            {
                stringValue = stringValue.TrimEnd();
            }
            else if (Regex.IsMatch(stringValue, @"[\s\r\n]+$"))
            {
                throw new FormatException("String ends with disallowed whitespace");
            }

            return stringValue;
        }

        public static object Parse(string stringValue, Type targetType, ConvertOptions options, TypeCode typeCode)
        {
            // Note if options.TrimAll is true, nothing needs to be done because default 
            // behavior of System.ChangeType is to trim the string first
            if (!options.TrimAll) stringValue = TrimCheck(stringValue, options);

            if ((options.ParseBaseN || options.AllowSeparator) && Value.IsNumeric(stringValue, options.ParseFlags, out int @base, out bool hasSeparator))
            {
                if (hasSeparator) stringValue = stringValue.Replace("_", "");

                if (@base != 10)
                {
                    stringValue = stringValue.Trim().Substring(2);
                    switch (typeCode)
                    {
                        case TypeCode.Byte:
                            return SysConvert.ToByte(stringValue, @base);
                        case TypeCode.Decimal:
                            return SysConvert.ToDecimal(SysConvert.ToUInt64(stringValue, @base));
                        case TypeCode.Double:
                            return SysConvert.ToDouble(SysConvert.ToUInt64(stringValue, @base));
                        case TypeCode.Int16:
                            return SysConvert.ToInt16(stringValue, @base);
                        case TypeCode.Int32:
                            return SysConvert.ToInt32(stringValue, @base);
                        case TypeCode.Int64:
                            return SysConvert.ToInt64(stringValue, @base);
                        case TypeCode.SByte:
                            return SysConvert.ToSByte(stringValue, @base);
                        case TypeCode.Single:
                            return SysConvert.ToSingle(SysConvert.ToUInt64(stringValue, @base));
                        case TypeCode.UInt16:
                            return SysConvert.ToUInt16(stringValue, @base);
                        case TypeCode.UInt32:
                            return SysConvert.ToUInt32(stringValue, @base);
                        case TypeCode.UInt64:
                            return SysConvert.ToUInt64(stringValue, @base);
                    }
                }
                else
                {
                    try
                    {
                        // Use built-in number parsers
                        return SysConvert.ChangeType(stringValue, targetType);
                    }
                    catch { }

                    // double.Parse() supports a wider range of notations, such as '1242.34e+12',
                    // than System.Convert.ToInt** methods
                    return SysConvert.ChangeType(double.Parse(stringValue), targetType);
                }
            }

            return null;
        }
    }
}
