namespace Ockham.Data.Tests.Fixtures
{
    public struct Amount
    {
        public double Value { get; }

        public Amount(double value) { this.Value = value; }

        private long IntegerValue => System.Convert.ToInt64(this.Value);

        public override string ToString()
        {
            return "Amount(" + Value.ToString() + ")";
        }

        public static explicit operator Amount(int value) => new Amount(value);

        public static implicit operator bool(Amount amount) => amount.Value > 0.0;
        public static implicit operator byte(Amount amount) => (byte)(amount.IntegerValue % (1L + byte.MaxValue));
        public static implicit operator sbyte(Amount amount) => (sbyte)(amount.IntegerValue % (1L + sbyte.MaxValue));
        public static implicit operator short(Amount amount) => (short)(amount.IntegerValue % (1L + short.MaxValue));
        public static implicit operator ushort(Amount amount) => (ushort)(amount.IntegerValue % (1L + ushort.MaxValue));
        public static implicit operator int(Amount amount) => (int)(amount.IntegerValue % (1L + int.MaxValue));
        public static implicit operator uint(Amount amount) => (uint)(amount.IntegerValue % (1L + uint.MaxValue));
        public static implicit operator long(Amount amount) => amount.IntegerValue;
        public static implicit operator ulong(Amount amount) => (ulong)amount.IntegerValue;
        public static implicit operator decimal(Amount amount) => amount.IntegerValue;
        public static implicit operator double(Amount amount) => amount.IntegerValue;
        public static implicit operator float(Amount amount) => amount.IntegerValue;
    }
}
