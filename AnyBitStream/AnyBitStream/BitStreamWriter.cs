﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AnyBitStream
{
    /// <summary>
    /// Write bits to a stream
    /// </summary>
    public class BitStreamWriter : BinaryWriter
    {
        private static readonly Encoding _defaultEncoding = Encoding.UTF8;
        private const bool DefaultLeaveOpen = false;

        /// <summary>
        /// Get the base stream exposed as a BitStream
        /// </summary>
        public new BitStream BaseStream => base.OutStream as BitStream;

        /// <summary>
        /// Write bits to a stream
        /// </summary>
        public BitStreamWriter() : this(new BitStream())
        {
        }

        /// <summary>
        /// Write bits to a stream
        /// </summary>
        /// <param name="allowUnalignedOperations">True to allow unaligned operations</param>
        public BitStreamWriter(bool allowUnalignedOperations) : this(new BitStream(allowUnalignedOperations))
        {
        }

        /// <summary>
        /// Write bits to a stream
        /// </summary>
        /// <param name="capacity"></param>
        public BitStreamWriter(int capacity) : this(new BitStream(capacity))
        {
        }

        /// <summary>
        /// Write bits to a stream
        /// </summary>
        /// <param name="buffer"></param>
        public BitStreamWriter(byte[] buffer) : this(new BitStream(buffer))
        {
        }

        /// <summary>
        /// Write bits to a stream
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="leaveOpen">True to leave the stream open when dispoed</param>
        /// <param name="allowUnalignedOperations">True to allow unaligned operations</param>
        public BitStreamWriter(byte[] buffer, bool leaveOpen, bool allowUnalignedOperations) : this(new BitStream(buffer), leaveOpen, allowUnalignedOperations)
        {
            BaseStream.AllowUnalignedOperations = allowUnalignedOperations;
        }

        /// <summary>
        /// Write bits to a stream
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public BitStreamWriter(byte[] buffer, int offset, int count) : this(new BitStream(buffer, offset, count))
        {
        }

        /// <summary>
        /// Write bits to a buffer
        /// </summary>
        /// <param name="buffer"></param>
        public BitStreamWriter(ArraySegment<byte> buffer) : this(buffer.Array, buffer.Offset, buffer.Count)
        {
        }

        /// <summary>
        /// Write bits to a stream
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="encoding"></param>
        public BitStreamWriter(byte[] buffer, Encoding encoding) : this(new BitStream(buffer), encoding, DefaultLeaveOpen)
        {
        }

        /// <summary>
        /// Write bits to a stream
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="encoding"></param>
        /// <param name="leaveOpen">True to leave the stream open when disposed</param>
        public BitStreamWriter(byte[] buffer, Encoding encoding, bool leaveOpen) : this(new BitStream(buffer), encoding, leaveOpen)
        {
        }

        /// <summary>
        /// Write bits to a stream
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="leaveOpen">True to leave the stream open when disposed</param>
        public BitStreamWriter(byte[] buffer, bool leaveOpen) : this(new BitStream(buffer), _defaultEncoding, leaveOpen)
        {
        }

        /// <summary>
        /// Write bits to a stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="leaveOpen">True to leave the stream open when disposed</param>
        public BitStreamWriter(BitStream stream, bool leaveOpen) : base(stream, _defaultEncoding, leaveOpen)
        {
        }

        /// <summary>
        /// Write bits to a stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="leaveOpen">True to leave the stream open when disposed</param>
        /// <param name="allowUnalignedOperations">True to allow unaligned operations</param>
        public BitStreamWriter(BitStream stream, bool leaveOpen, bool allowUnalignedOperations) : base(stream, _defaultEncoding, leaveOpen)
        {
            BaseStream.AllowUnalignedOperations = allowUnalignedOperations;
        }

        /// <summary>
        /// Write bits to a stream
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="encoding"></param>
        /// <param name="leaveOpen">True to leave the stream open when disposed</param>
        public BitStreamWriter(BitStream stream, Encoding encoding, bool leaveOpen) : base(stream, encoding, leaveOpen)
        {
        }

        /// <summary>
        /// Write bits to a stream
        /// </summary>
        /// <param name="stream"></param>
        public BitStreamWriter(BitStream stream) : this(stream, _defaultEncoding, DefaultLeaveOpen)
        {
        }

        /// <summary>
        /// Replace the underlying buffer without allocating new resources.
        /// The position will be reset to 0, and the length and capacity will adopt the new buffer's length.
        /// </summary>
        /// <param name="buffer">The new buffer to use</param>
        public void ReplaceBuffer(byte[] buffer)
            => BaseStream.ReplaceBuffer(buffer);

        #region Overrides

        public override void Write(bool value) => BaseStream.WriteByte((byte)(value ? 1 : 0));

        public override void Write(byte value) => BaseStream.WriteByte(value);

        public override void Write(char ch) => BaseStream.WriteByte((byte)ch);

        public override void Write(byte[] buffer) => BaseStream.Write(buffer, 0, buffer.Length);

        public override void Write(byte[] buffer, int index, int count) => BaseStream.Write(buffer, index, count);

        public override void Write(char[] chars) => BaseStream.Write(chars, 0, chars.Length);

        public override void Write(char[] chars, int index, int count) => BaseStream.Write(chars, index, count);

        public override void Write(decimal value)
        {
            var bits = decimal.GetBits(value);
            BaseStream.WriteBits(bits);
        }

        public override void Write(double value) => BaseStream.Write(BitConverter.GetBytes(value), 0, sizeof(double));

        public override void Write(float value) => BaseStream.Write(BitConverter.GetBytes(value), 0, sizeof(float));

        public override void Write(int value) => BaseStream.Write(BitConverter.GetBytes(value), 0, sizeof(int));

        public override void Write(long value) => BaseStream.Write(BitConverter.GetBytes(value), 0, sizeof(long));

        public override void Write(sbyte value) => BaseStream.Write(BitConverter.GetBytes(value), 0, sizeof(sbyte));

        public override void Write(short value) => BaseStream.Write(BitConverter.GetBytes(value), 0, sizeof(short));

        public override void Write(uint value) => BaseStream.Write(BitConverter.GetBytes(value), 0, sizeof(uint));

        public override void Write(ulong value) => BaseStream.Write(BitConverter.GetBytes(value), 0, sizeof(ulong));

        public override void Write(ushort value) => BaseStream.Write(BitConverter.GetBytes(value), 0, sizeof(ushort));

        public override void Write(string value)
        {
            BaseStream.Write((Int7)(value?.Length ?? 0));
            var bytes = Encoding.Default.GetBytes(value);
            BaseStream.Write(bytes, 0, bytes.Length);
        }

#if NET5_0
        public override void Write(ReadOnlySpan<byte> buffer) => BaseStream.Write(buffer);

        public override void Write(ReadOnlySpan<char> chars) => BaseStream.Write(chars);
#endif

        #endregion

        #region Write types methods

        /// <summary>
        /// Align the current position to the next byte boundary by writing bits of 0 for the remainder of the unwritten bits
        /// </summary>
        public void Align() => BaseStream.Align();

        /// <summary>
        /// Write a byte array
        /// </summary>
        /// <param name="buffer">A byte array containing the data to write.</param>
        public void Write(ArraySegment<byte> buffer) => Write(buffer.Array, buffer.Offset, buffer.Count);

        /// <summary>
        /// Write a byte array
        /// </summary>
        /// <param name="buffer">A byte array containing the data to write.</param>
        public void WriteBytes(ArraySegment<byte> buffer) => Write(buffer);

        /// <summary>
        /// Writes a region of a byte array to the current stream.
        /// </summary>
        /// <param name="buffer">A byte array containing the data to write.</param>
        public void WriteBytes(byte[] buffer) => Write(buffer, 0, buffer.Length);

        /// <summary>
        /// Write a byte array
        /// </summary>
        /// <param name="buffer">A byte array containing the data to write.</param>
        /// <param name="offset">The starting point in buffer at which to begin writing.</param>
        /// <param name="length">The number of bytes to write.</param>
        public void WriteBytes(byte[] buffer, int offset, int length) => Write(buffer, offset, length);

        /// <summary>
        /// Write a single bit
        /// </summary>
        /// <param name="value"></param>
        public void WriteBit(Bit value) => BaseStream.WriteBit(value);

        /// <summary>
        /// Write a single bit
        /// </summary>
        /// <param name="value"></param>
        public void WriteBit(bool value) => BaseStream.WriteBit(value);

        /// <summary>
        /// Write a single bit
        /// </summary>
        /// <param name="value"></param>
        public void WriteBit(byte value) => BaseStream.WriteBit(value);

        /// <summary>
        /// Write a single bit
        /// </summary>
        /// <param name="value"></param>
        public void WriteBit(int value) => BaseStream.WriteBit(value);

        /// <summary>
        /// Write a sequence of bits
        /// </summary>
        /// <param name="value"></param>
        public void WriteBits(IEnumerable<Bit> value) => BaseStream.WriteBits(value);

        /// <summary>
        ///Write a sequence of bits
        /// </summary>
        /// <param name="value"></param>
        public void WriteBits(IEnumerable<bool> value) => BaseStream.WriteBits(value);

        /// <summary>
        /// Write a sequence of bits
        /// </summary>
        /// <param name="value"></param>
        public void WriteBits(IEnumerable<int> value) => BaseStream.WriteBits(value);

        /// <summary>
        /// Write a value using a specified number of bits
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bits"></param>
        public void WriteBits(long value, int bits) => BaseStream.WriteBits(value, bits);

        /// <summary>
        /// Write a value using a specified number of bits
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bits"></param>
        public void WriteBits(int value, int bits) => BaseStream.WriteBits(value, bits);

        /// <summary>
        /// Write a value using a specified number of bits
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bits"></param>
        public void WriteBits(short value, int bits) => BaseStream.WriteBits(value, bits);

        /// <summary>
        /// Write a value using a specified number of bits
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bits"></param>
        public void WriteBits(byte value, int bits) => BaseStream.WriteBits(value, bits);

        /// <summary>
        /// Write a value using a specified number of bits
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bits"></param>
        public void WriteBits(ulong value, int bits) => BaseStream.WriteBits(value, bits);

        /// <summary>
        /// Write a value using a specified number of bits
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bits"></param>
        public void WriteBits(uint value, int bits) => BaseStream.WriteBits(value, bits);

        /// <summary>
        /// Write a value using a specified number of bits
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bits"></param>
        public void WriteBits(ushort value, int bits) => BaseStream.WriteBits(value, bits);

        /// <summary>
        /// Write a value using a specified number of bits
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bits"></param>
        public void WriteBits(sbyte value, int bits) => BaseStream.WriteBits(value, bits);

        /// <summary>
        /// Write a 2 bit integer
        /// </summary>
        /// <param name="value"></param>
        public void Write(Int2 value) => BaseStream.Write(value);

        /// <summary>
        /// Write a 2 bit integer
        /// </summary>
        /// <param name="value"></param>
        public void Write(UInt2 value) => BaseStream.Write(value);

        /// <summary>
        /// Write a 3 bit integer
        /// </summary>
        /// <param name="value"></param>
        public void Write(Int3 value) => BaseStream.Write(value);

        /// <summary>
        /// Write a 3 bit integer
        /// </summary>
        /// <param name="value"></param>
        public void Write(UInt3 value) => BaseStream.Write(value);

        /// <summary>
        /// Write a 4 bit integer
        /// </summary>
        /// <param name="value"></param>
        public void Write(Int4 value) => BaseStream.Write(value);

        /// <summary>
        /// Write a 4 bit integer
        /// </summary>
        /// <param name="value"></param>
        public void Write(UInt4 value) => BaseStream.Write(value);

        /// <summary>
        /// Write a 5 bit integer
        /// </summary>
        /// <param name="value"></param>
        public void Write(Int5 value) => BaseStream.Write(value);

        /// <summary>
        /// Write a 5 bit integer
        /// </summary>
        /// <param name="value"></param>
        public void Write(UInt5 value) => BaseStream.Write(value);

        /// <summary>
        /// Write a 6 bit integer
        /// </summary>
        /// <param name="value"></param>
        public void Write(Int6 value) => BaseStream.Write(value);

        /// <summary>
        /// Write a 6 bit integer
        /// </summary>
        /// <param name="value"></param>
        public void Write(UInt6 value) => BaseStream.Write(value);

        /// <summary>
        /// Write a 7 bit integer
        /// </summary>
        /// <param name="value"></param>
        public void Write(Int7 value) => BaseStream.Write(value);

        /// <summary>
        /// Write a 7 bit integer
        /// </summary>
        /// <param name="value"></param>
        public void Write(UInt7 value) => BaseStream.Write(value);

        /// <summary>
        /// Write a 10 bit integer
        /// </summary>
        /// <param name="value"></param>
        public void Write(Int10 value) => BaseStream.Write(value);

        /// <summary>
        /// Write a 10 bit integer
        /// </summary>
        /// <param name="value"></param>
        public void Write(UInt10 value) => BaseStream.Write(value);

        /// <summary>
        /// Write a 12 bit integer
        /// </summary>
        /// <param name="value"></param>
        public void Write(Int12 value) => BaseStream.Write(value);

        /// <summary>
        /// Write a 12 bit integer
        /// </summary>
        /// <param name="value"></param>
        public void Write(UInt12 value) => BaseStream.Write(value);

        /// <summary>
        /// Write a 24 bit integer
        /// </summary>
        /// <param name="value"></param>
        public void Write(Int24 value) => BaseStream.Write(value);

        /// <summary>
        /// Write a 24 bit integer
        /// </summary>
        /// <param name="value"></param>
        public void Write(UInt24 value) => BaseStream.Write(value);

        /// <summary>
        /// Write a 48 bit integer
        /// </summary>
        /// <param name="value"></param>
        public void Write(Int48 value) => BaseStream.Write(value);

        /// <summary>
        /// Write a 48 bit integer
        /// </summary>
        /// <param name="value"></param>
        public void Write(UInt48 value) => BaseStream.Write(value);

        /// <summary>
        /// Write unsigned integer as Exponential Golomb coded syntax element with the left bit first
        /// </summary>
        /// <returns>The number of bits written</returns>
        public int WriteUe(uint value)
        {
            var result = 0U;
            value = value + 1;
            var bitCount = MathUtilities.GetRequiredBits(value);
            while (value > 0)
            {
                result <<= 1;
                result |= value & 0x1;
                value >>= 1;
            }

            result <<= bitCount - 1;
            bitCount <<= 1;
            WriteBits(result, bitCount);
            return bitCount;
        }

        /// <summary>
        /// Write signed integer (32 bits) Exp-Golomb-coded syntax element with the left bit first
        /// </summary>
        /// <returns>The number of bits written</returns>
        public int WriteSe(int value)
        {
            var result = 0;
            value = (value == 0 ? 1 : (MathUtilities.Abs(value) << 1) | ((value >> 15) & 0x1));
            var bitCount = MathUtilities.GetRequiredBits((uint)value);
            while (value > 0)
            {
                result <<= 1;
                result |=value & 0x1;
                value >>= 1;
            }

            result <<= bitCount - 1;
            bitCount <<= 1;
            WriteBits(result, bitCount);
            return bitCount;
        }

        /// <summary>
        /// Write unsigned integer Exp-Golomb-coded syntax element with the left bit first
        /// </summary>
        /// <param name="value"></param>
        /// <param name="max"></param>
        /// <returns>The number of bits written</returns>
        public int WriteTe(uint value, int max)
        {
            if (max > 1)
                return WriteUe(value);
            WriteBit((Bit)(~value & 0x1));
            return 1;
        }

        #endregion

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
    }
}
