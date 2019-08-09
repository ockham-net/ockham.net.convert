using System;

namespace Ockham.Data
{
    // Methods for detecting empty values, and converting to or from empty value types
    public partial class Converter
    {
        /// <summary>
        /// Test whether the input value is null or DBNull. Empty or whitespace strings may also return true 
        /// depending on the settings of this <see cref="Converter"/>
        /// </summary> 
        public bool IsNull(object value)
        {
            return Value.IsNull(value, this.Options);
        }

        /// <summary>
        /// Converts null or DBNull to null. Empty or whitespace strings may also be converted to DBNull 
        /// depending on the settings of this <see cref="Converter"/>
        /// </summary>
        /// <param name="value">A value of any type</param> 
        public object ToNull(object value)
        {
            return (IsNull(value) ? null : value);
        }

        /// <summary>
        /// Converts null or DBNull to DBNull. Empty or whitespace strings may also be converted to DBNull 
        /// depending on the settings of this <see cref="Converter"/>
        /// </summary>
        /// <param name="value">A value of any type</param> 
        public object ToDBNull(object value)
        {
            return (IsNull(value) ? DBNull.Value : value);
        }
    }
}