namespace Ockham.Data
{
    /// <summary>
    /// A delegate which converts an input value to a predefined type, accounting for the provided <see cref="ConvertOptions"/>
    /// </summary>
    public delegate object ConverterDelegate(object value, ConvertOptions options);

    /// <summary>
    /// A delegate which converts an input value to type <typeparamref name="T"/>, accounting for the provided <see cref="ConvertOptions"/>
    /// </summary>
    public delegate T ConverterDelegate<T>(object value, ConvertOptions options);
}
