using System;
using System.Diagnostics.CodeAnalysis;

namespace Ockham.Data.Tests.Fixtures
{
    [ExcludeFromCodeCoverage]
    public class ComplexNumberConvertOptions : OptionSet
    {
        public ComplexNumberConvertOptions(ComplexNumberElement convertToFlags)
        {
            this.ConvertToFlags = convertToFlags;
        }
        public ComplexNumberElement ConvertToFlags { get; }

        // Do not convert plain numbers to ComplexNumber
        public bool DoNotConvert => ConvertToFlags == ComplexNumberElement.None;

        // Convert plain number as real part of ComplexNumber
        public bool ToReal => ConvertToFlags == ComplexNumberElement.Real;

        // Convert plain number as imaginary part of ComplexNumber
        public bool ToImaginary => ConvertToFlags == ComplexNumberElement.Imaginary;
    }

    [Flags]
    public enum ComplexNumberElement
    {
        None = 0x0,
        Real = 0x1,
        Imaginary = 0x2
    }

    [ExcludeFromCodeCoverage]
    public static class ComplexNumberConvertOptionsExtensions
    {

        // allows options.ComplexNumbers()
        public static ComplexNumberConvertOptions ComplexNumbers(this ConvertOptions options)
            => options.GetOptions<ComplexNumberConvertOptions>();

        // allows build.WithComplexNumberOptions(...)
        public static ConvertOptionsBuilder WithComplexNumberOptions(
            this ConvertOptionsBuilder builder,
            ComplexNumberElement convertToFlags
        ) => builder.WithOptions(new ComplexNumberConvertOptions(convertToFlags));
    }
}
