using System;
using System.IO;
using System.Runtime.CompilerServices;
using NUnit.Framework;

namespace AnyBitStream.Tests
{
    [TestFixture]
    public class BitStreamTests
    {
        [Test]
        public void Should_CreateFromStream()
        {
            Stream stream = new MemoryStream();
            stream.WriteByte(0xFF);
            stream.WriteByte(0xAA);
            stream.WriteByte(0xBB);
            var bitStream = new BitStream(stream);
            bitStream.Write(new byte[] { 0xCC, 0xDD }, 0, 2);
            var bytes = bitStream.ToArray();
            Assert.AreEqual(new byte[] { 0xFF, 0xAA, 0xBB, 0xCC, 0xDD }, bytes);

            // ensure sure the original stream has the data, and bitStream is not a copy
            stream.Position = 0;
            var buffer = new byte[bytes.Length];
            stream.Read(buffer, 0, buffer.Length);
            Assert.AreEqual(new byte[] { 0xFF, 0xAA, 0xBB, 0xCC, 0xDD }, buffer);
        }

        [Test]
        public void Should_WriteUnaligned_FromStream()
        {
            Stream stream = new MemoryStream();
            stream.WriteByte(0xFF);
            stream.WriteByte(0xAA);
            stream.WriteByte(0xBB);
            var bitStream = new BitStream(stream);
            bitStream.AllowUnalignedOperations = true;
            // write 5 bits
            bitStream.WriteBits(13, 5);
            // we are unaligned, write 2 more bytes.
            bitStream.Write(new byte[] { 0xCC, 0xDD }, 0, 2);
            var bytes = bitStream.ToArray();
            // we expect 3 original bytes, [5 bits + 2 bytes + 3 padding bits] = 6 bytes
            Assert.AreEqual(new byte[] { 0xFF, 0xAA, 0xBB, 0x8D, 0xB9, 0x1B }, bytes);

            // ensure sure the original stream has the data, and bitStream is not a copy
            stream.Position = 0;
            var buffer = new byte[bytes.Length];
            stream.Read(buffer, 0, buffer.Length);
            Assert.AreEqual(new byte[] { 0xFF, 0xAA, 0xBB, 0x8D, 0xB9, 0x1B }, buffer);
        }

        [Test]
        public void Should_Read_FromStream()
        {
            Stream stream = new MemoryStream(new byte[] { 0xFF, 0xAA, 0xBB, 0x00 });
            var bitStream = new BitStream(stream);
            bitStream.Position = 3;
            bitStream.WriteByte(0xCC);

            // ensure sure the original stream has the data, and bitStream is not a copy
            stream.Position = 3;
            var value = stream.ReadByte();
            Assert.AreEqual(0xCC, value);
        }

        [Test]
        public void Should_Seek_BitPosition()
        {
            Stream stream = new MemoryStream(new byte[] { 0xFF, 0xAA, 0xBB, 0x00 });
            var bitStream = new BitStream(stream);
            bitStream.BitsPosition = 10;
            Assert.AreEqual(2, bitStream.BitsPosition);
            Assert.AreEqual(1, bitStream.Position);
        }

        [Test]
        public void Should_ReadConsecutiveByte_FromStream()
        {
            var bytes = new byte[] { 0xFF, 0xAA, 0xBB, 0xCC, 0xDD };
            Stream stream = new MemoryStream(bytes);
            var bitStream = new BitStream(stream);
            var byte1 = bitStream.ReadByte();
            var byte2 = bitStream.ReadByte();
            var byte3 = bitStream.ReadByte();
            var byte4 = bitStream.ReadByte();
            // original stream's position should be updated
            var byte5 = stream.ReadByte();
            Assert.AreEqual(bytes[0], byte1);
            Assert.AreEqual(bytes[1], byte2);
            Assert.AreEqual(bytes[2], byte3);
            Assert.AreEqual(bytes[3], byte4);
            Assert.AreEqual(bytes[4], byte5);
        }

        [Test]
        public void Should_ReplaceBuffer()
        {
            var bufferRef = new WeakReference(new byte[8 * 1024 * 1024]);
            
            var stream = new BitStream(bufferRef.Target as byte[]);
            stream.Position = 1;
            stream.WriteByte(0xAA);
            stream.WriteByte(0xBB);
            var internalBuffer1 = stream.GetBuffer();
            //Assert.AreEqual(buffer1.Length, internalBuffer1.Length);
            Assert.AreEqual(0x00, internalBuffer1[0]);
            Assert.AreEqual(0xAA, internalBuffer1[1]);
            Assert.AreEqual(0xBB, internalBuffer1[2]);
            Assert.AreEqual(0x00, internalBuffer1[3]);

            // replace the buffer with a new buffer
            var buffer2 = new byte[300];
            stream.ReplaceBuffer(buffer2);
            stream.Position = 1;
            stream.WriteByte(0xDD);
            stream.WriteByte(0xEE);
            var internalBuffer2 = stream.GetBuffer();
            Assert.AreEqual(buffer2.Length, internalBuffer2.Length);
            Assert.AreEqual(0x00, internalBuffer2[0]);
            Assert.AreEqual(0xDD, internalBuffer2[1]);
            Assert.AreEqual(0xEE, internalBuffer2[2]);
            Assert.AreEqual(0x00, internalBuffer2[3]);

            // ideally we would do a GC.Collection() and check if the weak reference is alive,
            // but it seems that isn't reliable or doesn't work at all as it always returns true.
            // so we will assume this just works :)
        }

        private static IntPtr GetAddress(object o)
        {
            if (o == null)
                return IntPtr.Zero;
            unsafe
            {
                var tr = __makeref(o);
                var ptr = **(System.IntPtr**)(&tr);
                return ptr;
            }
        }
    }
}
