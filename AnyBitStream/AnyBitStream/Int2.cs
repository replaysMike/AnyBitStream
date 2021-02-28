using System;
using System.Runtime.InteropServices;

namespace AnyBitStream
{
    /// <summary>
    /// Represents a 2-bit signed integer
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
    public struct Int2 : ICustomType
    {
        /// <summary>
        /// The number of bits occupied by the type
        /// </summary>
        public const int BitSize = 2;
        /// <summary>
        /// The number of bytes required to store the type
        /// </summary>
        public const int ByteSize = 1;
        /// <summary>
        /// The minimum value the type can store
        /// </summary>
        public static readonly Int2 MinValue = (Int2)(-((1 << (BitSize - 1)) - 1));
        /// <summary>
        /// The maximum value the type can store
        /// </summary>
        public static readonly Int2 MaxValue = (Int2)((1 << (BitSize - 1)) - 1);
        public int TotalBits => BitSize;
        public int TotalBytes => ByteSize;
        internal readonly byte _value;
        internal bool _sign;

        public Int2(long value)
        {
            _value = (byte)(value < 0 ? -value : value & 0x1);
            _sign = value < 0;
        }

        public Bit GetBit(int index) => index < BitSize - 1 ? (byte)(_value >> index & 0x1) : (_sign ? 1 : 0);
        public Bit[] GetBits() => new Bit[BitSize] { GetBit(0), _sign };

        public static explicit operator Int2(long value) => new Int2(value);
        public static explicit operator long(Int2 i)
            => -((i._sign ? 1 : 0) << (BitSize - 1)) + i._value;

        public static bool operator ==(Int2 val1, Int2 val2) => val1.Equals(val2);
        public static bool operator !=(Int2 val1, Int2 val2) => !(val1.Equals(val2));
        public static Int2 operator -(Int2 a, long b) => new Int2(a._value - b);
        public static long operator -(long a, Int2 b) => a - b._value;
        public static Int2 operator +(Int2 a, long b) => new Int2(a._value + b);
        public static long operator +(long a, Int2 b) => a + b._value;
        public static Int2 operator *(Int2 a, long b) => new Int2(a._value * b);
        public static long operator *(long a, Int2 b) => a * b._value;
        public static Int2 operator /(Int2 a, long b) => new Int2(a._value / b);
        public static long operator /(long a, Int2 b) => a / b._value;
        public override bool Equals(object obj) => _value.Equals(obj);
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => _value.ToString();
        public bool Equals(long other) => _value == other;
        public bool Equals(int other) => _value == other;
        public bool Equals(short other) => _value == other;
        public bool Equals(byte other) => _value == other;
    }

    /// <summary>
    /// Represents a 2-bit unsigned integer
    /// </summary>
    [Serializable]
    public struct UInt2 : ICustomType
    {
        /// <summary>
        /// The number of bits occupied by the type
        /// </summary>
        public const int BitSize = 2;
        /// <summary>
        /// The number of bytes required to store the type
        /// </summary>
        public const int ByteSize = 1;
        /// <summary>
        /// The minimum value the type can store
        /// </summary>
        public static readonly UInt2 MinValue = (UInt2)0;
        /// <summary>
        /// The maximum value the type can store
        /// </summary>
        public static readonly UInt2 MaxValue = (UInt2)((1 << BitSize) - 1);
        public int TotalBits => BitSize;
        public int TotalBytes => ByteSize;
        internal readonly byte _value;

        public UInt2(ulong value)
        {
            _value = (byte)(value & 0x3);
        }

        public Bit GetBit(int index) => (byte)(_value >> index & 0x1);
        public Bit[] GetBits() => new Bit[BitSize] { GetBit(0), GetBit(1) };

        public static explicit operator UInt2(ulong value) => new UInt2(value);
        public static explicit operator ulong(UInt2 i)
            => (uint)(i._value & 0xF);
        public static bool operator ==(UInt2 val1, UInt2 val2) => val1.Equals(val2);
        public static bool operator !=(UInt2 val1, UInt2 val2) => !(val1.Equals(val2));
        public static UInt2 operator -(UInt2 a, ulong b) => new UInt2(a._value - b);
        public static ulong operator -(ulong a, UInt2 b) => a - b._value;
        public static UInt2 operator +(UInt2 a, ulong b) => new UInt2(a._value + b);
        public static ulong operator +(ulong a, UInt2 b) => a + b._value;
        public static UInt2 operator *(UInt2 a, ulong b) => new UInt2(a._value * b);
        public static ulong operator *(ulong a, UInt2 b) => a * b._value;
        public static UInt2 operator /(UInt2 a, ulong b) => new UInt2(a._value / b);
        public static ulong operator /(ulong a, UInt2 b) => a / b._value;
        public override bool Equals(object obj) => _value.Equals(obj);
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => _value.ToString();
        public bool Equals(long other) => _value == other;
        public bool Equals(int other) => _value == other;
        public bool Equals(short other) => _value == other;
        public bool Equals(byte other) => _value == other;
    }
}
