using System;

namespace Ockham.Data
{
    /// <summary>
    /// Settings for processing strings during conversion. This class is immutable
    /// </summary>
    public class StringConvertOptions : OptionSet
    {
        /// <summary>
        /// <see cref="StringConvertOptions"/> that matches BCL behavior. Empty or whitespace strings
        /// are not treated as null, but strings are trimmed before converting to other types. 
        /// Equivalent to <see cref="StringAsNullOption.NullReference"/> and <see cref="TrimStringFlags.TrimAll"/>
        /// </summary>
        public static StringConvertOptions Default { get; }
            = new StringConvertOptions(StringAsNullOption.NullReference, TrimStringFlags.TrimAll);

        /// <summary>
        /// Create a new <see cref="StringConvertOptions"/>
        /// </summary>
        /// <param name="asNullOption">Determines which strings to treat as null</param>
        /// <param name="trimFlags">Flags for indicating whether strings should be trimmed</param>
        public StringConvertOptions(StringAsNullOption asNullOption, TrimStringFlags trimFlags)
        {
            this.AsNullOption = asNullOption;
            this.TrimFlags = trimFlags;

            this.WhitespaceAsNull = AsNullOption >= StringAsNullOption.Whitespace;
            this.EmptyStringAsNull = AsNullOption >= StringAsNullOption.EmptyString;

            this.TrimNone = TrimFlags == TrimStringFlags.None;
            this.TrimStart = TrimFlags.HasFlag(TrimStringFlags.TrimStart);
            this.TrimEnd = TrimFlags.HasFlag(TrimStringFlags.TrimEnd);
            this.TrimAll = TrimFlags.HasFlag(TrimStringFlags.TrimAll);
        }

        /// <summary>
        /// Determines which strings to treat as null
        /// </summary>
        public StringAsNullOption AsNullOption { get; }

        /// <summary>
        /// Flags for indicating whether strings should be trimmed
        /// </summary>
        public TrimStringFlags TrimFlags { get; }

        /// <summary>
        /// Whether to treat empty or whitespace strings as null
        /// </summary>
        public bool WhitespaceAsNull { get; }

        /// <summary>
        /// Whether to treat empty strings as null 
        /// </summary>
        public bool EmptyStringAsNull { get; }

        /// <summary>
        /// If true, strings are not trimmed before converting to other types
        /// </summary>
        public bool TrimNone { get; }

        /// <summary>
        /// Whether to trim the start of a string before converting to another type
        /// </summary>
        public bool TrimStart { get; }

        /// <summary>
        /// Whether to trim the end of a string before converting to another type
        /// </summary>
        public bool TrimEnd { get; }

        /// <summary>
        /// Whether to trim both the start and end of a string before converting to another type
        /// </summary>
        public bool TrimAll { get; }
    }

    /// <summary>
    /// Determines which strings to treat as null
    /// </summary>
    public enum StringAsNullOption
    {
        /// <summary>
        /// Only treat a null reference as null
        /// </summary>
        NullReference = 0,

        /// <summary>
        /// Treat a non-null empty string as null
        /// </summary>
        EmptyString = 1,

        /// <summary>
        /// Treat a non-null whitespace string as null
        /// </summary>
        Whitespace = 2
    }

    /// <summary>
    /// Flags for indicating whether strings should be trimmed
    /// </summary>
    [Flags]
    public enum TrimStringFlags
    {
        /// <summary>
        /// Do not trim
        /// </summary>
        None = 0,

        /// <summary>
        /// Trim the start
        /// </summary>
        TrimStart = 0x1,

        /// <summary>
        /// Trim the end
        /// </summary>
        TrimEnd = 0x2,

        /// <summary>
        /// Trim both start and end. Alias for (<see cref="TrimStart"/> | <see cref="TrimEnd"/>)
        /// </summary>
        TrimAll = 0x3
    }
}
