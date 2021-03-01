namespace AnyBitStream
{
    /// <summary>
    /// Math utilities
    /// </summary>
    public static class MathUtilities
    {
        private const int MaxInt32 = 0x7FFFFFFF;
        private const int MinInt32 = -MaxInt32 - 1;
        private static readonly byte[] _evxLog2Lut = new byte[] {
  0, 0, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3,
  4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4,
  5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
  5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5,
  6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
  6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
  6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
  6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6,
  7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
  7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
  7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
  7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
  7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
  7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
  7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
  7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7, 7,
};
        /// <summary>
        /// Get the number of bits required to encode value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetRequiredBits(byte value) => _evxLog2Lut[value];

        /// <summary>
        /// Get the number of bits required to encode value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetRequiredBits(ushort value)
        {
            if (value < 0xFF)
                return GetRequiredBits((byte)value);
            return 8 + GetRequiredBits((byte)(value >> 8));
        }

        /// <summary>
        /// Get the number of bits required to encode value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int GetRequiredBits(uint value)
        {
            if (value < 0xFFFF)
                return GetRequiredBits((ushort)value);
            return 16 + GetRequiredBits((ushort)(value >> 16));
        }

        /// <summary>
        /// Get the absolute value, within the valid Int32 range
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int Abs(int value)
        {
            if (value == MinInt32)
            {
                return MaxInt32;
            }
            return (value < 0 ? -value : value);
        }
    }
}
