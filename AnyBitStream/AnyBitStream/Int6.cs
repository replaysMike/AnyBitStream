using System;
using System.Runtime.InteropServices;

namespace AnyBitStream
{
    /// <summary>
    /// Represents a 6-bit signed integer
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
    public struct Int6 : ICustomType
    {
        /// <summary>
        /// The number of bits occupied by the type
        /// </summary>
        public const int BitSize = 6;
        /// <summary>
        /// The number of bytes required to store the type
        /// </summary>
        public const int ByteSize = 1;
        /// <summary>
        /// The minimum value the type can store
        /// </summary>
        public static readonly Int6 MinValue = (Int6)(-((1 << (BitSize - 1)) - 1));
        /// <summary>
        /// The maximum value the type can store
        /// </summary>
        public static readonly Int6 MaxValue = (Int6)((1 << (BitSize - 1)) - 1);
        public int TotalBits => BitSize;
        public int TotalBytes => ByteSize;
        internal readonly byte _value;
        internal bool _sign;

        public Int6(long value)
        {
            _value = (byte)(value < 0 ? -value : value & 0x1F);
            _sign = value < 0;
        }

        public Bit GetBit(int index) => (_value >> index) & 0x1;
        public Bit[] GetBits() => new Bit[BitSize] { GetBit(0), GetBit(1), GetBit(2), GetBit(3), GetBit(4), _sign };

        public static explicit operator Int6(int value) => new Int6(value);
        public static explicit operator int(Int6 i)
            => -((i._sign ? 1 : 0) << (BitSize - 1)) + i._value;
        public static bool operator ==(Int6 val1, Int6 val2) => val1.Equals(val2);
        public static bool operator !=(Int6 val1, Int6 val2) => !(val1.Equals(val2));
        public static Int6 operator -(Int6 a, long b) => new Int6(a._value - b);
        public static long operator -(long a, Int6 b) => a - b._value;
        public static Int6 operator +(Int6 a, long b) => new Int6(a._value + b);
        public static long operator +(long a, Int6 b) => a + b._value;
        public static Int6 operator *(Int6 a, long b) => new Int6(a._value * b);
        public static long operator *(long a, Int6 b) => a * b._value;
        public static Int6 operator /(Int6 a, long b) => new Int6(a._value / b);
        public static long operator /(long a, Int6 b) => a / b._value;
        public override bool Equals(object obj) => _value.Equals(obj);
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => _value.ToString();
        public bool Equals(long other) => _value == other;
        public bool Equals(int other) => _value == other;
        public bool Equals(short other) => _value == other;
        public bool Equals(byte other) => _value == other;
    }

    /// <summary>
    /// Represents a 6-bit unsigned integer
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
    public struct UInt6 : ICustomType
    {
        /// <summary>
        /// The number of bits occupied by the type
        /// </summary>
        public const int BitSize = 6;
        /// <summary>
        /// The number of bytes required to store the type
        /// </summary>
        public const int ByteSize = 1;
        /// <summary>
        /// The minimum value the type can store
        /// </summary>
        public static readonly UInt6 MinValue = (UInt6)0;
        /// <summary>
        /// The maximum value the type can store
        /// </summary>
        public static readonly UInt6 MaxValue = (UInt6)((1 << BitSize) - 1);
        public int TotalBits => BitSize;
        public int TotalBytes => ByteSize;
        internal readonly byte _value;

        public UInt6(ulong value)
        {
            _value = (byte)(value & 0x3F);
        }

        public Bit GetBit(int index) => (byte)(_value >> index & 0x1);
        public Bit[] GetBits() => new Bit[BitSize] { GetBit(0), GetBit(1), GetBit(2), GetBit(3), GetBit(4), GetBit(5) };

        public static explicit operator UInt6(ulong value) => new UInt6(value);
        public static explicit operator ulong(UInt6 i)
            => (ulong)(i._value & 0x3F);
        public static bool operator ==(UInt6 val1, UInt6 val2) => val1.Equals(val2);
        public static bool operator !=(UInt6 val1, UInt6 val2) => !(val1.Equals(val2));
        public static UInt6 operator -(UInt6 a, ulong b) => new UInt6(a._value - b);
        public static ulong operator -(ulong a, UInt6 b) => a - b._value;
        public static UInt6 operator +(UInt6 a, ulong b) => new UInt6(a._value + b);
        public static ulong operator +(ulong a, UInt6 b) => a + b._value;
        public static UInt6 operator *(UInt6 a, ulong b) => new UInt6(a._value * b);
        public static ulong operator *(ulong a, UInt6 b) => a * b._value;
        public static UInt6 operator /(UInt6 a, ulong b) => new UInt6(a._value / b);
        public static ulong operator /(ulong a, UInt6 b) => a / b._value;
        public override bool Equals(object obj) => _value.Equals(obj);
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => _value.ToString();
        public bool Equals(long other) => _value == other;
        public bool Equals(int other) => _value == other;
        public bool Equals(short other) => _value == other;
        public bool Equals(byte other) => _value == other;
    }
}
