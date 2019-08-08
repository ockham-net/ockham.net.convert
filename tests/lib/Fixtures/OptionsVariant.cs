using System;
using System.Collections.Generic;
using System.Text;

namespace Ockham.Data.Tests.Fixtures
{
    internal enum OptionsPreset
    {
        Default = 1,
        EmptyStringAsNull = 2,
        WhitespaceAsNull = 3,
        NullToValueDefault = 4
    }

    internal class OptionsVariant
    {
        public static ConvertOptions GetOptions(OptionsPreset preset, bool createNew = false)
        {
            if (createNew)
            {
                switch (preset)
                {
                    case OptionsPreset.Default:
                        return new ConvertOptions(ConvertOptionsBuilder.Default);
                    case OptionsPreset.EmptyStringAsNull:
                        return ConvertOptionsBuilder.Default.WithStringOptions(StringAsNullOption.EmptyString, TrimStringFlags.TrimAll).Options;
                    case OptionsPreset.WhitespaceAsNull:
                        return ConvertOptionsBuilder.Default.WithStringOptions(StringAsNullOption.Whitespace, TrimStringFlags.TrimAll).Options;
                    case OptionsPreset.NullToValueDefault:
                        return ConvertOptionsBuilder.Default.WithValueTypeOptions(ValueTypeConvertFlags.NullToValueDefault).Options;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            switch (preset)
            {
                case OptionsPreset.Default:
                    return ConvertOptions.Default;
                case OptionsPreset.EmptyStringAsNull:
                    return OptionsVariant.EmptyStringAsNull;
                case OptionsPreset.WhitespaceAsNull:
                    return OptionsVariant.WhitespaceAsNull;
                case OptionsPreset.NullToValueDefault:
                    return OptionsVariant.NullToValueDefault;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static ConvertOptions ParseBaseN { get; } = ConvertOptionsBuilder.Default.WithNumberOptions(ParseNumericStringFlags.HexString | ParseNumericStringFlags.OctalString | ParseNumericStringFlags.BinaryString).Options;

        public static ConvertOptions EmptyStringAsNull { get; } = GetOptions(OptionsPreset.EmptyStringAsNull, true);

        public static ConvertOptions WhitespaceAsNull { get; } = GetOptions(OptionsPreset.WhitespaceAsNull, true);

        public static ConvertOptions NullToValueDefault { get; } = GetOptions(OptionsPreset.NullToValueDefault, true);

        public OptionsVariant(ConvertOptions options, bool ignoreError, object defaultValue)
        {
            this.ConvertOptions = options;
            this.IgnoreError = ignoreError;
            this.DefaultValue = defaultValue;
        }

        public ConvertOptions ConvertOptions { get; set; }
        public bool IgnoreError { get; set; }
        public object DefaultValue { get; set; }

        /// <summary>
        /// Generate a list of all 12 permutations of convert options, ignore error, and default value
        /// </summary> 
        public static List<OptionsVariant> GetAll(object defaultValue, bool newOptionsInstance = false)
        {
            return new List<OptionsVariant>()
            {
                new OptionsVariant(GetOptions(OptionsPreset.Default, newOptionsInstance), false, null),
                new OptionsVariant(GetOptions(OptionsPreset.EmptyStringAsNull, newOptionsInstance), false, null),
                new OptionsVariant(GetOptions(OptionsPreset.NullToValueDefault, newOptionsInstance), false, null),

                new OptionsVariant(GetOptions(OptionsPreset.Default, newOptionsInstance), true, null),
                new OptionsVariant(GetOptions(OptionsPreset.EmptyStringAsNull, newOptionsInstance), true, null),
                new OptionsVariant(GetOptions(OptionsPreset.NullToValueDefault, newOptionsInstance), true, null),

                new OptionsVariant(GetOptions(OptionsPreset.Default, newOptionsInstance), false, defaultValue),
                new OptionsVariant(GetOptions(OptionsPreset.EmptyStringAsNull, newOptionsInstance), false, defaultValue),
                new OptionsVariant(GetOptions(OptionsPreset.NullToValueDefault, newOptionsInstance), false, defaultValue),

                new OptionsVariant(GetOptions(OptionsPreset.Default, newOptionsInstance), true, defaultValue),
                new OptionsVariant(GetOptions(OptionsPreset.EmptyStringAsNull, newOptionsInstance), true, defaultValue),
                new OptionsVariant(GetOptions(OptionsPreset.NullToValueDefault, newOptionsInstance), true, defaultValue)
            };
        }


    }

}
