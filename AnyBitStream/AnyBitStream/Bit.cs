using System;
using System.Runtime.InteropServices;

namespace AnyBitStream
{
    /// <summary>
    /// Represents a single bit
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 1)]
    public struct Bit : IEquatable<Bit>, IEquatable<long>, IEquatable<int>, IEquatable<byte>, IEquatable<short>, IEquatable<bool>
    {
        public const int BitSize = 1;

        private readonly bool _value;

        public Bit(bool value)
        {
            _value = value;
        }

        public Bit(byte value)
        {
            _value = value > 0x0;
        }

        public Bit(short value)
        {
            _value = value > 0x0;
        }

        public Bit(int value)
        {
            _value = value > 0x0;
        }

        public Bit(long value)
        {
            _value = value > 0x0;
        }

        public static implicit operator Bit(bool value) => new Bit(value);
        public static implicit operator Bit(byte value) => new Bit(value);
        public static implicit operator Bit(short value) => new Bit(value);
        public static implicit operator Bit(int value) => new Bit(value);
        public static implicit operator Bit(long value) => new Bit(value);
        public static implicit operator bool(Bit value) => value._value;
        public static implicit operator byte(Bit value) => (byte)(value._value ? 1 : 0);
        public static implicit operator short(Bit value) => (short)(value._value ? 1 : 0);
        public static implicit operator int(Bit value) => value._value ? 1 : 0;
        public static implicit operator long(Bit value) => value._value ? 1 : 0;

        public static bool operator ==(Bit bit1, Bit bit2) => bit1._value.Equals(bit2._value);
        public static bool operator !=(Bit bit1, Bit bit2) => !(bit1 == bit2);
        public static bool operator ==(Bit bit1, bool bit2) => bit1._value.Equals(bit2);
        public static bool operator !=(Bit bit1, bool bit2) => !(bit1._value == bit2);
        public static bool operator ==(bool bit1, Bit bit2) => bit1.Equals(bit2._value);
        public static bool operator !=(bool bit1, Bit bit2) => !(bit1 == bit2._value);
        public static bool operator ==(Bit bit1, int bit2) => (bit1._value ? 1 : 0).Equals(bit2);
        public static bool operator !=(Bit bit1, int bit2) => !(bit1 == bit2);
        public static bool operator ==(int bit1, Bit bit2) => bit1.Equals(bit2._value ? 1 : 0);
        public static bool operator !=(int bit1, Bit bit2) => !(bit1 == bit2);
        public static bool operator ==(Bit bit1, byte bit2) => ((byte)(bit1._value ? 1 : 0)).Equals(bit2);
        public static bool operator !=(Bit bit1, byte bit2) => !(bit1 == bit2);
        public static bool operator ==(byte bit1, Bit bit2) => bit1.Equals((byte)(bit2._value ? 1 : 0));
        public static bool operator !=(byte bit1, Bit bit2) => !(bit1 == bit2);
        public static bool operator >(Bit bit1, Bit bit2) => (bit1._value ? 1 : 0) > (bit2._value ? 1 : 0);
        public static bool operator <(Bit bit1, Bit bit2) => (bit1._value ? 1 : 0) < (bit2._value ? 1 : 0);
        public static bool operator >=(Bit bit1, Bit bit2) => (bit1._value ? 1 : 0) >= (bit2._value ? 1 : 0);
        public static bool operator <=(Bit bit1, Bit bit2) => (bit1._value ? 1 : 0) <= (bit2._value ? 1 : 0);

        public static Bit operator &(Bit x, Bit y) => x._value & y._value;

        public static Bit operator |(Bit x, Bit y) => x._value | y._value;

        public static Bit operator ^(Bit x, Bit y) => x._value ^ y._value;

        public static Bit operator ~(Bit bit) => ~((bit._value ? 1 : 0) & 1);


        public override bool Equals(object obj)
        {
            if(obj is Bit bit) return Equals(bit);
            if (obj is long l)
                return Equals(l);
            if (obj is int i)
                return Equals(i);
            if (obj is short s)
                return Equals(s);
            if (obj is byte b)
                return Equals(b);
            if (obj is bool bl)
                return Equals(bl);
            return false;
        }

        public override int GetHashCode() => _value ? 1 : 0;

        public override string ToString() => _value ? "1" : "0";

        public bool Equals(Bit other) => _value.Equals(other._value);

        public bool Equals(long other) => (_value ? 1 : 0).Equals(other);

        public bool Equals(int other) => (_value ? 1 : 0).Equals(other);

        public bool Equals(short other) => (_value ? 1 : 0).Equals(other);

        public bool Equals(byte other) => (_value ? 1 : 0).Equals(other);
        public bool Equals(bool other) => _value.Equals(other);
    }
}
