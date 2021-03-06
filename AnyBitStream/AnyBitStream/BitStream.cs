﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace AnyBitStream
{
    /// <summary>
    /// Creates a stream whose backing stream is memory
    /// </summary>

    public class BitStream : MemoryStream
    {
        private const bool DefaultPubliclyVisible = true;
        private const bool DefaultWritable = true;
        private const bool DefaultAllowUnalignedOperations = false;
        /// <summary>
        /// The current bit position
        /// </summary>
        internal int _bitsPosition;
        // the byte that contains partial bit values for unwritten bits, if _bitsPostition > 0
        private byte _pendingByteValue;
        private bool _hasPendingWrites;


        #region StreamPointer support

        // below is the implementation for referencing an underlying stream.
        // we utilize reflection to update the stream pointer when data is modified, so
        // no copying of the underlying stream is required.
        private readonly Stream _streamPointer;
        private static readonly Type _type = typeof(MemoryStream);
        private static readonly FieldInfo _bufferField;
        private static readonly FieldInfo _originField;
        private static readonly FieldInfo _positionField;
        private static readonly FieldInfo _lengthField;
        private static readonly FieldInfo _capacityField;
        private static readonly FieldInfo _expandableField;
        private static readonly FieldInfo _writableField;
        private static readonly FieldInfo _exposableField;
        private static readonly FieldInfo _isOpenField;

        static BitStream()
        {
            _type = typeof(MemoryStream);
            _bufferField = _type.GetField("_buffer", BindingFlags.NonPublic | BindingFlags.Instance);
            _originField = _type.GetField("_origin", BindingFlags.NonPublic | BindingFlags.Instance);
            _positionField = _type.GetField("_position", BindingFlags.NonPublic | BindingFlags.Instance);
            _lengthField = _type.GetField("_length", BindingFlags.NonPublic | BindingFlags.Instance);
            _capacityField = _type.GetField("_capacity", BindingFlags.NonPublic | BindingFlags.Instance);
            _expandableField = _type.GetField("_expandable", BindingFlags.NonPublic | BindingFlags.Instance);
            _writableField = _type.GetField("_writable", BindingFlags.NonPublic | BindingFlags.Instance);
            _exposableField = _type.GetField("_exposable", BindingFlags.NonPublic | BindingFlags.Instance);
            _isOpenField = _type.GetField("_isOpen", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        #endregion

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

        /// <summary>
        /// Get/sets the bits position of the stream
        /// </summary>
        public int BitsPosition
        {
            get {
                return _bitsPosition;
            }
            set {
                _bitsPosition = value % 8;
                Position = value / 8;
            }
        }

        /// <inheritdoc />
        public override long Position
        {
            get {
                if (_streamPointer != null)
                    return _streamPointer.Position;
                return base.Position;
            }
            set {
                if (_streamPointer != null)
                    _streamPointer.Position = value;
                base.Position = value;
            }
        }

        /// <inheritdoc />
        public override long Length
        {
            get {
                if (_streamPointer != null)
                    return _streamPointer.Length;
                return base.Length;
            }
        }

        /// <inheritdoc />
        public override void SetLength(long value)
        {
            if (_streamPointer != null)
            {
                _streamPointer.SetLength(value);
            }
            base.SetLength(value);
        }

        /// <inheritdoc />
        public BitStream()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitStream"/> from an existing <see cref="Stream"/>.
        /// No copying of the stream is performed, the underlying stream will be referenced.
        /// </summary>
        /// <param name="stream"></param>
        public BitStream(Stream stream) : this(stream as MemoryStream)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitStream"/> from an existing <see cref="MemoryStream"/>.
        /// No copying of the stream is performed, the underlying stream will be referenced.
        /// </summary>
        /// <param name="memoryStream"></param>
        /// <exception cref="NotSupportedException"></exception>
        public BitStream(MemoryStream memoryStream)
        {
            if (memoryStream != null)
            {
                // re-initialize the current stream from an existing stream
                _streamPointer = memoryStream;
                InitializeStreamPointer(memoryStream);
            }
            else
            {
                throw new NotSupportedException($"Only MemoryStream streams are supported!");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitStream"/> class with an expandable capacity initialized to zero.
        /// </summary>
        /// <param name="allowUnalignedOperations">True to allow unaligned operations</param>
        public BitStream(bool allowUnalignedOperations)
        {
            AllowUnalignedOperations = allowUnalignedOperations;
        }

        /// <inheritdoc />
        public BitStream(byte[] buffer) : base(buffer, 0, buffer.Length, DefaultWritable, DefaultPubliclyVisible)
        {
        }

        /// <summary>
        /// Initializes a new non-resizable instance of the <see cref="BitStream"/> class based on the specified region of the ArraySegment
        /// </summary>
        /// <param name="buffer"></param>
        public BitStream(ArraySegment<byte> buffer) : base(buffer.Array, buffer.Offset, buffer.Count, DefaultWritable, DefaultPubliclyVisible)
        {
        }

        /// <inheritdoc />
        public BitStream(int capacity) : base(capacity)
        {
        }

        /// <inheritdoc />
        public BitStream(byte[] buffer, bool writable) : base(buffer, 0, buffer.Length, writable, DefaultPubliclyVisible)
        {
        }

        /// <summary>
        /// Initializes a new non-resizable instance of the <see cref="BitStream"/> class based on the specified byte array with the <see cref="MemoryStream.CanWrite"/> property set as specified
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="writable"></param>
        /// <param name="allowUnalignedOperations">True to allow unaligned operations</param>
        public BitStream(byte[] buffer, bool writable, bool allowUnalignedOperations) : base(buffer, 0, buffer.Length, writable, DefaultPubliclyVisible)
        {
            AllowUnalignedOperations = allowUnalignedOperations;
        }

        /// <inheritdoc />
        public BitStream(byte[] buffer, int index, int count) : base(buffer, index, count, DefaultWritable, DefaultPubliclyVisible)
        {
        }

        /// <inheritdoc />
        public BitStream(byte[] buffer, int index, int count, bool writable) : base(buffer, index, count, writable, DefaultPubliclyVisible)
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
        /// Write the contents of one stream to another from it's current position
        /// </summary>
        /// <param name="stream">The stream to copy</param>
        public void Write(Stream stream)
        {
            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Write the contents of one stream to another from a specified position and length
        /// </summary>
        /// <param name="stream">The stream to copy</param>
        /// <param name="offset">The offset from which to start copying from</param>
        /// <param name="length">The length to copy</param>
        public void Write(Stream stream, int offset, int length)
        {
            if (offset > stream.Length)
                throw new ArgumentOutOfRangeException(nameof(offset), $"Value must be within the range of the stream.");
            if (length > stream.Length - offset)
                throw new ArgumentOutOfRangeException(nameof(offset), $"Value must be within the range of the stream.");
            var buffer = new byte[length];
            stream.Position = offset;
            stream.Read(buffer, 0, buffer.Length);
            Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Write a single bit
        /// </summary>
        /// <param name="value"></param>
        public void WriteBit(Bit value) => WriteBitsInternal((long)value, 1);

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
        public void WriteBits(long value, int bits) => WriteBitsInternal(value, bits);

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
        public void WriteBits(short value, int bits) => WriteBitsInternal(value, bits);

        /// <summary>
        /// Write a value using a specified number of bits
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bits"></param>
        public void WriteBits(byte value, int bits) => WriteBitsInternal(value, bits);

        /// <summary>
        /// Write a value using a specified number of bits
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bits"></param>
        public void WriteBits(ulong value, int bits) => WriteBitsInternal(value, bits);

        /// <summary>
        /// Write a value using a specified number of bits
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bits"></param>
        public void WriteBits(uint value, int bits) => WriteBitsInternal(value, bits);

        /// <summary>
        /// Write a value using a specified number of bits
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bits"></param>
        public void WriteBits(ushort value, int bits) => WriteBitsInternal(value, bits);

        /// <summary>
        /// Write a value using a specified number of bits
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bits"></param>
        public void WriteBits(sbyte value, int bits) => WriteBitsInternal(value, bits);

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
            var val = base.Read(buffer, offset, count);
            UpdateReadStreamPointer();
            return val;
        }

#if NET5_0
        public override int Read(Span<byte> destination)
        {
            if (IsUnaligned)
            {
                return ReadUnaligned(destination);
            }
            var val = base.Read(destination);
            UpdateReadStreamPointer();
            return val;
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
            var val = base.ReadByte();
            UpdateReadStreamPointer();
            return val;
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
            {
                base.WriteByte(value);
                UpdateWriteStreamPointer();
            }
        }

        /// <inheritdoc />
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (IsUnaligned)
            {
                WriteUnaligned(buffer, offset, count);
            }
            else
            {
                base.Write(buffer, offset, count);
                UpdateWriteStreamPointer();
            }
        }

        public void Write(char[] buffer, int offset, int count) => WriteUnaligned(buffer, offset, count);

#if NET5_0
        /// <inheritdoc />
        public override void Write(ReadOnlySpan<byte> source)
        {
            if (IsUnaligned)
            {
                WriteUnaligned(source);
            }
            else
            {
                base.Write(source);
                UpdateWriteStreamPointer();
            }
        }

        public void Write(ReadOnlySpan<char> chars)
        {
            WriteUnaligned(chars);
        }


        /// <inheritdoc />
        public override async ValueTask WriteAsync(ReadOnlyMemory<byte> source, CancellationToken cancellationToken = default)
        {
            if (IsUnaligned)
            {
                await WriteUnalignedAsync(source, cancellationToken);
            }
            await base.WriteAsync(source, cancellationToken);
            UpdateWriteStreamPointer();
        }

        /// <inheritdoc />
        public override async Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            if (IsUnaligned)
            {
                await WriteUnalignedAsync(buffer, 0, count, cancellationToken);
            }
            await base.WriteAsync(buffer, offset, count, cancellationToken);
            UpdateWriteStreamPointer();
        }

        /// <inheritdoc />
        public override bool TryGetBuffer(out ArraySegment<byte> buffer)
        {
            if (_bitsPosition > 0 && _hasPendingWrites)
                ForceFlush();
            var result = base.TryGetBuffer(out buffer);
            UpdateWriteStreamPointer();
            return result;
        }

        /// <inheritdoc />
        public override async Task FlushAsync(CancellationToken cancellationToken)
        {
            FlushIfUnaligned();
            await base.FlushAsync(cancellationToken);
            UpdateWriteStreamPointer();
        }
#endif

        /// <inheritdoc />
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            if (IsUnaligned)
            {
                WriteUnalignedAsync(buffer, 0, count, new CancellationTokenSource().Token);
            }
            var result = base.BeginWrite(buffer, offset, count, (ar) => {
                callback.Invoke(ar);
                UpdateWriteStreamPointer();
            }, state);
            return result;
        }

        /// <inheritdoc />
        public override void WriteTo(Stream stream)
        {
            if (IsUnaligned)
            {
                FlushIfUnaligned();
            }
            base.WriteTo(stream);
            UpdateWriteStreamPointer();
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

        internal void WriteBitsInternal(ulong value, int bits)
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

        /// <summary>
        /// Peek a number of bytes without moving the stream pointer
        /// </summary>
        /// <param name="count">The number of bytes to read</param>
        /// <returns></returns>
        internal int PeekByte()
        {
            var startBitPosition = _bitsPosition;
            var startPosition = Position;
            try
            {
                return ReadByte();
            }
            finally
            {
                _bitsPosition = startBitPosition;
                Position = startPosition;
            }
        }

        /// <summary>
        /// Peek a number of bytes without moving the stream pointer
        /// </summary>
        /// <param name="count">The number of bytes to read</param>
        /// <returns></returns>
        internal byte[] PeekBytes(int count)
        {
            var startBitPosition = _bitsPosition;
            var startPosition = Position;
            try
            {
                var buffer = new byte[count];
                Read(buffer, 0, count);
                return buffer;
            }
            finally
            {
                _bitsPosition = startBitPosition;
                Position = startPosition;
            }
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
                    _pendingByteValue = (byte)PeekByte();
                }

                var bit = (_pendingByteValue >> (_bitsPosition % 8)) & 0x1L;
                returnValue[i] = (Bit)bit;
                _bitsPosition++;
                MovePointerOnByteBoundary();
            }
            UpdateReadStreamPointer();
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
                    _pendingByteValue = (byte)PeekByte();
                }

                var bit = (_pendingByteValue >> (_bitsPosition % 8)) & 0x1L;
                var bitValue = bit << i;
                returnValue += bitValue;
                _bitsPosition++;
                MovePointerOnByteBoundary();
            }
            UpdateReadStreamPointer();
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
                    _pendingByteValue = (byte)PeekByte();
                }

                var bit = (ulong)((_pendingByteValue >> (_bitsPosition % 8))) & 0x1;
                var bitValue = bit << i;
                returnValue += bitValue;
                _bitsPosition++;
                MovePointerOnByteBoundary();
            }
            UpdateReadStreamPointer();
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
                        _pendingByteValue = (byte)PeekByte();
                    }

                    var bit = (_pendingByteValue >> (_bitsPosition % 8)) & 0x1;
                    var bitValue = bit << i;
                    buffer[b] += (byte)bitValue;
                    _bitsPosition++;
                    MovePointerOnByteBoundary();
                }
            }
            UpdateReadStreamPointer();
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
                        _pendingByteValue = (byte)PeekByte();
                    }

                    var bit = (_pendingByteValue >> (_bitsPosition % 8)) & 0x1;
                    var bitValue = bit << i;
                    buffer[b] += (char)bitValue;
                    _bitsPosition++;
                    MovePointerOnByteBoundary();
                }
            }
            UpdateReadStreamPointer();
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
                        _pendingByteValue = (byte)PeekByte();
                    }

                    var bit = (_pendingByteValue >> (_bitsPosition % 8)) & 0x1;
                    var bitValue = bit << i;
                    destination[b] += (byte)bitValue;
                    _bitsPosition++;
                    MovePointerOnByteBoundary();
                }
            }
            UpdateReadStreamPointer();
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
                        _pendingByteValue = (byte)PeekByte();
                    }

                    var bit = (_pendingByteValue >> (_bitsPosition % 8)) & 0x1;
                    var bitValue = bit << i;
                    destination[b] += (char)bitValue;
                    _bitsPosition++;
                    MovePointerOnByteBoundary();
                }
            }
            UpdateReadStreamPointer();
            return bytesRead;
        }
#endif

        internal T ReadCustomInternal<T>()
            where T : ICustomType
        {
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
        /// Skip a specified number of bytes
        /// </summary>
        /// <param name="count"></param>
        public void SkipBytes(int count)
        {
            if (_bitsPosition == 0)
            {
                base.Position += count;
                UpdateReadStreamPointer();
            }
            else
                _bitsPosition += count * 8;
        }

        /// <summary>
        /// Skip a specified number of bits
        /// </summary>
        /// <param name="bits"></param>
        public void SkipBits(int bits)
        {
            _bitsPosition += bits;
        }

        /// <summary>
        /// Set the bits position of the stream
        /// </summary>
        /// <param name="bitsPosition"></param>
        public void SetBitsPosition(int bitsPosition)
        {
            _bitsPosition = bitsPosition;
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
        internal virtual void MovePointerOnByteBoundary()
        {
            if (_bitsPosition > 0 && _bitsPosition % 8 == 0)
            {
                _bitsPosition = 0;
                _pendingByteValue = 0;
                Position++;
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
            _bitsPosition = 0;
            _pendingByteValue = 0;
            _hasPendingWrites = false;
            UpdateWriteStreamPointer();
        }

        /// <summary>
        /// Replace the underlying buffer without allocating new resources.
        /// The position will be reset to 0, and the length and capacity will adopt the new buffer's length.
        /// </summary>
        /// <param name="buffer">The new buffer to use</param>
        public void ReplaceBuffer(byte[] buffer)
        {
            // ensure the base stream no longer has references to the old buffer, so it is garbage collected
            _bufferField.SetValue(this, null);

            _capacityField.SetValue(this, buffer.Length);
            _originField.SetValue(this, 0);
            _expandableField.SetValue(this, false);
            _writableField.SetValue(this, true);
            _exposableField.SetValue(this, true);
            _isOpenField.SetValue(this, true);
            _positionField.SetValue(this, 0);
            _lengthField.SetValue(this, buffer.Length);
            _bufferField.SetValue(this, buffer);
        }

        /// <summary>
        /// If a base stream is used, update it's information
        /// </summary>
        private void UpdateWriteStreamPointer()
        {
            if (_streamPointer != null)
            {
                _capacityField.SetValue(_streamPointer, _capacityField.GetValue(this));
                _originField.SetValue(_streamPointer, _originField.GetValue(this));
                _expandableField.SetValue(_streamPointer, _expandableField.GetValue(this));
                _writableField.SetValue(_streamPointer, _writableField.GetValue(this));
                _exposableField.SetValue(_streamPointer, _exposableField.GetValue(this));
                _isOpenField.SetValue(_streamPointer, _isOpenField.GetValue(this));
                _positionField.SetValue(_streamPointer, _positionField.GetValue(this));
                _lengthField.SetValue(_streamPointer, _lengthField.GetValue(this));
                _bufferField.SetValue(_streamPointer, _bufferField.GetValue(this));
            }
        }

        /// <summary>
        /// If a base stream is used, update it's information
        /// </summary>
        private void UpdateReadStreamPointer()
        {
            if (_streamPointer != null)
            {
                _positionField.SetValue(_streamPointer, _positionField.GetValue(this));
            }
        }

        private void InitializeStreamPointer(MemoryStream memoryStream)
        {
            _capacityField.SetValue(this, _capacityField.GetValue(memoryStream));
            _originField.SetValue(this, _originField.GetValue(memoryStream));
            _expandableField.SetValue(this, _expandableField.GetValue(memoryStream));
            _writableField.SetValue(this, _writableField.GetValue(memoryStream));
            _exposableField.SetValue(this, _exposableField.GetValue(memoryStream));
            _isOpenField.SetValue(this, _isOpenField.GetValue(memoryStream));
            _positionField.SetValue(this, _positionField.GetValue(memoryStream));
            _lengthField.SetValue(this, _lengthField.GetValue(memoryStream));
            _bufferField.SetValue(this, _bufferField.GetValue(memoryStream));
        }

        /// <summary>
        /// Get the number of bits for a given type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static int BitsSizeOf<T>()
        {
            var type = typeof(T);
            if (type == typeof(bool))
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
            else if (type == typeof(Bit))
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
            // if we are using a stream pointer, don't dispose anything as we don't own the resources.
            if (_streamPointer != null)
                return;

            if (disposing)
            {
                if (_bitsPosition > 0 && _hasPendingWrites)
                    ForceFlush();
            }
            base.Dispose(disposing);
        }
    }
}
