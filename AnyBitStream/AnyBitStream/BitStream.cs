using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AnyBitStream
{
    /// <summary>
    /// Creates a stream whose backing stream is memory
    /// </summary>

    public class BitStream : MemoryStream
    {
        private const bool DefaultAllowUnalignedOperations = false;

        /// <summary>
        /// The current bit position
        /// </summary>
        internal int _bitsPosition;
        // the byte that contains partial bit values for unwritten bits, if _bitsPostition > 0
        private byte _pendingByteValue;
        private bool _hasPendingWrites;

        /// <summary>
        /// Get/set if unaligned reads/writes are allowed.
        /// When true, if bits have been written but there are empty bits remaining, the values will be padded to zero.
        /// When false, an exception will be raised.
        /// </summary>
        public bool AllowUnalignedOperations { get; set; } = DefaultAllowUnalignedOperations;

        /// <summary>
        /// Get if the stream is unaligned (has unwritten bits less than a byte)
        /// </summary>
        public bool IsUnaligned => _bitsPosition % 8 > 0;

        /// <inheritdoc />
        public BitStream() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitStream"/> class with an expandable capacity initialized to zero.
        /// </summary>
        /// <param name="allowUnalignedOperations">True to allow unaligned operations</param>
        public BitStream(bool allowUnalignedOperations) : base()
        {
            AllowUnalignedOperations = allowUnalignedOperations;
        }

        /// <inheritdoc />
        public BitStream(byte[] buffer) : base(buffer)
        {
        }

        /// <summary>
        /// Initializes a new non-resizable instance of the <see cref="BitStream"/> class based on the specified region of the ArraySegment
        /// </summary>
        /// <param name="buffer"></param>
        public BitStream(ArraySegment<byte> buffer) : base(buffer.Array, buffer.Offset, buffer.Count)
        {
        }

        /// <inheritdoc />
        public BitStream(int capacity) : base(capacity)
        {
        }

        /// <inheritdoc />
        public BitStream(byte[] buffer, bool writable) : base(buffer, writable)
        {
        }

        /// <summary>
        /// Initializes a new non-resizable instance of the <see cref="BitStream"/> class based on the specified byte array with the <see cref="MemoryStream.CanWrite"/> property set as specified
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="writable"></param>
        /// <param name="allowUnalignedOperations">True to allow unaligned operations</param>
        public BitStream(byte[] buffer, bool writable, bool allowUnalignedOperations) : base(buffer, writable)
        {
            AllowUnalignedOperations = allowUnalignedOperations;
        }

        /// <inheritdoc />
        public BitStream(byte[] buffer, int index, int count) : base(buffer, index, count)
        {
        }

        /// <inheritdoc />
        public BitStream(byte[] buffer, int index, int count, bool writable) : base(buffer, index, count, writable)
        {
        }

        /// <inheritdoc />
        public BitStream(byte[] buffer, int index, int count, bool writable, bool publicyVisible) : base(buffer, index, count, writable, publicyVisible)
        {
        }

        /// <summary>
        /// Initializes a new non-resizable instance of the <see cref="BitStream"/> class based on the specified byte array with the <see cref="MemoryStream.CanWrite"/> property set as specified, and the ability to call <see cref="MemoryStream.GetBuffer()"/> set as specified. 
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <param name="writable"></param>
        /// <param name="publicyVisible"></param>
        /// <param name="allowUnalignedOperations">True to allow unaligned operations</param>
        public BitStream(byte[] buffer, int index, int count, bool writable, bool publicyVisible, bool allowUnalignedOperations) : base(buffer, index, count, writable, publicyVisible)
        {
            AllowUnalignedOperations = allowUnalignedOperations;
        }

        #region Read types methods

        /// <summary>
        /// Read a specified number of bits
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public long ReadBits(int bits) => ReadBitsInternal(bits);

        /// <summary>
        /// Read a specified number of bits
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bits"></param>
        /// <returns></returns>
        public T ReadBits<T>(int bits)
            where T : struct
            => ReadBitsInternal<T>(bits);

        /// <summary>
        /// Read a single bit
        /// </summary>
        /// <returns></returns>
        public Bit ReadBit() => ReadBitsInternal(1) > 0;

        public Int2 ReadInt2() => ReadCustomInternal<Int2>();

        public UInt2 ReadUInt2() => ReadCustomInternal<UInt2>();

        public Int3 ReadInt3() => ReadCustomInternal<Int3>();

        public UInt3 ReadUInt3() => ReadCustomInternal<UInt3>();

        public Int4 ReadInt4() => ReadCustomInternal<Int4>();

        public UInt4 ReadUInt4() => ReadCustomInternal<UInt4>();

        public Int5 ReadInt5() => ReadCustomInternal<Int5>();

        public UInt5 ReadUInt5() => ReadCustomInternal<UInt5>();

        public Int6 ReadInt6() => ReadCustomInternal<Int6>();

        public UInt6 ReadUInt6() => ReadCustomInternal<UInt6>();

        public Int7 ReadInt7() => ReadCustomInternal<Int7>();

        public UInt7 ReadUInt7() => ReadCustomInternal<UInt7>();

        public Int10 ReadInt10() => ReadCustomInternal<Int10>();

        public UInt10 ReadUInt10() => ReadCustomInternal<UInt10>();

        public Int12 ReadInt12() => ReadCustomInternal<Int12>();

        public UInt12 ReadUInt12() => ReadCustomInternal<UInt12>();
        public Int24 ReadInt24() => ReadCustomInternal<Int24>();

        public UInt24 ReadUInt24() => ReadCustomInternal<UInt24>();
        public Int48 ReadInt48() => ReadCustomInternal<Int48>();

        public UInt48 ReadUInt48() => ReadCustomInternal<UInt48>();

        #endregion

        #region Write types methods

        /// <summary>
        /// Align the current position to the next byte boundary by writing bits of 0 for the remainder of the unwritten bits
        /// </summary>
        public void Align()
        {
            FlushIfUnaligned();
        }

        /// <summary>
        /// Write a single bit
        /// </summary>
        /// <param name="value"></param>
        public void WriteBit(Bit value) => WriteBitsInternal(value, 1);

        /// <summary>
        /// Write a single bit
        /// </summary>
        /// <param name="value"></param>
        public void WriteBit(bool value) => WriteBitsInternal(value ? 1 : 0, 1);

        /// <summary>
        /// Write a single bit
        /// </summary>
        /// <param name="value"></param>
        public void WriteBit(byte value) => WriteBitsInternal(value, 1);

        /// <summary>
        /// Write a single bit
        /// </summary>
        /// <param name="value"></param>
        public void WriteBit(int value) => WriteBitsInternal(value, 1);

        /// <summary>
        /// Write a value using a specified number of bits
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bits"></param>
        public void WriteBits(int value, int bits) => WriteBitsInternal(value, bits);

        /// <summary>
        /// Write a value using a specified number of bits
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bits"></param>
        public void WriteBits(byte value, int bits) => WriteBitsInternal(value, bits);

        /// <summary>
        /// Write a sequence of bits
        /// </summary>
        /// <param name="bits"></param>
        public void WriteBits(IEnumerable<Bit> bits) => WriteBitsInternal(bits);

        /// <summary>
        /// Write a sequence of bits
        /// </summary>
        /// <param name="bits"></param>
        public void WriteBits(IEnumerable<bool> bits) => WriteBitsInternal(bits.ToArray());

        /// <summary>
        /// Write a sequence of bits
        /// </summary>
        /// <param name="bits"></param>
        public void WriteBits(IEnumerable<int> bits) => WriteBitsInternal(bits.ToArray());

        public void Write(Int2 value) => WriteInternal(value);

        public void Write(UInt2 value) => WriteInternal(value);

        public void Write(Int3 value) => WriteInternal(value);

        public void Write(UInt3 value) => WriteInternal(value);

        public void Write(Int4 value) => WriteInternal(value);

        public void Write(UInt4 value) => WriteInternal(value);

        public void Write(Int5 value) => WriteInternal(value);

        public void Write(UInt5 value) => WriteInternal(value);

        public void Write(Int6 value) => WriteInternal(value);

        public void Write(UInt6 value) => WriteInternal(value);

        public void Write(Int7 value) => WriteInternal(value);

        public void Write(UInt7 value) => WriteInternal(value);

        public void Write(Int10 value) => WriteInternal(value);

        public void Write(UInt10 value) => WriteInternal(value);

        public void Write(Int12 value) => WriteInternal(value);

        public void Write(UInt12 value) => WriteInternal(value);
        public void Write(Int24 value) => WriteInternal(value);

        public void Write(UInt24 value) => WriteInternal(value);
        public void Write(Int48 value) => WriteInternal(value);

        public void Write(UInt48 value) => WriteInternal(value);

        #endregion

        #region Read overrides

        public int Read(char[] buffer, int offset, int count)
        {
            return ReadUnaligned(buffer, offset, count);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (IsUnaligned)
            {
                return ReadUnaligned(buffer, offset, count);
            }
            return base.Read(buffer, offset, count);
        }

#if NET5_0
        public override int Read(Span<byte> destination)
        {
            if (IsUnaligned)
            {
                return ReadUnaligned(destination);
            }
            return base.Read(destination);
        }

        public int Read(Span<char> destination)
        {
            return ReadUnaligned(destination);
        }

        public override ValueTask<int> ReadAsync(Memory<byte> destination, CancellationToken cancellationToken = default)
        {
            throw new NotSupportedException();
        }
        
        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            throw new NotSupportedException();
        }
#endif


        public override int ReadByte()
        {
            if (IsUnaligned)
            {
                return (int)ReadUnaligned(BitsSizeOf<byte>());
            }
            return base.ReadByte();
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region Write Overrides

        /// <inheritdoc />
        public override void WriteByte(byte value)
        {
            if (IsUnaligned)
            {
                WriteUnaligned(value, BitsSizeOf<byte>());
            }
            else
                base.WriteByte(value);
        }

        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (IsUnaligned)
            {
                WriteUnaligned(buffer, offset, count);
            }
            else
                base.Write(buffer, offset, count);
        }

        public void Write(char[] buffer, int offset, int count)
        {
            WriteUnaligned(buffer, offset, count);
        }

#if NET5_0
        /// <inheritdoc />
        public override void Write(ReadOnlySpan<byte> source)
        {
            if (IsUnaligned)
            {
                WriteUnaligned(source);
            }
            else
                base.Write(source);
        }

        public void Write(ReadOnlySpan<char> chars)
        {
            WriteUnaligned(chars);
        }
        

        /// <inheritdoc />
        public override ValueTask WriteAsync(ReadOnlyMemory<byte> source, CancellationToken cancellationToken = default)
        {
            if (IsUnaligned)
            {
                WriteUnalignedAsync(source, cancellationToken);
            }
            return base.WriteAsync(source, cancellationToken);
        }

        /// <inheritdoc />
        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (IsUnaligned)
            {
                WriteUnalignedAsync(buffer, 0, count, cancellationToken);
            }
            return base.WriteAsync(buffer, offset, count, cancellationToken);
        }

        /// <inheritdoc />
        public override bool TryGetBuffer(out ArraySegment<byte> buffer)
        {
            if (_bitsPosition > 0 && _hasPendingWrites)
                ForceFlush();
            return base.TryGetBuffer(out buffer);
        }

        /// <inheritdoc />
        public override Task FlushAsync(CancellationToken cancellationToken)
        {
            FlushIfUnaligned();
            return base.FlushAsync(cancellationToken);
        }
#endif

        /// <inheritdoc />
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            if (IsUnaligned)
            {
                WriteUnalignedAsync(buffer, 0, count, new CancellationTokenSource().Token);
            }
            return base.BeginWrite(buffer, offset, count, callback, state);
        }

        /// <inheritdoc />
        public override void WriteTo(Stream stream)
        {
            if (IsUnaligned)
            {
                FlushIfUnaligned();
            }
            base.WriteTo(stream);
        }

        #endregion

        #region Overrides


        

        /// <inheritdoc />
        public override byte[] ToArray()
        {
            try
            {
                if (_bitsPosition > 0 && _hasPendingWrites)
                    ForceFlush();
                return base.ToArray();
            }
            finally
            {
                _bitsPosition = 0;
            }
        }

        /// <inheritdoc />
        public override void Flush()
        {
            FlushIfUnaligned();
            base.Flush();
        }

        #endregion

        internal void WriteUnaligned(long value, int bits)
        {
            ValidateUnalignedBits();
            WriteBitsInternal(value, bits);
        }

        internal void WriteUnaligned(byte[] buffer, int offset, int count)
        {
            ValidateUnalignedBits();
            var bits = BitsSizeOf<byte>();
            for (var b = offset; b < count; b++)
            {
                for (byte i = 0; i < bits; i++)
                {
                    var bit = (buffer[b] >> i) & 0x1;
                    var bitValue = (byte)(bit << (_bitsPosition % 8));
                    _pendingByteValue += bitValue;
                    _bitsPosition++;
                    _hasPendingWrites = true;
                    FlushOnByteBoundary();
                }
            }
        }

        internal void WriteUnaligned(char[] buffer, int offset, int count)
        {
            ValidateUnalignedBits();
            var bits = BitsSizeOf<byte>();
            for (var b = offset; b < count; b++)
            {
                for (byte i = 0; i < bits; i++)
                {
                    var bit = (buffer[b] >> i) & 0x1;
                    var bitValue = (byte)(bit << (_bitsPosition % 8));
                    _pendingByteValue += bitValue;
                    _bitsPosition++;
                    _hasPendingWrites = true;
                    FlushOnByteBoundary();
                }
            }
        }

#if NET5_0
        internal void WriteUnaligned(ReadOnlySpan<byte> buffer)
        {
            ValidateUnalignedBits();
            var bits = BitsSizeOf<byte>();
            for (var b = 0; b < buffer.Length; b++)
            {
                for (byte i = 0; i < bits; i++)
                {
                    var bit = (buffer[b] >> i) & 0x1;
                    var bitValue = (byte)(bit << (_bitsPosition % 8));
                    _pendingByteValue += bitValue;
                    _bitsPosition++;
                    _hasPendingWrites = true;
                    FlushOnByteBoundary();
                }
            }
        }

        internal void WriteUnaligned(ReadOnlySpan<char> buffer)
        {
            ValidateUnalignedBits();
            var bits = BitsSizeOf<char>();
            for (var b = 0; b < buffer.Length; b++)
            {
                for (byte i = 0; i < bits; i++)
                {
                    var bit = (buffer[b] >> i) & 0x1;
                    var bitValue = (byte)(bit << (_bitsPosition % 8));
                    _pendingByteValue += bitValue;
                    _bitsPosition++;
                    _hasPendingWrites = true;
                    FlushOnByteBoundary();
                }
            }
        }

        private void WriteUnaligned(ReadOnlyMemory<byte> buffer)
        {
            WriteUnaligned(buffer.Span);
        }
#endif

        internal void WriteBitsInternal(long value, int bits)
        {
            for (byte i = 0; i < bits; i++)
            {
                var bit = (value >> i) & 0x1;
                var bitValue = (byte)(bit << (_bitsPosition % 8));
                _pendingByteValue += bitValue;
                _bitsPosition++;
                _hasPendingWrites = true;
                FlushOnByteBoundary();
            }
        }

        internal void WriteBitsInternal(IEnumerable<Bit> bits)
        {
            var i = 0;
            foreach (var b in bits)
            {
                var bit = (b >> i) & 0x1;
                var bitValue = (byte)(bit << (_bitsPosition % 8));
                _pendingByteValue += bitValue;
                _bitsPosition++;
                _hasPendingWrites = true;
                FlushOnByteBoundary();
                i++;
            }
        }

        internal void WriteInternal(ICustomType value)
        {
            for (byte i = 0; i < value.TotalBits; i++)
            {
                var bit = value.GetBit(i);
                var bitValue = (byte)(bit << (_bitsPosition % 8));
                _pendingByteValue += bitValue;
                _bitsPosition++;
                _hasPendingWrites = true;
                FlushOnByteBoundary();
            }
        }

        private Task WriteUnalignedAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            ValidateUnalignedBits();
            return cancellationToken.IsCancellationRequested
                ? Task.FromCanceled(cancellationToken)
                : BeginEndWriteUnalignedAsync(buffer, offset, count, cancellationToken);
        }

        private Task BeginEndWriteUnalignedAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            var factory = new TaskFactory(cancellationToken);
            return factory.FromAsync((callback, state) => {
                WriteUnaligned(buffer, offset, count);
                return base.BeginWrite(buffer, offset, count, callback, state);
            }, EndWrite, cancellationToken);
        }

#if NET5_0
        private Task WriteUnalignedAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken)
        {
            ValidateUnalignedBits();
            return cancellationToken.IsCancellationRequested
                ? Task.FromCanceled(cancellationToken)
                : BeginEndWriteUnalignedAsync(buffer, cancellationToken);
        }

        private Task BeginEndWriteUnalignedAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken)
        {
            var factory = new TaskFactory(cancellationToken);
            return factory.FromAsync((callback, state) => {
                WriteUnaligned(buffer);
                return base.BeginWrite(buffer.Span.ToArray(), 0, buffer.Span.Length, callback, state);
            }, EndWrite, cancellationToken);
        }
#endif

        private void ValidateUnalignedBits()
        {
            if (!AllowUnalignedOperations && _bitsPosition % 8 != 0)
                throw new StreamUnalignedException($"There are {8 - _bitsPosition} bits that have not been written to the stream. Either write more bits, flush the pending bits to the stream, or enable {nameof(AllowUnalignedOperations)}!");
        }

        internal T ReadBitsInternal<T>(int bits)
            where T : struct
        {
            if (bits > BitsSizeOf<T>())
                throw new ArgumentOutOfRangeException($"Cannot read {bits} bits into type {typeof(T).Name} as it can only hold a maximum of {BitsSizeOf<T>()} bits.");
            return (T)(object)ReadBitsInternal(bits);
        }

        internal IEnumerable<Bit> ReadBitArrayInternal(int bits)
        {
            if (bits > BitsSizeOf<long>())
                throw new ArgumentOutOfRangeException($"Cannot read {bits} bits into type {typeof(long).Name} as it can only hold a maximum of {BitsSizeOf<long>()} bits.");
            var returnValue = new Bit[bits];
            for (byte i = 0; i < bits; i++)
            {
                if (_bitsPosition % 8 == 0)
                {
                    Debug.WriteLine($"");
                    _pendingByteValue = (byte)ReadByte();
                }

                var bit = (_pendingByteValue >> (_bitsPosition % 8)) & 0x1L;
                returnValue[i] = bit;
                _bitsPosition++;
            }
            return returnValue;
        }

        internal long ReadBitsInternal(int bits)
        {
            if (bits > BitsSizeOf<long>())
                throw new ArgumentOutOfRangeException($"Cannot read {bits} bits into type {typeof(long).Name} as it can only hold a maximum of {BitsSizeOf<long>()} bits.");
            var returnValue = 0L;
            for (byte i = 0; i < bits; i++)
            {
                if (_bitsPosition % 8 == 0)
                {
                    _pendingByteValue = (byte)ReadByte();
                }

                var bit = (_pendingByteValue >> (_bitsPosition % 8)) & 0x1L;
                var bitValue = bit << i;
                returnValue += bitValue;
                _bitsPosition++;
            }
            return returnValue;
        }

        internal ulong ReadUBitsInternal(int bits)
        {
            if (bits > BitsSizeOf<ulong>())
                throw new ArgumentOutOfRangeException($"Cannot read {bits} bits into type {typeof(ulong).Name} as it can only hold a maximum of {BitsSizeOf<long>()} bits.");
            var returnValue = 0UL;
            for (byte i = 0; i < bits; i++)
            {
                if (_bitsPosition % 8 == 0)
                {
                    _pendingByteValue = (byte)ReadByte();
                }

                var bit = (ulong)((_pendingByteValue >> (_bitsPosition % 8))) & 0x1;
                var bitValue = bit << i;
                returnValue += bitValue;
                _bitsPosition++;
            }
            return returnValue;
        }

        private long ReadUnaligned(int bits)
        {
            ValidateUnalignedBits();
            return ReadBitsInternal(bits);
        }

        private int ReadUnaligned(byte[] buffer, int offset, int count)
        {
            ValidateUnalignedBits();
            var bytesRead = 0;
            for (var b = offset; b < count; b++)
            {
                if (b > buffer.Length - 1)
                    break;
                bytesRead++;
                for (byte i = 0; i < BitsSizeOf<byte>(); i++)
                {
                    if (_bitsPosition % 8 == 0)
                    {
                        _pendingByteValue = (byte)ReadByte();
                    }

                    var bit = (_pendingByteValue >> (_bitsPosition % 8)) & 0x1;
                    var bitValue = bit << i;
                    buffer[b] += (byte)bitValue;
                    _bitsPosition++;
                }
            }
            return bytesRead;
        }

        private int ReadUnaligned(char[] buffer, int offset, int count)
        {
            ValidateUnalignedBits();
            var bytesRead = 0;
            for (var b = offset; b < count; b++)
            {
                if (b > buffer.Length - 1)
                    break;
                bytesRead++;
                for (byte i = 0; i < BitsSizeOf<byte>(); i++)
                {
                    if (_bitsPosition % 8 == 0)
                    {
                        _pendingByteValue = (byte)ReadByte();
                    }

                    var bit = (_pendingByteValue >> (_bitsPosition % 8)) & 0x1;
                    var bitValue = bit << i;
                    buffer[b] += (char)bitValue;
                    _bitsPosition++;
                }
            }
            return bytesRead;
        }

#if NET5_0
        private int ReadUnaligned(Span<byte> destination)
        {
            ValidateUnalignedBits();
            var bytesRead = 0;
            for (var b = 0; b < destination.Length; b++)
            {
                if (b > destination.Length - 1)
                    break;
                bytesRead++;
                for (byte i = 0; i < BitsSizeOf<byte>(); i++)
                {
                    if (_bitsPosition % 8 == 0)
                    {
                        _pendingByteValue = (byte)ReadByte();
                    }

                    var bit = (_pendingByteValue >> (_bitsPosition % 8)) & 0x1;
                    var bitValue = bit << i;
                    destination[b] += (byte)bitValue;
                    _bitsPosition++;
                }
            }
            return bytesRead;
        }

        private int ReadUnaligned(Span<char> destination)
        {
            ValidateUnalignedBits();
            var bytesRead = 0;
            for (var b = 0; b < destination.Length; b++)
            {
                if (b > destination.Length - 1)
                    break;
                bytesRead++;
                for (byte i = 0; i < BitsSizeOf<byte>(); i++)
                {
                    if (_bitsPosition % 8 == 0)
                    {
                        _pendingByteValue = (byte)ReadByte();
                    }

                    var bit = (_pendingByteValue >> (_bitsPosition % 8)) & 0x1;
                    var bitValue = bit << i;
                    destination[b] += (char)bitValue;
                    _bitsPosition++;
                }
            }
            return bytesRead;
        }
#endif

        internal T ReadCustomInternal<T>()
            where T : ICustomType
        {
            var v = (Int2)1L;
            if (typeof(T) == typeof(Int2))
                return (T)(object)(Int2)ReadBitsInternal(Int2.BitSize);
            else if (typeof(T) == typeof(UInt2))
                return (T)(object)(UInt2)ReadUBitsInternal(UInt2.BitSize);
            else if (typeof(T) == typeof(Int3))
                return (T)(object)(Int3)ReadBitsInternal(Int3.BitSize);
            else if (typeof(T) == typeof(UInt3))
                return (T)(object)(UInt3)ReadUBitsInternal(UInt3.BitSize);
            else if (typeof(T) == typeof(Int4))
                return (T)(object)(Int4)ReadBitsInternal(Int4.BitSize);
            else if (typeof(T) == typeof(UInt4))
                return (T)(object)(UInt4)ReadUBitsInternal(UInt4.BitSize);
            else if (typeof(T) == typeof(Int5))
                return (T)(object)(Int5)ReadBitsInternal(Int5.BitSize);
            else if (typeof(T) == typeof(UInt5))
                return (T)(object)(UInt5)ReadUBitsInternal(UInt5.BitSize);
            else if (typeof(T) == typeof(Int6))
                return (T)(object)(Int6)ReadBitsInternal(Int6.BitSize);
            else if (typeof(T) == typeof(UInt6))
                return (T)(object)(UInt6)ReadUBitsInternal(UInt6.BitSize);
            else if (typeof(T) == typeof(Int7))
                return (T)(object)(Int7)ReadBitsInternal(Int7.BitSize);
            else if (typeof(T) == typeof(UInt7))
                return (T)(object)(UInt7)ReadUBitsInternal(UInt7.BitSize);
            else if (typeof(T) == typeof(Int10))
                return (T)(object)(Int10)ReadBitsInternal(Int10.BitSize);
            else if (typeof(T) == typeof(UInt10))
                return (T)(object)(UInt10)ReadUBitsInternal(UInt10.BitSize);
            else if (typeof(T) == typeof(Int12))
                return (T)(object)(Int12)ReadBitsInternal(Int12.BitSize);
            else if (typeof(T) == typeof(UInt12))
                return (T)(object)(UInt12)ReadUBitsInternal(UInt12.BitSize);
            else if (typeof(T) == typeof(Int24))
                return (T)(object)(Int24)ReadBitsInternal(Int24.BitSize);
            else if (typeof(T) == typeof(UInt24))
                return (T)(object)(UInt24)ReadUBitsInternal(UInt24.BitSize);
            else if (typeof(T) == typeof(Int48))
                return (T)(object)(Int48)ReadBitsInternal(Int48.BitSize);
            else if (typeof(T) == typeof(UInt48))
                return (T)(object)(UInt48)ReadUBitsInternal(UInt48.BitSize);
            throw new NotSupportedException($"Type '{typeof(T).Name}' is not supported!");
        }

        /// <summary>
        /// If any bits are pending written, write them to the stream.
        /// </summary>
        internal virtual void FlushIfUnaligned()
        {
            if (_bitsPosition > 0 && _bitsPosition % 8 != 0 && _hasPendingWrites)
            {
                ForceFlush();
                _bitsPosition = 0;
            }
        }

        /// <summary>
        /// If there is a pending byte with partial bits written,
        /// flush the value out as a byte with the rest of the bits being padded to zero
        /// </summary>
        internal virtual void FlushOnByteBoundary()
        {
            if (_bitsPosition > 0 && _bitsPosition % 8 == 0 && _hasPendingWrites)
            {
                ForceFlush();
            }
        }

        /// <summary>
        /// Write the pending unwritten byte to the stream
        /// </summary>
        internal void ForceFlush()
        {
            base.WriteByte(_pendingByteValue);
            _pendingByteValue = 0;
            _hasPendingWrites = false;
        }

        /// <summary>
        /// Get the number of bits for a given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private int BitsSizeOf<T>()
        {
            var type = typeof(T);
            if (type == typeof(bool) || type == typeof(bool))
                return sizeof(bool);
            if (type == typeof(byte) || type == typeof(sbyte))
                return sizeof(byte) * 8;
            else if (type == typeof(short) || type == typeof(ushort))
                return sizeof(short) * 8;
            else if (type == typeof(int) || type == typeof(uint))
                return sizeof(int) * 8;
            else if (type == typeof(long) || type == typeof(ulong))
                return sizeof(long) * 8;
            // this isn't ideal but we can't utilize the interface without an instance
            else if (type == typeof(Bit) || type == typeof(Bit))
                return Bit.BitSize;
            else if (type == typeof(Int2) || type == typeof(UInt2))
                return Int2.BitSize;
            else if (type == typeof(Int3) || type == typeof(UInt3))
                return Int3.BitSize;
            else if (type == typeof(Int4) || type == typeof(UInt4))
                return Int4.BitSize;
            else if (type == typeof(Int5) || type == typeof(UInt5))
                return Int5.BitSize;
            else if (type == typeof(Int6) || type == typeof(UInt6))
                return Int6.BitSize;
            else if (type == typeof(Int7) || type == typeof(UInt7))
                return Int7.BitSize;
            else if (type == typeof(Int10) || type == typeof(UInt10))
                return Int10.BitSize;
            else if (type == typeof(Int12) || type == typeof(UInt12))
                return Int12.BitSize;
            else if (type == typeof(Int24) || type == typeof(UInt24))
                return Int24.BitSize;
            else if (type == typeof(Int48) || type == typeof(UInt48))
                return Int48.BitSize;
            throw new NotSupportedException($"Type {(typeof(T))} is not supported.");
        }

        protected override void Dispose(bool disposing)
        {
            if (_bitsPosition > 0 && _hasPendingWrites)
                ForceFlush();
            base.Dispose(disposing);
        }
    }
}
