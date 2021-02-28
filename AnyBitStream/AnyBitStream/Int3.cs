using System;
using System.Runtime.InteropServices;

namespace AnyBitStream
{
    /// <summary>
    /// Represents a 3-bit signed integer
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
    public struct Int3 : ICustomType
    {
        /// <summary>
        /// The number of bits occupied by the type
        /// </summary>
        public const int BitSize = 3;
        /// <summary>
        /// The number of bytes required to store the type
        /// </summary>
        public const int ByteSize = 1;
        /// <summary>
        /// The minimum value the type can store
        /// </summary>
        public static readonly Int3 MinValue = (Int3)(- ((1 << (BitSize - 1)) - 1));
        /// <summary>
        /// The maximum value the type can store
        /// </summary>
        public static readonly Int3 MaxValue = (Int3)((1 << (BitSize - 1)) - 1);
        public int TotalBits => BitSize;
        public int TotalBytes => ByteSize;
        internal readonly byte _value;
        internal bool _sign;

        public Int3(long value)
        {
            _value = (byte)(value < 0 ? -value : value & 0x3);
            _sign = value < 0;
        }

        public Bit GetBit(int index) => index < BitSize - 1 ? (byte)(_value >> index & 0x1) : (_sign ? 1 : 0);
        public Bit[] GetBits() => new Bit[BitSize] { GetBit(0), GetBit(1), _sign };

        public static explicit operator Int3(int value) => new Int3(value);
        public static explicit operator int(Int3 i) 
            => -((i._sign ? 1 : 0) << (BitSize - 1)) + i._value;
        public static bool operator ==(Int3 val1, Int3 val2) => val1.Equals(val2);
        public static bool operator !=(Int3 val1, Int3 val2) => !(val1.Equals(val2));
        public static Int3 operator -(Int3 a, long b) => new Int3(a._value - b);
        public static long operator -(long a, Int3 b) => a - b._value;
        public static Int3 operator +(Int3 a, long b) => new Int3(a._value + b);
        public static long operator +(long a, Int3 b) => a + b._value;
        public static Int3 operator *(Int3 a, long b) => new Int3(a._value * b);
        public static long operator *(long a, Int3 b) => a * b._value;
        public static Int3 operator /(Int3 a, long b) => new Int3(a._value / b);
        public static long operator /(long a, Int3 b) => a / b._value;
        public override bool Equals(object obj) => _value.Equals(obj);
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => _value.ToString();
        public bool Equals(long other) => _value == other;
        public bool Equals(int other) => _value == other;
        public bool Equals(short other) => _value == other;
        public bool Equals(byte other) => _value == other;
    }

    /// <summary>
    /// Represents a 3-bit unsigned integer
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
    public struct UInt3 : ICustomType
    {
        /// <summary>
        /// The number of bits occupied by the type
        /// </summary>
        public const int BitSize = 3;
        /// <summary>
        /// The number of bytes required to store the type
        /// </summary>
        public const int ByteSize = 1;
        /// <summary>
        /// The minimum value the type can store
        /// </summary>
        public static readonly UInt3 MinValue = (UInt3)0;
        /// <summary>
        /// The maximum value the type can store
        /// </summary>
        public static readonly UInt3 MaxValue = (UInt3)((1 << BitSize) - 1);
        public int TotalBits => BitSize;
        public int TotalBytes => ByteSize;
        internal readonly byte _value;

        public UInt3(ulong value)
        {
            _value = (byte)(value & 0x7);
        }

        public Bit GetBit(int index) => (byte)(_value >> index & 0x1);
        public Bit[] GetBits() => new Bit[BitSize] { GetBit(0), GetBit(1), GetBit(2) };

        public static explicit operator UInt3(ulong value) => new UInt3(value);
        public static explicit operator ulong(UInt3 i)
            => (ulong)(i._value & 0x7);
        public static bool operator ==(UInt3 val1, UInt3 val2) => val1.Equals(val2);
        public static bool operator !=(UInt3 val1, UInt3 val2) => !(val1.Equals(val2));
        public static UInt3 operator -(UInt3 a, ulong b) => new UInt3(a._value - b);
        public static ulong operator -(ulong a, UInt3 b) => a - b._value;
        public static UInt3 operator +(UInt3 a, ulong b) => new UInt3(a._value + b);
        public static ulong operator +(ulong a, UInt3 b) => a + b._value;
        public static UInt3 operator *(UInt3 a, ulong b) => new UInt3(a._value * b);
        public static ulong operator *(ulong a, UInt3 b) => a * b._value;
        public static UInt3 operator /(UInt3 a, ulong b) => new UInt3(a._value / b);
        public static ulong operator /(ulong a, UInt3 b) => a / b._value;
        public override bool Equals(object obj) => _value.Equals(obj);
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => _value.ToString();
        public bool Equals(long other) => _value == other;
        public bool Equals(int other) => _value == other;
        public bool Equals(short other) => _value == other;
        public bool Equals(byte other) => _value == other;
    }
}
