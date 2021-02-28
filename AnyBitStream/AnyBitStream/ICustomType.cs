using System;

namespace AnyBitStream
{
    /// <summary>
    /// Custom type
    /// </summary>
    public interface ICustomType : IEquatable<long>, IEquatable<int>, IEquatable<short>, IEquatable<byte>
    {
        /// <summary>
        /// Get the number of bits in the type
        /// </summary>
        int TotalBits { get; }

        /// <summary>
        /// Get the number of bytes in the type
        /// </summary>
        int TotalBytes { get; }

        /// <summary>
        /// Get the bit value at a specific zero-based index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        Bit GetBit(int index);

        /// <summary>
        /// Get the bits as an array
        /// </summary>
        /// <returns></returns>
        Bit[] GetBits();
    }
}
