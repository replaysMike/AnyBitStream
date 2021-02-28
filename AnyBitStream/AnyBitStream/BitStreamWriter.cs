using System;
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

        #region Overrides

        public override void Write(bool value) => BaseStream.WriteBit(value);

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
        /// Write a single bit
        /// </summary>
        /// <param name="value"></param>
        public void WriteBit(Bit value) => BaseStream.WriteBit(value);

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
        public void WriteBits(byte value, int bits) => BaseStream.WriteBits(value, bits);

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

        #endregion

        /*public override void Flush()
        {
            BaseStream.FlushIfUnaligned();
            base.Flush();
        }*/

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
