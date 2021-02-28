using System;
using System.Runtime.Serialization;

namespace AnyBitStream
{
    /// <summary>
    /// Stream bits are unaligned exception
    /// </summary>
    [Serializable]
    public class StreamUnalignedException : Exception
    {
        /// <summary>
        /// Stream bits are unaligned exception
        /// </summary>
        public StreamUnalignedException()
        {
        }

        /// <summary>
        /// Stream bits are unaligned exception
        /// </summary>
        /// <param name="message"></param>
        public StreamUnalignedException(string message) : base(message)
        {
        }

        /// <summary>
        /// Stream bits are unaligned exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public StreamUnalignedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Stream bits are unaligned exception
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected StreamUnalignedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
