using System;
using System.Runtime.InteropServices;

namespace AnyBitStream
{
    /// <summary>
    /// Represents a 5-bit signed integer
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
    public struct Int5 : ICustomType
    {
        /// <summary>
        /// The number of bits occupied by the type
        /// </summary>
        public const int BitSize = 5;
        /// <summary>
        /// The number of bytes required to store the type
        /// </summary>
        public const int ByteSize = 1;
        /// <summary>
        /// The minimum value the type can store
        /// </summary>
        public static readonly Int5 MinValue = (Int5)(- ((1 << (BitSize - 1)) - 1));
        /// <summary>
        /// The maximum value the type can store
        /// </summary>
        public static readonly Int5 MaxValue = (Int5)((1 << (BitSize - 1)) - 1);
        public int TotalBits => BitSize;
        public int TotalBytes => ByteSize;
        internal readonly byte _value;
        internal bool _sign;

        public Int5(long value)
        {
            _value = (byte)(value < 0 ? -value : value & 0xF);
            _sign = value < 0;
        }

        public Bit GetBit(int index) => index < BitSize - 1 ? (byte)(_value >> index & 0x1) : (_sign ? 1 : 0);
        public Bit[] GetBits() => new Bit[BitSize] { GetBit(0), GetBit(1), GetBit(2), GetBit(3), _sign };

        public static explicit operator Int5(int value) => new Int5(value);
        public static explicit operator int(Int5 i)
            => -((i._sign ? 1 : 0) << (BitSize - 1)) + i._value;
        public static bool operator ==(Int5 val1, Int5 val2) => val1.Equals(val2);
        public static bool operator !=(Int5 val1, Int5 val2) => !(val1.Equals(val2));
        public static Int5 operator -(Int5 a, long b) => new Int5(a._value - b);
        public static long operator -(long a, Int5 b) => a - b._value;
        public static Int5 operator +(Int5 a, long b) => new Int5(a._value + b);
        public static long operator +(long a, Int5 b) => a + b._value;
        public static Int5 operator *(Int5 a, long b) => new Int5(a._value * b);
        public static long operator *(long a, Int5 b) => a * b._value;
        public static Int5 operator /(Int5 a, long b) => new Int5(a._value / b);
        public static long operator /(long a, Int5 b) => a / b._value;
        public override bool Equals(object obj) => _value.Equals(obj);
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => _value.ToString();
        public bool Equals(long other) => _value == other;
        public bool Equals(int other) => _value == other;
        public bool Equals(short other) => _value == other;
        public bool Equals(byte other) => _value == other;
    }

    /// <summary>
    /// Represents a 5-bit unsigned integer
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
    public struct UInt5 : ICustomType
    {
        /// <summary>
        /// The number of bits occupied by the type
        /// </summary>
        public const int BitSize = 5;
        /// <summary>
        /// The number of bytes required to store the type
        /// </summary>
        public const int ByteSize = 1;
        /// <summary>
        /// The minimum value the type can store
        /// </summary>
        public static readonly UInt5 MinValue = (UInt5)0;
        /// <summary>
        /// The maximum value the type can store
        /// </summary>
        public static readonly UInt5 MaxValue = (UInt5)((1 << BitSize) - 1);
        public int TotalBits => BitSize;
        public int TotalBytes => ByteSize;
        internal readonly byte _value;

        public UInt5(ulong value)
        {
            _value = (byte)(value & 0x1F);
        }

        public Bit GetBit(int index) => (byte)(_value >> index & 0x1);
        public Bit[] GetBits() => new Bit[BitSize] { GetBit(0), GetBit(1), GetBit(2), GetBit(3), GetBit(4) };

        public static explicit operator UInt5(ulong value) => new UInt5(value);
        public static explicit operator ulong(UInt5 i)
            => (ulong)(i._value & 0x1F);
        public static bool operator ==(UInt5 val1, UInt5 val2) => val1.Equals(val2);
        public static bool operator !=(UInt5 val1, UInt5 val2) => !(val1.Equals(val2));
        public static UInt5 operator -(UInt5 a, ulong b) => new UInt5(a._value - b);
        public static ulong operator -(ulong a, UInt5 b) => a - b._value;
        public static UInt5 operator +(UInt5 a, ulong b) => new UInt5(a._value + b);
        public static ulong operator +(ulong a, UInt5 b) => a + b._value;
        public static UInt5 operator *(UInt5 a, ulong b) => new UInt5(a._value * b);
        public static ulong operator *(ulong a, UInt5 b) => a * b._value;
        public static UInt5 operator /(UInt5 a, ulong b) => new UInt5(a._value / b);
        public static ulong operator /(ulong a, UInt5 b) => a / b._value;
        public override bool Equals(object obj) => _value.Equals(obj);
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => _value.ToString();
        public bool Equals(long other) => _value == other;
        public bool Equals(int other) => _value == other;
        public bool Equals(short other) => _value == other;
        public bool Equals(byte other) => _value == other;
    }
}
