using System;
using System.Runtime.InteropServices;

namespace AnyBitStream
{
    /// <summary>
    /// Represents a 24-bit (3 byte) signed integer
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = sizeof(int))]
    public struct Int24 : ICustomType
    {
        /// <summary>
        /// The number of bits occupied by the type
        /// </summary>
        public const int BitSize = 24;
        /// <summary>
        /// The number of bytes required to store the type
        /// </summary>
        public const int ByteSize = 3;
        /// <summary>
        /// The minimum value the type can store
        /// </summary>
        public static readonly Int24 MinValue = (Int24)(-((1 << (BitSize - 1)) - 1));
        /// <summary>
        /// The maximum value the type can store
        /// </summary>
        public static readonly Int24 MaxValue = (Int24)((1 << (BitSize - 1)) - 1);
        public int TotalBits => BitSize;
        public int TotalBytes => ByteSize;
        internal readonly int _value;
        internal bool _sign;

        public Int24(long value)
        {
            _value = value < 0 ? -(int)value : (int)value & 0x7FFFFF;
            _sign = value < 0 ? true : false;
        }

        public Int24(byte[] bytes)
        {
            _value = bytes[0] + bytes[1] << 8 + (bytes[2] << 16 & 0x7F);
            _sign = (byte)(bytes[2] >> 7 & 0x1) == 0x1;
        }

        public Bit GetBit(int index) => index < BitSize - 1 ? (byte)(_value >> index & 0x1) : (_sign ? 1 : 0);

        public Bit[] GetBits()
        {
            var bits = new Bit[BitSize];
            for (byte i = 0; i < bits.Length - 1; i++)
                bits[i] = GetBit(i);
            bits[BitSize - 1] = _sign;
            return bits;
        }

        public byte[] GetBytes()
        {
            return new byte[BitSize / 8] {
                (byte)(_value & 0xFF),
                (byte)((_value >> 8) & 0xFF),
                (byte)(((_value >> 16) & 0xFF) + ((_sign ? 1 : 0) << 7))
            };
        }

        public static explicit operator Int24(int value) => new Int24(value);
        public static explicit operator int(Int24 i)
            => -((i._sign ? 1 : 0) << (BitSize - 1)) + i._value;
        public static bool operator ==(Int24 val1, Int24 val2) => val1.Equals(val2);
        public static bool operator !=(Int24 val1, Int24 val2) => !(val1.Equals(val2));
        public static Int24 operator -(Int24 a, long b) => new Int24((long)a._value - b);
        public static long operator -(long a, Int24 b) => a - b._value;
        public static Int24 operator +(Int24 a, long b) => new Int24((long)a._value + b);
        public static long operator +(long a, Int24 b) => a + b._value;
        public static Int24 operator *(Int24 a, long b) => new Int24((long)a._value * b);
        public static long operator *(long a, Int24 b) => a * b._value;
        public static Int24 operator /(Int24 a, long b) => new Int24((long)a._value / b);
        public static long operator /(long a, Int24 b) => a / b._value;
        public override bool Equals(object obj) => _value.Equals(obj);
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => _value.ToString();
        public bool Equals(long other) => _value == other;
        public bool Equals(int other) => _value == other;
        public bool Equals(short other) => _value == other;
        public bool Equals(byte other) => _value == other;
    }

    /// <summary>
    /// Represents a 24-bit (3 byte) unsigned integer
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = sizeof(int))]
    public struct UInt24 : ICustomType
    {
        /// <summary>
        /// The number of bits occupied by the type
        /// </summary>
        public const int BitSize = 24;
        /// <summary>
        /// The number of bytes required to store the type
        /// </summary>
        public const int ByteSize = 3;
        /// <summary>
        /// The minimum value the type can store
        /// </summary>
        public static readonly UInt24 MinValue = (UInt24)0;
        /// <summary>
        /// The maximum value the type can store
        /// </summary>
        public static readonly UInt24 MaxValue = (UInt24)((1 << BitSize) - 1);
        public int TotalBits => BitSize;
        public int TotalBytes => ByteSize;
        internal readonly int _value;

        public UInt24(ulong value)
        {
            _value = (int)(value & 0xFFFFFF);
        }

        public UInt24(byte[] bytes)
        {
            _value = bytes[0] + bytes[1] << 8 + bytes[2] << 16;
        }

        public Bit GetBit(int index) => (byte)(_value >> index & 0x1);

        public Bit[] GetBits() {
            var bits = new Bit[BitSize];
            for (byte i = 0; i < bits.Length; i++)
                bits[i] = GetBit(i);
            return bits;
        }

        public byte[] GetBytes()
        {
            return new byte[BitSize / 8] {
                (byte)(_value & 0xFF),
                (byte)((_value >> 8) & 0xFF),
                (byte)((_value >> 16) & 0xFF)
            };
        }

        public static explicit operator UInt24(ulong value) => new UInt24(value);
        public static explicit operator ulong(UInt24 i)
            => (ulong)(i._value & 0xFFFFFF);
        public static bool operator ==(UInt24 val1, UInt24 val2) => val1.Equals(val2);
        public static bool operator !=(UInt24 val1, UInt24 val2) => !(val1.Equals(val2));
        public static UInt24 operator -(UInt24 a, ulong b) => new UInt24((ulong)a._value - b);
        public static ulong operator -(ulong a, UInt24 b) => a - (ulong)b._value;
        public static UInt24 operator +(UInt24 a, ulong b) => new UInt24((ulong)a._value + b);
        public static ulong operator +(ulong a, UInt24 b) => a + (ulong)b._value;
        public static UInt24 operator *(UInt24 a, ulong b) => new UInt24((ulong)a._value * b);
        public static ulong operator *(ulong a, UInt24 b) => a * (ulong)b._value;
        public static UInt24 operator /(UInt24 a, ulong b) => new UInt24((ulong)a._value / b);
        public static ulong operator /(ulong a, UInt24 b) => a / (ulong)b._value;
        public override bool Equals(object obj) => _value.Equals(obj);
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => _value.ToString();
        public bool Equals(long other) => _value == other;
        public bool Equals(int other) => _value == other;
        public bool Equals(short other) => _value == other;
        public bool Equals(byte other) => _value == other;
    }
}
