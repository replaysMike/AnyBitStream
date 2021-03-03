using System;
using System.Runtime.InteropServices;

namespace AnyBitStream
{
    /// <summary>
    /// Represents a 7-bit signed integer
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
    public struct Int7 : ICustomType, IEquatable<Int7>, IEquatable<UInt7>
    {
        /// <summary>
        /// The number of bits occupied by the type
        /// </summary>
        public const int BitSize = 7;
        /// <summary>
        /// The number of bytes required to store the type
        /// </summary>
        public const int ByteSize = 1;
        /// <summary>
        /// The minimum value the type can store
        /// </summary>
        public static readonly Int7 MinValue = (Int7)(-((1 << (BitSize - 1)) - 1));
        /// <summary>
        /// The maximum value the type can store
        /// </summary>
        public static readonly Int7 MaxValue = (Int7)((1 << (BitSize - 1)) - 1);
        public int TotalBits => BitSize;
        public int TotalBytes => ByteSize;
        internal readonly byte _value;
        internal bool _sign;

        public Int7(long value)
        {
            _value = (byte)(value < 0 ? -value : value & 0x3F);
            _sign = value < 0;
        }

        public Bit GetBit(int index) => (Bit)(index < BitSize - 1 ? (byte)(_value >> index & 0x1) : (_sign ? 1 : 0));
        public Bit[] GetBits() => new Bit[BitSize] { GetBit(0), GetBit(1), GetBit(2), GetBit(3), GetBit(4), GetBit(5), _sign };

        public static explicit operator Int7(int value) => new Int7(value);
        public static explicit operator int(Int7 i)
            => -((i._sign ? 1 : 0) << (BitSize - 1)) + i._value;
        public static bool operator ==(Int7 val1, Int7 val2) => val1.Equals(val2);
        public static bool operator !=(Int7 val1, Int7 val2) => !(val1.Equals(val2));
        public static bool operator ==(Int7 val1, object val2) => val1.Equals(val2);
        public static bool operator !=(Int7 val1, object val2) => !(val1.Equals(val2));
        public static bool operator ==(Int7 val1, long val2) => val1.Equals(val2);
        public static bool operator !=(Int7 val1, long val2) => !(val1.Equals(val2));
        public static bool operator ==(long val2, Int7 val1) => val1.Equals(val2);
        public static bool operator !=(long val2, Int7 val1) => !(val1.Equals(val2));
        public static Int7 operator -(Int7 a, long b) => new Int7(a._value - b);
        public static long operator -(long a, Int7 b) => a - b._value;
        public static Int7 operator +(Int7 a, long b) => new Int7(a._value + b);
        public static long operator +(long a, Int7 b) => a + b._value;
        public static Int7 operator *(Int7 a, long b) => new Int7(a._value * b);
        public static long operator *(long a, Int7 b) => a * b._value;
        public static Int7 operator /(Int7 a, long b) => new Int7(a._value / b);
        public static long operator /(long a, Int7 b) => a / b._value;
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (obj is Int7 other)
                return Equals(other);
            if (obj is UInt7 other2)
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
        public bool Equals(Int7 other) => _value == other._value && _sign == other._sign;
        public bool Equals(UInt7 other) => _value == other._value && !_sign;
        public bool Equals(long other) => _value == other;
        public bool Equals(int other) => _value == other;
        public bool Equals(short other) => _value == other;
        public bool Equals(byte other) => _value == other;
    }

    /// <summary>
    /// Represents a 7-bit unsigned integer
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
    public struct UInt7 : ICustomType, IEquatable<UInt7>, IEquatable<Int7>
    {
        /// <summary>
        /// The number of bits occupied by the type
        /// </summary>
        public const int BitSize = 7;
        /// <summary>
        /// The number of bytes required to store the type
        /// </summary>
        public const int ByteSize = 1;
        /// <summary>
        /// The minimum value the type can store
        /// </summary>
        public static readonly UInt7 MinValue = (UInt7)0;
        /// <summary>
        /// The maximum value the type can store
        /// </summary>
        public static readonly UInt7 MaxValue = (UInt7)((1 << BitSize) - 1);
        public int TotalBits => BitSize;
        public int TotalBytes => ByteSize;
        internal readonly byte _value;

        public UInt7(ulong value)
        {
            _value = (byte)(value & 0x7F);
        }

        public Bit GetBit(int index) => (Bit)(_value >> index & 0x1);
        public Bit[] GetBits() => new Bit[BitSize] { GetBit(0), GetBit(1), GetBit(2), GetBit(3), GetBit(4), GetBit(5), GetBit(6) };

        public static explicit operator UInt7(ulong value) => new UInt7(value);
        public static explicit operator ulong(UInt7 i)
            => (ulong)(i._value & 0x7F);
        public static bool operator ==(UInt7 val1, UInt7 val2) => val1.Equals(val2);
        public static bool operator !=(UInt7 val1, UInt7 val2) => !(val1.Equals(val2));
        public static bool operator ==(UInt7 val1, object val2) => val1.Equals(val2);
        public static bool operator !=(UInt7 val1, object val2) => !(val1.Equals(val2));
        public static bool operator ==(UInt7 val1, long val2) => val1.Equals(val2);
        public static bool operator !=(UInt7 val1, long val2) => !(val1.Equals(val2));
        public static bool operator ==(long val2, UInt7 val1) => val1.Equals(val2);
        public static bool operator !=(long val2, UInt7 val1) => !(val1.Equals(val2));
        public static UInt7 operator -(UInt7 a, ulong b) => new UInt7(a._value - b);
        public static ulong operator -(ulong a, UInt7 b) => a - b._value;
        public static UInt7 operator +(UInt7 a, ulong b) => new UInt7(a._value + b);
        public static ulong operator +(ulong a, UInt7 b) => a + b._value;
        public static UInt7 operator *(UInt7 a, ulong b) => new UInt7(a._value * b);
        public static ulong operator *(ulong a, UInt7 b) => a * b._value;
        public static UInt7 operator /(UInt7 a, ulong b) => new UInt7(a._value / b);
        public static ulong operator /(ulong a, UInt7 b) => a / b._value;
        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;
            if (obj is UInt7 other)
                return Equals(other);
            if (obj is Int7 other2)
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
        public bool Equals(UInt7 other) => _value == other._value;
        public bool Equals(Int7 other) => _value == other._value && !other._sign;
        public bool Equals(long other) => _value == other;
        public bool Equals(int other) => _value == other;
        public bool Equals(short other) => _value == other;
        public bool Equals(byte other) => _value == other;
    }
}
