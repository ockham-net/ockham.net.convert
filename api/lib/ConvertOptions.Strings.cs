using System;

namespace Ockham.Data
{
    // *Immutable* set of string convert options
    public class StringConvertOptions : OptionSet
    {
        public StringConvertOptions(StringAsNullOption asNullOption, TrimStringFlags trimFlags) { }
        public StringAsNullOption AsNullOption { get; }
        public TrimStringFlags TrimFlags { get; }

        public bool WhitespaceAsNull => AsNullOption >= StringAsNullOption.Whitespace;
        public bool EmptyStringAsNull => AsNullOption >= StringAsNullOption.EmptyString;

        public bool TrimNone => TrimFlags == TrimStringFlags.None;
        public bool TrimStart => TrimFlags.HasFlag(TrimStringFlags.TrimStart);
        public bool TrimEnd => TrimFlags.HasFlag(TrimStringFlags.TrimEnd);
        public bool TrimAll => TrimFlags.HasFlag(TrimStringFlags.TrimAll);
    }

    public enum StringAsNullOption
    {
        NullReference = 0,
        EmptyString = 1,
        Whitespace = 2
    }

    [Flags]
    public enum TrimStringFlags
    {
        None = 0,
        TrimStart = 0x1,
        TrimEnd = 0x2,
        TrimAll = 0x3
    } 
}
