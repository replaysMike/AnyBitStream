using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AnyBitStream
{
    /// <summary>
    /// Read bits from a stream
    /// </summary>
    public class BitStreamReader : BinaryReader
    {
        private static readonly Encoding _defaultEncoding = Encoding.UTF8;
        private const bool DefaultLeaveOpen = false;

        /// <summary>
        /// Get the base stream exposed as a BitStream
        /// </summary>
        public new BitStream BaseStream => base.BaseStream as BitStream;

        /// <summary>
        /// Read bits from a stream
        /// </summary>
        public BitStreamReader() : this(new BitStream())
        {
        }

        /// <summary>
        /// Read bits from a stream
        /// </summary>
        /// <param name="allowUnalignedOperations">True to allow unaligned operations</param>
        public BitStreamReader(bool allowUnalignedOperations) : this(new BitStream(allowUnalignedOperations))
        {
        }

        /// <summary>
        /// Read bits from a stream
        /// </summary>
        /// <param name="capacity"></param>
        public BitStreamReader(int capacity) : this(new BitStream(capacity))
        {
        }

        /// <summary>
        /// Read bits from a stream
        /// </summary>
        /// <param name="buffer"></param>
        public BitStreamReader(byte[] buffer) : this(new BitStream(buffer))
        {
        }

        /// <summary>
        /// Read bits from a stream
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="leaveOpen">True to leave the stream open when disposed</param>
        public BitStreamReader(byte[] buffer, bool leaveOpen) : this(new BitStream(buffer), _defaultEncoding, leaveOpen)
        {
        }

        /// <summary>
        /// Read bits from a stream
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="leaveOpen">True to leave the stream open when disposed</param>
        /// <param name="allowUnalignedOperations">True to allow unaligned operations</param>
        public BitStreamReader(byte[] buffer, bool leaveOpen, bool allowUnalignedOperations) : this(new BitStream(buffer), leaveOpen, allowUnalignedOperations)
        {
        }

        /// <summary>
        /// Read bits from a stream
        /// </summary>
        /// <param name="buffer"></param>
        public BitStreamReader(byte[] buffer, Encoding encoding) : this(new BitStream(buffer), encoding, DefaultLeaveOpen)
        {
        }

        /// <summary>
        /// Read bits from a stream
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="encoding"></param>
        /// <param name="leaveOpen">True to leave the stream open when disposed</param>
        public BitStreamReader(byte[] buffer, Encoding encoding, bool leaveOpen) : this(new BitStream(buffer), encoding, leaveOpen)
        {
        }

        /// <summary>
        /// Read bits from a stream
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public BitStreamReader(byte[] buffer, int offset, int count) : this(new BitStream(buffer, offset, count))
        {
        }

        /// <summary>
        /// Read bits from a stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="leaveOpen">True to leave the stream open when disposed</param>
        public BitStreamReader(BitStream stream, bool leaveOpen) : base(stream, _defaultEncoding, leaveOpen)
        {
        }

        /// <summary>
        /// Read bits from a stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="leaveOpen">True to leave the stream open when disposed</param>
        /// <param name="allowUnalignedOperations">True to allow unaligned operations</param>
        public BitStreamReader(BitStream stream, bool leaveOpen, bool allowUnalignedOperations) : base(stream, _defaultEncoding, leaveOpen)
        {
            BaseStream.AllowUnalignedOperations = allowUnalignedOperations;
        }

        /// <summary>
        /// Read bits from a stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        /// <param name="leaveOpen">True to leave the stream openwhen disposed</param>
        public BitStreamReader(BitStream stream, Encoding encoding, bool leaveOpen) : base(stream, encoding, leaveOpen)
        {
        }

        /// <summary>
        /// Read bits from a stream
        /// </summary>
        /// <param name="stream"></param>
        public BitStreamReader(BitStream stream) : this(stream, _defaultEncoding, DefaultLeaveOpen)
        {
        }

        #region Overrides

        /// <inheritdoc />
        public override byte ReadByte() => (byte)BaseStream.ReadByte();

        /// <inheritdoc />
        public override int Read() => BaseStream.ReadByte();

        /// <inheritdoc />
        public override int Read(byte[] buffer, int index, int count) => BaseStream.Read(buffer, index, count);

        /// <inheritdoc />
        public override int Read(char[] buffer, int index, int count) => BaseStream.Read(buffer, index, count);


#if NET5_0
        /// <inheritdoc />
        public override int Read(Span<byte> buffer) => BaseStream.Read(buffer);

        /// <inheritdoc />
        public override int Read(Span<char> buffer) => BaseStream.Read(buffer);
#endif

        /// <inheritdoc />
        public override bool ReadBoolean() => BaseStream.ReadBit();

        /// <inheritdoc />
        public override byte[] ReadBytes(int count)
        {
            var buffer = new byte[count];
            BaseStream.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        /// <inheritdoc />
        public override char ReadChar() => (char)BaseStream.ReadByte();

        /// <inheritdoc />
        public override char[] ReadChars(int count)
        {
            var buffer = new char[count];
            BaseStream.Read(buffer, 0, buffer.Length);
            return buffer;
        }

        /// <inheritdoc />
        public override decimal ReadDecimal()
        {
            var bits = BaseStream.ReadBits(sizeof(decimal) * 8);
            return new decimal(bits);
        }

        /// <inheritdoc />
        public override double ReadDouble()
        {
            var buffer = new byte[sizeof(double)];
            BaseStream.Read(buffer, 0, buffer.Length);
            return BitConverter.ToDouble(buffer, 0);
        }

        /// <inheritdoc />
        public override short ReadInt16()
        {
            var buffer = new byte[sizeof(short)];
            BaseStream.Read(buffer, 0, buffer.Length);
            return BitConverter.ToInt16(buffer, 0);
        }

        /// <inheritdoc />
        public override int ReadInt32()
        {
            var buffer = new byte[sizeof(int)];
            BaseStream.Read(buffer, 0, buffer.Length);
            return BitConverter.ToInt32(buffer, 0);
        }

        /// <inheritdoc />
        public override long ReadInt64()
        {
            var buffer = new byte[sizeof(long)];
            BaseStream.Read(buffer, 0, buffer.Length);
            return BitConverter.ToInt64(buffer, 0);
        }

        /// <inheritdoc />
        public override ushort ReadUInt16()
        {
            var buffer = new byte[sizeof(ushort)];
            BaseStream.Read(buffer, 0, buffer.Length);
            return BitConverter.ToUInt16(buffer, 0);
        }

        /// <inheritdoc />
        public override uint ReadUInt32()
        {
            var buffer = new byte[sizeof(uint)];
            BaseStream.Read(buffer, 0, buffer.Length);
            return BitConverter.ToUInt32(buffer, 0);
        }

        /// <inheritdoc />
        public override ulong ReadUInt64()
        {
            var buffer = new byte[sizeof(ulong)];
            BaseStream.Read(buffer, 0, buffer.Length);
            return BitConverter.ToUInt64(buffer, 0);
        }

        /// <inheritdoc />
        public override sbyte ReadSByte()
        {
            return (sbyte)BaseStream.ReadByte();
        }

        /// <inheritdoc />
        public override float ReadSingle()
        {
            var buffer = new byte[sizeof(float)];
            BaseStream.Read(buffer, 0, buffer.Length);
            return BitConverter.ToSingle(buffer, 0);
        }

        /// <inheritdoc />
        public override string ReadString()
        {
            // read the length of the string
            var length = (int)BaseStream.ReadInt7();
            var buffer = new byte[length];
            BaseStream.Read(buffer, 0, length);
            return Encoding.Default.GetString(buffer);
        }

        #endregion

        #region Read types methods

        /// <summary>
        /// Read a single bit
        /// </summary>
        /// <returns></returns>
        public Bit ReadBit() => BaseStream.ReadBit();

        /// <summary>
        /// Read a single bit
        /// </summary>
        /// <param name="bits">The number of bits to read</param>
        /// <returns></returns>
        public IEnumerable<Bit> ReadBitArray(int bits) => BaseStream.ReadBitArrayInternal(bits);

        /// <summary>
        /// Read a specified number of bits
        /// </summary>
        /// <param name="bits">The number of bits to read</param>
        /// <returns></returns>
        public long ReadBits(int bits) => BaseStream.ReadBitsInternal(bits);

        /// <summary>
        /// Read a specified number of bits into a known numeric or ICustomType
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bits">The number of bits to read</param>
        /// <returns></returns>
        public T ReadBits<T>(int bits)
            where T : struct
            => BaseStream.ReadBitsInternal<T>(bits);

        /// <summary>
        /// Read a 2 bit integer
        /// </summary>
        /// <returns></returns>
        public Int2 ReadInt2() => BaseStream.ReadCustomInternal<Int2>();

        /// <summary>
        /// Read a 2 bit integer
        /// </summary>
        /// <returns></returns>
        public UInt2 ReadUInt2() => BaseStream.ReadCustomInternal<UInt2>();

        /// <summary>
        /// Read a 3 bit integer
        /// </summary>
        /// <returns></returns>
        public Int3 ReadInt3() => BaseStream.ReadCustomInternal<Int3>();

        /// <summary>
        /// Read a 3 bit integer
        /// </summary>
        /// <returns></returns>
        public UInt3 ReadUInt3() => BaseStream.ReadCustomInternal<UInt3>();

        /// <summary>
        /// Read a 4 bit integer
        /// </summary>
        /// <returns></returns>
        public Int4 ReadInt4() => BaseStream.ReadCustomInternal<Int4>();

        /// <summary>
        /// Read a 4 bit integer
        /// </summary>
        /// <returns></returns>
        public UInt4 ReadUInt4() => BaseStream.ReadCustomInternal<UInt4>();

        /// <summary>
        /// Read a 5 bit integer
        /// </summary>
        /// <returns></returns>
        public Int5 ReadInt5() => BaseStream.ReadCustomInternal<Int5>();

        /// <summary>
        /// Read a 5 bit integer
        /// </summary>
        /// <returns></returns>
        public UInt5 ReadUInt5() => BaseStream.ReadCustomInternal<UInt5>();

        /// <summary>
        /// Read a 6 bit integer
        /// </summary>
        /// <returns></returns>
        public Int6 ReadInt6() => BaseStream.ReadCustomInternal<Int6>();

        /// <summary>
        /// Read a 6 bit integer
        /// </summary>
        /// <returns></returns>
        public UInt6 ReadUInt6() => BaseStream.ReadCustomInternal<UInt6>();

        /// <summary>
        /// Read a 7 bit integer
        /// </summary>
        /// <returns></returns>
        public Int7 ReadInt7() => BaseStream.ReadCustomInternal<Int7>();

        /// <summary>
        /// Read a 7 bit integer
        /// </summary>
        /// <returns></returns>
        public UInt7 ReadUInt7() => BaseStream.ReadCustomInternal<UInt7>();

        /// <summary>
        /// Read a 10 bit integer
        /// </summary>
        /// <returns></returns>
        public Int10 ReadInt10() => BaseStream.ReadCustomInternal<Int10>();

        /// <summary>
        /// Read a 10 bit integer
        /// </summary>
        /// <returns></returns>
        public UInt10 ReadUInt10() => BaseStream.ReadCustomInternal<UInt10>();

        /// <summary>
        /// Read a 12 bit integer
        /// </summary>
        /// <returns></returns>
        public Int12 ReadInt12() => BaseStream.ReadCustomInternal<Int12>();

        /// <summary>
        /// Read a 12 bit integer
        /// </summary>
        /// <returns></returns>
        public UInt12 ReadUInt12() => BaseStream.ReadCustomInternal<UInt12>();

        /// <summary>
        /// Read a 24 bit integer
        /// </summary>
        /// <returns></returns>
        public Int24 ReadInt24() => BaseStream.ReadCustomInternal<Int24>();

        /// <summary>
        /// Read a 24 bit integer
        /// </summary>
        /// <returns></returns>
        public UInt24 ReadUInt24() => BaseStream.ReadCustomInternal<UInt24>();

        /// <summary>
        /// Read a 48 bit integer
        /// </summary>
        /// <returns></returns>
        public Int48 ReadInt48() => BaseStream.ReadCustomInternal<Int48>();

        /// <summary>
        /// Read a 48 bit integer
        /// </summary>
        /// <returns></returns>
        public UInt48 ReadUInt48() => BaseStream.ReadCustomInternal<UInt48>();

        /// <summary>
        /// Peek a single bit from the stream without moving the stream pointer
        /// </summary>
        /// <param name="bits">The number of bits to read</param>
        /// <returns></returns>
        public Bit PeekBit()
        {
            var startBitPosition = BaseStream._bitsPosition;
            try
            {
                return ReadBit();
            }
            finally
            {
                BaseStream._bitsPosition = startBitPosition;
            }
        }

        /// <summary>
        /// Peek a specified number of bits from the stream without moving the stream pointer
        /// </summary>
        /// <param name="bits">The number of bits to read</param>
        /// <returns></returns>
        public long PeekBits(int bits)
        {
            var startBitPosition = BaseStream._bitsPosition;
            try
            {
                return ReadBits(bits);
            }
            finally
            {
                BaseStream._bitsPosition = startBitPosition;
            }
        }

        /// <summary>
        /// Peek a specified number of bits from the stream without moving the stream pointer
        /// </summary>
        /// <param name="bits">The number of bits to read</param>
        /// <returns></returns>
        public T PeekBits<T>(int bits)
            where T : struct
        {
            var startBitPosition = BaseStream._bitsPosition;
            try
            {
                return ReadBits<T>(bits);
            }
            finally
            {
                BaseStream._bitsPosition = startBitPosition;
            }
        }

        /// <summary>
        /// Peek a number of bytes without moving the stream pointer
        /// </summary>
        /// <param name="count">The number of bytes to read</param>
        /// <returns></returns>
        public byte[] PeekBytes(int count)
        {
            var startBitPosition = BaseStream._bitsPosition;
            try
            {
                return ReadBytes(count);
            }
            finally
            {
                BaseStream._bitsPosition = startBitPosition;
            }
        }

        /// <summary>
        /// Skip a specified number of bytes
        /// </summary>
        public void SkipBytes(int count) => BaseStream.SkipBytes(count);

        /// <summary>
        /// Skip a single byte
        /// </summary>
        public void SkipByte() => SkipBytes(1);

        /// <summary>
        /// Skip a specified number of bits
        /// </summary>
        /// <param name="bitCount">The number of bits to skip</param>
        public void SkipBits(int bits) => BaseStream.SkipBits(bits);

        /// <summary>
        /// Skip a single bit
        /// </summary>
        public void SkipBit() => SkipBits(1);

        /// <summary>
        /// Set the bits position of the stream
        /// </summary>
        /// <param name="bitPosition"></param>
        public void SetBitsPosition(int bitPosition) => BaseStream.SetBitsPosition(bitPosition);

        /// <summary>
        /// Read unsigned integer (32 bits) Exponential Golomb coded syntax element with the left bit first.
        /// </summary>
        /// <param name="bitCount">The number of bits used to construct the value</param>
        /// <returns></returns>
        public uint ReadUe(out int bitCount)
        {
            var result = 0U;
            byte zeroCount = 0;
            var input = ReadUInt32();
            while((input & 0x1) == 0)
            {
                zeroCount++;
                input >>= 1;
            }

            var valueBitCount = (byte)(zeroCount + 1);
            for (byte i = 0; i <= valueBitCount; i++)
            {
                result <<= 1;
                result |= input & 0x1;
                input >>= 1;
            }
            result -= 1;
            bitCount = valueBitCount + zeroCount;

            return result;
        }

        /// <summary>
        /// Read signed integer (32 bits) Exponential Golomb coded syntax element with the left bit first
        /// </summary>
        /// <param name="bitCount">The number of bits used to construct the value</param>
        /// <returns></returns>
        public int ReadSe(out int bitCount)
        {
            var result = 0;
            byte zeroCount = 0;
            byte valueBitCount;
            var input = ReadInt32();
            while ((input & 0x1) == 0)
            {
                zeroCount++;
                input >>= 1;
            }

            valueBitCount = (byte)(zeroCount + 1);
            for (byte i = 0; i <= valueBitCount; i++)
            {
                result <<= 1;
                result |= input & 0x1;
                input >>= 1;
            }

            // remove the sign bit
            var sign = 1 - 2 * (result & 0x1);
            result = sign * ((result >> 1) & 0x7FFF);

            valueBitCount += zeroCount;
            if (valueBitCount > 0x20)
                result |= 0x8000;

            bitCount = valueBitCount + 1;

            return result;
        }

        /// <summary>
        /// Read truncated integer Exponential Golomb coded syntax element with the left bit first
        /// </summary>
        /// <param name="max"></param>
        /// <param name="bitCount">The number of bits used to construct the value</param>
        /// <returns></returns>
        public uint ReadTe(int max, out int bitCount)
        {
            if (max > 1)
                return ReadUe(out bitCount);
            bitCount = 1;
            return ~ReadBit() & 0x1;
        }

        #endregion
    }
}
