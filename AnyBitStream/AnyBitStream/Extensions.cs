using System;
using System.Collections.Generic;
using System.Linq;

namespace AnyBitStream
{
    public static class Extensions
    {
        /// <summary>
        /// Convert a list to a list of bits
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bits"></param>
        /// <returns></returns>
        public static Bit[] ToArray<T>(this IEnumerable<T> bits)
            where T : struct
        {
            return bits.Cast<Bit>().ToArray();
        }

        /// <summary>
        /// Get the bits in a byte
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Bit[] GetBits(this byte value) => GetBits(value, sizeof(byte) * 8);

        /// <summary>
        /// Get the bits in a byte
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bits">The number of bits to get</param>
        /// <returns></returns>
        public static Bit[] GetBits(this byte value, int bits)
        {
            var bitArray = new Bit[bits];
            for(var i = 0; i < bitArray.Length; i++)
            {
                bitArray[i] = (Bit)(value >> i & 0x1);
            }
            return bitArray;
        }

        /// <summary>
        /// Get the bits in a byte array
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Bit[] GetBits(this byte[] value) => GetBits(value, sizeof(byte) * value.Length);

        /// <summary>
        /// Get the bits in a byte array
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bits">The number of bits to get</param>
        /// <returns></returns>
        public static Bit[] GetBits(this byte[] value, int bits)
        {
            var bitArray = new Bit[bits];
            var bitsInByte = sizeof(byte) * 8;
            for (var i = 0; i < bitArray.Length; i++)
            {
                var b = i / bitsInByte;
                bitArray[i] = (Bit)(value[b] >> (i % bitsInByte) & 0x1);
            }
            return bitArray;
        }

        /// <summary>
        /// Get the bits in a short
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Bit[] GetBits(this short value) => GetBits(value, sizeof(short) * 8);

        /// <summary>
        /// Get the bits in a short
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bits">The number of bits to get</param>
        /// <returns></returns>
        public static Bit[] GetBits(this short value, int bits)
        {
            var bitArray = new Bit[bits];
            for (var i = 0; i < bitArray.Length; i++)
            {
                bitArray[i] = (Bit)(value >> i & 0x1);
            }
            return bitArray;
        }

        /// <summary>
        /// Get the bits in a int
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Bit[] GetBits(this int value) => GetBits(value, sizeof(int) * 8);

        /// <summary>
        /// Get the bits in a int
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bits">The number of bits to get</param>
        /// <returns></returns>
        public static Bit[] GetBits(this int value, int bits)
        {
            var bitArray = new Bit[bits];
            for (var i = 0; i < bitArray.Length; i++)
            {
                bitArray[i] = (Bit)(value >> i & 0x1);
            }
            return bitArray;
        }

        /// <summary>
        /// Get the bits in a long
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Bit[] GetBits(this long value) => GetBits(value, sizeof(long) * 8);

        /// <summary>
        /// Get the bits in a long
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bits">The number of bits to get</param>
        /// <returns></returns>
        public static Bit[] GetBits(this long value, int bits)
        {
            var bitArray = new Bit[bits];
            for (var i = 0; i < bitArray.Length; i++)
            {
                bitArray[i] = (Bit)(value >> i & 0x1);
            }
            return bitArray;
        }

        /// <summary>
        /// Shift a value by a specified number of bits. Remainder bits will be lost
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bits">The number of bits to shift by, cannot exceed the type's number of bits.</param>
        /// <returns></returns>
        public static byte ShiftBits(this byte value, int bits) => bits <= (sizeof(byte) * 8) ? (byte)(value << bits) : throw new ArgumentOutOfRangeException(nameof(bits), $"Cannot shift value more than {sizeof(byte) * 8} bits");

        /// <summary>
        /// Shift a value by a specified number of bits. Remainder bits will be lost
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bits">The number of bits to shift by, cannot exceed the type's number of bits.</param>
        /// <returns></returns>
        public static short ShiftBits(this short value, int bits) => bits <= (sizeof(short) * 8) ? (short)(value << bits) : throw new ArgumentOutOfRangeException(nameof(bits), $"Cannot shift value more than {sizeof(short) * 8} bits");

        /// <summary>
        /// Shift a value by a specified number of bits. Remainder bits will be lost
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bits">The number of bits to shift by, cannot exceed the type's number of bits.</param>
        /// <returns></returns>
        public static int ShiftBits(this int value, int bits) => bits <= (sizeof(int) * 8) ? (value << bits) : throw new ArgumentOutOfRangeException(nameof(bits), $"Cannot shift value more than {sizeof(int) * 8} bits");

        /// <summary>
        /// Shift a value by a specified number of bits. Remainder bits will be lost
        /// </summary>
        /// <param name="value"></param>
        /// <param name="bits">The number of bits to shift by, cannot exceed the type's number of bits.</param>
        /// <returns></returns>
        public static long ShiftBits(this long value, int bits) => bits <= (sizeof(long) * 8) ? (value << bits) : throw new ArgumentOutOfRangeException(nameof(bits), $"Cannot shift value more than {sizeof(long) * 8} bits");

        /// <summary>
        /// Shift a byte array by a specified number of bits
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="bits">The number of bits to shift by, cannot exceed the type's number of bits.</param>
        /// <returns></returns>
        public static byte[] ShiftBits(this byte[] bytes, int bits)
        {
            if (bits > 8)
                throw new ArgumentOutOfRangeException(nameof(bits), $"Cannot shift byte array more than 8 bits");
            var returnBytes = new byte[bytes.Length + 1];
            byte nextBits = 0;
            byte originalValue;
            for(var i = 0; i < returnBytes.Length; i++)
            {
                if (i < bytes.Length)
                    originalValue = bytes[i];
                else
                    originalValue = 0;
                var newByte = (byte)(((originalValue << bits) & 0xFF) + nextBits);
                nextBits = (byte)(((originalValue << bits) & 0xFF) >> (8 - bits));
                returnBytes[i] = newByte;
            }

            return returnBytes;
        }
    }
}
