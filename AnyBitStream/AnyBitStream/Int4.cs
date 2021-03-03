using System;
using System.Runtime.InteropServices;

namespace AnyBitStream
{
    /// <summary>
    /// Represents a 4-bit (nibble) signed integer
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
    public struct Int4 : ICustomType, IEquatable<Int4>, IEquatable<UInt4>
    {
        /// <summary>
        /// The number of bits occupied by the type
        /// </summary>
        public const int BitSize = 4;
        /// <summary>
        /// The number of bytes required to store the type
        /// </summary>
        public const int ByteSize = 1;
        /// <summary>
        /// The minimum value the type can store
        /// </summary>
        public static readonly Int4 MinValue = (Int4)(-((1 << (BitSize - 1)) - 1));
        /// <summary>
        /// The maximum value the type can store
        /// </summary>
        public static readonly Int4 MaxValue = (Int4)((1 << (BitSize - 1)) - 1);
        public int TotalBits => BitSize;
        public int TotalBytes => ByteSize;
        internal readonly byte _value;
        internal bool _sign;

        public Int4(long value)
        {
            _value = (byte)(value < 0 ? -value : value & 0x7);
            _sign = value < 0;
        }

        public Bit GetBit(int index) => (Bit)(index < BitSize - 1 ? (byte)(_value >> index & 0x1) : (_sign ? 1 : 0));
        public Bit[] GetBits() => new Bit[BitSize] { GetBit(0), GetBit(1), GetBit(2), _sign };

        public static explicit operator Int4(int value) => new Int4(value);
        public static explicit operator int(Int4 i)
            => -((i._sign ? 1 : 0) << (BitSize - 1)) + i._value;
        public static bool operator ==(Int4 val1, Int4 val2) => val1.Equals(val2);
        public static bool operator !=(Int4 val1, Int4 val2) => !(val1.Equals(val2));
        public static bool operator ==(Int4 val1, object val2) => val1.Equals(val2);
        public static bool operator !=(Int4 val1, object val2) => !(val1.Equals(val2));
        public static bool operator ==(Int4 val1, long val2) => val1.Equals(val2);
        public static bool operator !=(Int4 val1, long val2) => !(val1.Equals(val2));
        public static bool operator ==(long val2, Int4 val1) => val1.Equals(val2);
        public static bool operator !=(long val2, Int4 val1) => !(val1.Equals(val2));
        public static Int4 operator -(Int4 a, long b) => new Int4(a._value - b);
        public static long operator -(long a, Int4 b) => a - b._value;
        public static Int4 operator +(Int4 a, long b) => new Int4(a._value + b);
        public static long operator +(long a, Int4 b) => a + b._value;
        public static Int4 operator *(Int4 a, long b) => new Int4(a._value * b);
        public static long operator *(long a, Int4 b) => a * b._value;
        public static Int4 operator /(Int4 a, long b) => new Int4(a._value / b);
        public static long operator /(long a, Int4 b) => a / b._value;
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (obj is Int4 other)
                return Equals(other);
            if (obj is UInt4 other2)
                return _value == other2._value && !_sign;
            if (obj is byte b)
                return _value == b;
            if (obj is short s)
                return _value == s;
            if (obj is int i)
                return _value == i;
            if (obj is long l)
                return _value == l;
            if (obj is uint ui)
                return _value == ui;
            if (obj is ushort us)
                return _value == us;
            if (obj is ulong ul)
                return _value == ul;
            return false;
        }
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => _value.ToString();
        public bool Equals(Int4 other) => _value == other._value && _sign == other._sign;
        public bool Equals(UInt4 other) => _value == other._value && !_sign;
        public bool Equals(long other) => _value == other;
        public bool Equals(int other) => _value == other;
        public bool Equals(short other) => _value == other;
        public bool Equals(byte other) => _value == other;
    }

    /// <summary>
    /// Represents a 4-bit (nibble) unsigned integer
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
    public struct UInt4 : ICustomType, IEquatable<UInt4>, IEquatable<Int4>
    {
        /// <summary>
        /// The number of bits occupied by the type
        /// </summary>
        public const int BitSize = 4;
        /// <summary>
        /// The number of bytes required to store the type
        /// </summary>
        public const int ByteSize = 1;
        /// <summary>
        /// The minimum value the type can store
        /// </summary>
        public static readonly UInt4 MinValue = (UInt4)0;
        /// <summary>
        /// The maximum value the type can store
        /// </summary>
        public static readonly UInt4 MaxValue = (UInt4)((1 << BitSize) - 1);
        public int TotalBits => BitSize;
        public int TotalBytes => ByteSize;
        internal readonly byte _value;

        public UInt4(ulong value)
        {
            _value = (byte)(value & 0xF);
        }

        public Bit GetBit(int index) => (Bit)(_value >> index & 0x1);
        public Bit[] GetBits() => new Bit[BitSize] { GetBit(0), GetBit(1), GetBit(2), GetBit(3) };

        public static explicit operator UInt4(ulong value) => new UInt4(value);
        public static explicit operator ulong(UInt4 i)
            => (ulong)(i._value & 0xF);
        public static bool operator ==(UInt4 val1, UInt4 val2) => val1.Equals(val2);
        public static bool operator !=(UInt4 val1, UInt4 val2) => !(val1.Equals(val2));
        public static bool operator ==(UInt4 val1, object val2) => val1.Equals(val2);
        public static bool operator !=(UInt4 val1, object val2) => !(val1.Equals(val2));
        public static bool operator ==(UInt4 val1, long val2) => val1.Equals(val2);
        public static bool operator !=(UInt4 val1, long val2) => !(val1.Equals(val2));
        public static bool operator ==(long val2, UInt4 val1) => val1.Equals(val2);
        public static bool operator !=(long val2, UInt4 val1) => !(val1.Equals(val2));
        public static UInt4 operator -(UInt4 a, ulong b) => new UInt4(a._value - b);
        public static ulong operator -(ulong a, UInt4 b) => a - b._value;
        public static UInt4 operator +(UInt4 a, ulong b) => new UInt4(a._value + b);
        public static ulong operator +(ulong a, UInt4 b) => a + b._value;
        public static UInt4 operator *(UInt4 a, ulong b) => new UInt4(a._value * b);
        public static ulong operator *(ulong a, UInt4 b) => a * b._value;
        public static UInt4 operator /(UInt4 a, ulong b) => new UInt4(a._value / b);
        public static ulong operator /(ulong a, UInt4 b) => a / b._value;
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (obj is UInt4 other)
                return Equals(other);
            if (obj is Int4 other2)
                return _value == other2._value && !other2._sign;
            if (obj is byte b)
                return _value == b;
            if (obj is short s)
                return _value == s;
            if (obj is int i)
                return _value == i;
            if (obj is long l)
                return _value == l;
            if (obj is uint ui)
                return _value == ui;
            if (obj is ushort us)
                return _value == us;
            if (obj is ulong ul)
                return _value == ul;
            return false;
        }
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => _value.ToString();
        public bool Equals(UInt4 other) => _value == other._value;
        public bool Equals(Int4 other) => _value == other._value && !other._sign;
        public bool Equals(long other) => _value == other;
        public bool Equals(int other) => _value == other;
        public bool Equals(short other) => _value == other;
        public bool Equals(byte other) => _value == other;
    }
}
