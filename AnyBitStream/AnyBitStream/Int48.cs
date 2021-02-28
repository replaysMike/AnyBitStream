using System;
using System.Runtime.InteropServices;

namespace AnyBitStream
{
    /// <summary>
    /// Represents a 48-bit (6 byte) signed integer
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = BitSize / 8)]
    public struct Int48 : ICustomType
    {
        /// <summary>
        /// The number of bits occupied by the type
        /// </summary>
        public const int BitSize = 48;
        /// <summary>
        /// The number of bytes required to store the type
        /// </summary>
        public const int ByteSize = 6;
        /// <summary>
        /// The minimum value the type can store
        /// </summary>
        public static readonly Int48 MinValue = (Int48)(-((1L << (BitSize - 1)) - 1)); // -140737488355328
        /// <summary>
        /// The maximum value the type can store
        /// </summary>
        public static readonly Int48 MaxValue = (Int48)((1L << (BitSize - 1)) - 1); // 140737488355328
        public int TotalBits => BitSize;
        public int TotalBytes => ByteSize;
        internal readonly byte[] _value;
        internal bool _sign;

        public Int48(long value)
        {
            var absValue = value < 0 ? -value : value;
            _value = new byte[BitSize / 8];
            _value[0] = (byte)(absValue & 0xFF);
            _value[1] = (byte)((absValue >> 8) & 0xFF);
            _value[2] = (byte)((absValue >> 16) & 0xFF);
            _value[3] = (byte)((absValue >> 24) & 0xFF);
            _value[4] = (byte)((absValue >> 32) & 0xFF);
            _value[5] = (byte)((absValue >> 40) & 0x7F);
            _sign = value < 0;
        }

        public Int48(byte[] bytes)
        {
            _value = new byte[BitSize / 8] { bytes[0], bytes[1], bytes[2], bytes[3], bytes[4], (byte)(bytes[5] & 0x7F) };
            _sign = (bytes[5] >> 7 & 0x1) == 0x1;
        }

        public Bit GetBit(int index) => index < BitSize - 1 ? (byte)((long)this >> index & 0x1) : (_sign ? 1 : 0);

        public Bit[] GetBits()
        {
            var bits = new Bit[BitSize];
            for (byte i = 0; i < bits.Length - 1; i++)
            {
                bits[i] = GetBit(i);
            }
            bits[BitSize - 1] = _sign;
            return bits;
        }

        public byte[] GetBytes()
        {
            return new byte[BitSize / 8] {
                _value[0],
                _value[1],
                _value[2],
                _value[3],
                _value[4],
                (byte)(_value[5] + ((_sign ? 1L : 0L) << 7))
            };
        }

        public static explicit operator Int48(long value) => new Int48(value);
        public static explicit operator long(Int48 i)
        {
            var value = i._value[0] + (i._value[1] << 8) + (i._value[2] << 16) + ((long)i._value[3] << 24) + ((long)i._value[4] << 32) + ((long)i._value[5] << 40);
            return -((i._sign ? 1L : 0L) << 47) + value;
        }
        public static bool operator ==(Int48 val1, Int48 val2) => val1.Equals(val2);
        public static bool operator !=(Int48 val1, Int48 val2) => !(val1.Equals(val2));
        public override bool Equals(object obj) => _value.Equals(obj);
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => ((long)this).ToString();
        public bool Equals(long other) => (long)this == other;
        public bool Equals(int other) => (int)this == other;
        public bool Equals(short other) => (short)this == other;
        public bool Equals(byte other) => (byte)this == other;
    }

    /// <summary>
    /// Represents a 48-bit (6 byte) unsigned integer
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
    public struct UInt48 : ICustomType
    {
        /// <summary>
        /// The number of bits occupied by the type
        /// </summary>
        public const int BitSize = 48;
        /// <summary>
        /// The number of bytes required to store the type
        /// </summary>
        public const int ByteSize = 6;
        /// <summary>
        /// The minimum value the type can store
        /// </summary>
        public static readonly UInt48 MinValue = (UInt48)0;
        /// <summary>
        /// The maximum value the type can store
        /// </summary>
        public static readonly UInt48 MaxValue = (UInt48)((1L << BitSize) - 1);
        public int TotalBits => BitSize;
        public int TotalBytes => ByteSize;
        internal readonly byte[] _value;

        public UInt48(ulong value)
        {
            _value = new byte[BitSize / 8];
            _value[0] = (byte)(value & 0xFF);
            _value[1] = (byte)((value >> 8) & 0xFF);
            _value[2] = (byte)((value >> 16) & 0xFF);
            _value[3] = (byte)((value >> 24) & 0xFF);
            _value[4] = (byte)((value >> 32) & 0xFF);
            _value[5] = (byte)((value >> 40) & 0xFF);
        }

        public UInt48(byte[] bytes)
        {
            _value = new byte[BitSize / 8] { bytes[0], bytes[1], bytes[2], bytes[3], bytes[4], bytes[5] };
        }

        public Bit GetBit(int index) => (byte)((ulong)this >> index) & 0x1;

        public Bit[] GetBits()
        {
            var bits = new Bit[BitSize];
            for (byte i = 0; i < bits.Length; i++)
                bits[i] = GetBit(i);
            return bits;
        }

        public byte[] GetBytes() => _value;

        public static explicit operator UInt48(ulong value) => new UInt48(value);
        public static explicit operator ulong(UInt48 i)
            => i._value[0] + ((ulong)i._value[1] << 8) + ((ulong)i._value[2] << 16) + ((ulong)i._value[3] << 24) + ((ulong)i._value[4] << 32) + ((ulong)i._value[5] << 40);
        public static bool operator ==(UInt48 val1, UInt48 val2) => val1.Equals(val2);
        public static bool operator !=(UInt48 val1, UInt48 val2) => !(val1.Equals(val2));
        public override bool Equals(object obj) => _value.Equals(obj);
        public override int GetHashCode() => _value.GetHashCode();
        public override string ToString() => ((ulong)this).ToString();
        public bool Equals(long other) => (ulong)this == (ulong)other;
        public bool Equals(int other) => (uint)this == (uint)other;
        public bool Equals(short other) => (ushort)this == (ushort)other;
        public bool Equals(byte other) => (byte)this == other;
    }
}
