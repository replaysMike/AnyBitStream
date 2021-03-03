using System;
using System.Runtime.InteropServices;

namespace AnyBitStream
{
    /// <summary>
    /// Represents a 10-bit signed integer
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 2)]
    public struct Int10 : ICustomType, IEquatable<Int10>, IEquatable<UInt10>
    {
        /// <summary>
        /// The number of bits occupied by the type
        /// </summary>
        public const int BitSize = 10;
        /// <summary>
        /// The number of bytes required to store the type
        /// </summary>
        public const int ByteSize = 2;
        /// <summary>
        /// The minimum value the type can store
        /// </summary>
        public static readonly Int10 MinValue = (Int10)(-((1 << (BitSize - 1)) - 1));
        /// <summary>
        /// The maximum value the type can store
        /// </summary>
        public static readonly Int10 MaxValue = (Int10)((1 << (BitSize - 1)) - 1);
        public int TotalBits => BitSize;
        public int TotalBytes => ByteSize;
        internal readonly short _value;
        internal bool _sign;

        public Int10(long value)
        {
            _value = (short)(value < 0 ? -value : value & 0x1FF);
            _sign = value < 0;
        }

        public Bit GetBit(int index) => (Bit)(index < BitSize - 1 ? (byte)(_value >> index & 0x1) : (_sign ? 1 : 0));
        public Bit[] GetBits() => new Bit[BitSize] { GetBit(0), GetBit(1), GetBit(2), GetBit(3), GetBit(4), GetBit(5), GetBit(6), GetBit(7), GetBit(8), _sign };

        public static explicit operator Int10(int value) => new Int10(value);
        public static explicit operator int(Int10 i)
            => -((i._sign ? 1 : 0) << (BitSize - 1)) + i._value;
        public static bool operator ==(Int10 val1, Int10 val2) => val1.Equals(val2);
        public static bool operator !=(Int10 val1, Int10 val2) => !(val1.Equals(val2));
        public static bool operator ==(Int10 val1, object val2) => val1.Equals(val2);
        public static bool operator !=(Int10 val1, object val2) => !(val1.Equals(val2));
        public static bool operator ==(Int10 val1, long val2) => val1.Equals(val2);
        public static bool operator !=(Int10 val1, long val2) => !(val1.Equals(val2));
        public static bool operator ==(long val2, Int10 val1) => val1.Equals(val2);
        public static bool operator !=(long val2, Int10 val1) => !(val1.Equals(val2));
        public static Int10 operator -(Int10 a, long b) => new Int10(a._value - b);
        public static long operator -(long a, Int10 b) => a - b._value;
        public static Int10 operator +(Int10 a, long b) => new Int10(a._value + b);
        public static long operator +(long a, Int10 b) => a + b._value;
        public static Int10 operator *(Int10 a, long b) => new Int10(a._value * b);
        public static long operator *(long a, Int10 b) => a * b._value;
        public static Int10 operator /(Int10 a, long b) => new Int10(a._value / b);
        public static long operator /(long a, Int10 b) => a / b._value;
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (obj is Int10 other)
                return Equals(other);
            if (obj is UInt10 other2)
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
                return _value == (long)ul;
            return false;
        }
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => _value.ToString();
        public bool Equals(Int10 other) => _value == other._value && _sign == other._sign;
        public bool Equals(UInt10 other) => _value == other._value && !_sign;
        public bool Equals(long other) => _value == other;
        public bool Equals(int other) => _value == other;
        public bool Equals(short other) => _value == other;
        public bool Equals(byte other) => _value == other;
    }

    /// <summary>
    /// Represents a 10-bit unsigned integer
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 2)]
    public struct UInt10 : ICustomType, IEquatable<UInt10>, IEquatable<Int10>
    {
        /// <summary>
        /// The number of bits occupied by the type
        /// </summary>
        public const int BitSize = 10;
        /// <summary>
        /// The number of bytes required to store the type
        /// </summary>
        public const int ByteSize = 2;
        /// <summary>
        /// The minimum value the type can store
        /// </summary>
        public static readonly UInt10 MinValue = (UInt10)0;
        /// <summary>
        /// The maximum value the type can store
        /// </summary>
        public static readonly UInt10 MaxValue = (UInt10)((1 << BitSize) - 1);
        public int TotalBits => BitSize;
        public int TotalBytes => ByteSize;
        internal readonly short _value;

        public UInt10(ulong value)
        {
            _value = (short)(value & 0x3FF);
        }

        public Bit GetBit(int index) => (Bit)((byte)(_value >> index & 0x1));
        public Bit[] GetBits() => new Bit[BitSize] { GetBit(0), GetBit(1), GetBit(2), GetBit(3), GetBit(4), GetBit(5), GetBit(6), GetBit(7), GetBit(8), GetBit(9) };

        public static explicit operator UInt10(ulong value) => new UInt10(value);
        public static explicit operator ulong(UInt10 i)
            => (ulong)(i._value & 0x3FF);
        public static bool operator ==(UInt10 val1, UInt10 val2) => val1.Equals(val2);
        public static bool operator !=(UInt10 val1, UInt10 val2) => !(val1.Equals(val2));
        public static bool operator ==(UInt10 val1, object val2) => val1.Equals(val2);
        public static bool operator !=(UInt10 val1, object val2) => !(val1.Equals(val2));
        public static bool operator ==(UInt10 val1, long val2) => val1.Equals(val2);
        public static bool operator !=(UInt10 val1, long val2) => !(val1.Equals(val2));
        public static bool operator ==(long val2, UInt10 val1) => val1.Equals(val2);
        public static bool operator !=(long val2, UInt10 val1) => !(val1.Equals(val2));
        public static UInt10 operator -(UInt10 a, ulong b) => new UInt10((ulong)a._value - b);
        public static ulong operator -(ulong a, UInt10 b) => a - (ulong)b._value;
        public static UInt10 operator +(UInt10 a, ulong b) => new UInt10((ulong)a._value + b);
        public static ulong operator +(ulong a, UInt10 b) => a + (ulong)b._value;
        public static UInt10 operator *(UInt10 a, ulong b) => new UInt10((ulong)a._value * b);
        public static ulong operator *(ulong a, UInt10 b) => a * (ulong)b._value;
        public static UInt10 operator /(UInt10 a, ulong b) => new UInt10((ulong)a._value / b);
        public static ulong operator /(ulong a, UInt10 b) => a / (ulong)b._value;
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (obj is UInt10 other)
                return Equals(other);
            if (obj is Int10 other2)
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
                return (ulong)_value == ul;
            return false;
        }
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => _value.ToString();
        public bool Equals(UInt10 other) => _value == other._value;
        public bool Equals(Int10 other) => _value == other._value && !other._sign;
        public bool Equals(long other) => _value == other;
        public bool Equals(int other) => _value == other;
        public bool Equals(short other) => _value == other;
        public bool Equals(byte other) => _value == other;
    }
}
