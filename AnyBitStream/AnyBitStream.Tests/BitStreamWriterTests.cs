using NUnit.Framework;
using System;
using System.IO;
using System.Text;

namespace AnyBitStream.Tests
{
    [TestFixture]
    public class BitStreamWriterTests
    {
        [Test]
        public void Should_WriteBit()
        {
            var stream = new BitStream();
            var writer = new BitStreamWriter(stream, Encoding.UTF8, true);
            writer.WriteBit(0);
            writer.WriteBit(1);
            writer.Flush();
            Assert.AreEqual(1, stream.Length);
            var bytes = stream.ToArray();
            // we wrote 01 binary, which is 2
            Assert.AreEqual(2, bytes[0]);
        }

        [Test]
        public void Should_WriteBytes()
        {
            var stream = new BitStream();
            var writer = new BitStreamWriter(stream, Encoding.UTF8, true);
            var val = new byte[] { 0xFF, 0x01, 0xCC, 0xAA };
            writer.Write(val);
            Assert.AreEqual(val.Length, stream.Length);
            var bytes = stream.ToArray();
            // we wrote 01 binary, which is 2
            CollectionAssert.AreEqual(val, bytes);
        }

        [Test]
        public void Should_WriteBitsAndBytes()
        {
            var stream = new BitStream();
            stream.AllowUnalignedOperations = true;
            var writer = new BitStreamWriter(stream, Encoding.UTF8, true);
            var val = new byte[] { 0xFF, 0x01, 0xCC, 0xAA };
            writer.WriteBit(0);
            writer.WriteBit(1);
            writer.Write(val);
            writer.Flush();
            Assert.AreEqual(val.Length + 1, stream.Length);
            var bytes = stream.ToArray();
            CollectionAssert.AreNotEqual(val, bytes);
            CollectionAssert.AreEqual(new byte[] { 254, 7, 48, 171, 2 }, bytes);
        }

        [Test]
        public void ShouldNot_WriteBitsAndBytes()
        {
            var stream = new BitStream();
            stream.AllowUnalignedOperations = false;
            var writer = new BitStreamWriter(stream, Encoding.UTF8, true);
            var val = new byte[] { 0xFF, 0x01, 0xCC, 0xAA };
            writer.WriteBit(0);
            writer.WriteBit(1);
            Assert.Throws<StreamUnalignedException>(() => writer.Write(val));
        }

        [Test]
        public void Should_WriteInt32()
        {
            var stream = new BitStream();
            var writer = new BitStreamWriter(stream, Encoding.UTF8, true);
            var val = 123;
            writer.Write(val);
            Assert.AreEqual(sizeof(int), stream.Length);
            Assert.AreEqual(BitConverter.GetBytes(val), stream.ToArray());
        }

        [Test]
        public void Should_WriteInt64()
        {
            var stream = new BitStream();
            var writer = new BitStreamWriter(stream, Encoding.UTF8, true);
            var val = 12355222L;
            writer.Write(val);
            Assert.AreEqual(sizeof(long), stream.Length);
            Assert.AreEqual(BitConverter.GetBytes(val), stream.ToArray());
        }

        [Test]
        public void Should_WriteInt2()
        {
            var stream = new BitStream();
            var writer = new BitStreamWriter(stream, Encoding.UTF8, true);
            var val = Int2.MaxValue;
            writer.Write(val);
            writer.Flush();
            Assert.AreEqual(1, stream.Length);
            var bytes = stream.ToArray();
            var valBits = val.GetBits();
            var byteBits = bytes[0].GetBits(Int2.BitSize);
            CollectionAssert.AreEqual(valBits, byteBits);
        }

        [Test]
        public void Should_WriteInt4()
        {
            var stream = new BitStream();
            var writer = new BitStreamWriter(stream, Encoding.UTF8, true);
            var val = Int4.MaxValue;
            writer.Write(val);
            writer.Flush();
            Assert.AreEqual(1, stream.Length);
            var bytes = stream.ToArray();
            var valBits = val.GetBits();
            var byteBits = bytes[0].GetBits(Int4.BitSize);
            CollectionAssert.AreEqual(valBits, byteBits);
        }

        [Test]
        public void Should_WriteInt7()
        {
            var stream = new BitStream();
            var writer = new BitStreamWriter(stream, Encoding.UTF8, true);
            var val = Int7.MaxValue;
            writer.Write(val);
            writer.Flush();
            Assert.AreEqual(1, stream.Length);
            var bytes = stream.ToArray();
            var valBits = val.GetBits();
            var byteBits = bytes[0].GetBits(Int7.BitSize);
            CollectionAssert.AreEqual(valBits, byteBits);
        }

        [Test]
        public void Should_WriteInt10()
        {
            var stream = new BitStream();
            var writer = new BitStreamWriter(stream, Encoding.UTF8, true);
            var val = Int10.MaxValue;
            writer.Write(val);
            writer.Flush();
            Assert.AreEqual(2, stream.Length);
            var bytes = stream.ToArray();
            var valBits = val.GetBits();
            var byteBits = bytes.GetBits(Int10.BitSize);
            CollectionAssert.AreEqual(valBits, byteBits);
        }

        [Test]
        public void Should_WriteInt12()
        {
            var stream = new BitStream();
            var writer = new BitStreamWriter(stream, Encoding.UTF8, true);
            var val = Int12.MaxValue;
            writer.Write(val);
            writer.Flush();
            Assert.AreEqual(2, stream.Length);
            var bytes = stream.ToArray();
            var valBits = val.GetBits();
            var byteBits = bytes.GetBits(Int12.BitSize);
            CollectionAssert.AreEqual(valBits, byteBits);
        }

        [Test]
        public void Should_WriteInt24()
        {
            var stream = new BitStream();
            var writer = new BitStreamWriter(stream, Encoding.UTF8, true);
            var val = Int24.MaxValue;
            writer.Write(val);
            writer.Flush();
            Assert.AreEqual(3, stream.Length);
            var bytes = stream.ToArray();
            var valBits = val.GetBits();
            var byteBits = bytes.GetBits(Int24.BitSize);
            CollectionAssert.AreEqual(valBits, byteBits);
        }

        [Test]
        public void Should_WriteInt48()
        {
            var stream = new BitStream();
            var writer = new BitStreamWriter(stream, Encoding.UTF8, true);
            var val = Int48.MaxValue;
            writer.Write(val);
            writer.Flush();
            Assert.AreEqual(6, stream.Length);
            var bytes = stream.ToArray();
            var valBits = val.GetBits();
            var byteBits = bytes.GetBits(Int48.BitSize);
            CollectionAssert.AreEqual(valBits, byteBits);
        }

        [Test]
        public void Should_WriteUe()
        {
            var stream = new BitStream();
            var writer = new BitStreamWriter(stream, Encoding.UTF8, true);
            var val = 6300U;
            var bitsWritten = writer.WriteUe(val);
            writer.Flush();
            Assert.AreEqual(24, bitsWritten);
            var bytes = stream.ToArray();
            Assert.AreEqual(new byte[] { 0, 24, 185 }, bytes);
        }

        [Test]
        public void Should_WriteSe()
        {
            var stream = new BitStream();
            var writer = new BitStreamWriter(stream, Encoding.UTF8, true);
            var val = -6300;
            var bitsWritten = writer.WriteSe(val);
            Assert.AreEqual(26, bitsWritten);
            var bytes = stream.ToArray();
            Assert.AreEqual(new byte[] { 0, 48, 114, 2 }, bytes);
        }

        [Test]
        public void Should_WriteTe()
        {
            var stream = new BitStream();
            var writer = new BitStreamWriter(stream, Encoding.UTF8, true);
            var val = 6300U;
           var bitsWritten = writer.WriteTe(val, 30000);
            Assert.AreEqual(24, bitsWritten);
            var bytes = stream.ToArray();
            Assert.AreEqual(new byte[] { 0, 24, 185 }, bytes);
        }
    }
}
