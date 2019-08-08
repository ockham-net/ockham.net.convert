using System;

namespace Ockham.Data
{
    /// <summary>
    /// A utility for converting values to <see cref="Guid"/>
    /// </summary>
    public static class GuidConverter
    {
        /// <summary>
        /// Convert the provided value to a <see cref="Guid"/>
        /// </summary>  
        public static Guid ToGuid(object value, ConvertOptions options)
        {
            if (value is Guid) return (Guid)value;
            if (value is string stringValue)
            {
                return new Guid(stringValue);
            }
            else if (value is byte[] bytes)
            {
                return new Guid(bytes);
            }

            // If input is not a Guid, this will throw an invalid cast exception in the localized language:
            return (Guid)value;
        }
    }
}
