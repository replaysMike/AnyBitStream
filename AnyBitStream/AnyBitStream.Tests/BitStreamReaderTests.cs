using System;
using NUnit.Framework;

namespace AnyBitStream.Tests
{
    [TestFixture]
    public class BitStreamReaderTests
    {
        private byte[] _testData1 = new byte[] { 30 };
        private byte[] _testData2 = new byte[] { 94, 57, 151, 222, 27 };
        private byte[] _testData3 = new byte[] { 222, 63 };

        [Test]
        public void Should_ReadBit()
        {
            var stream = new BitStream(_testData1);
            var reader = new BitStreamReader(stream);
            var bit1 = reader.ReadBit();
            var bit2 = reader.ReadBit();
            Assert.AreEqual(0, bit1);
            Assert.AreEqual(1, bit2);
        }

        [Test]
        public void Should_ReadBytes()
        {
            var bytes = new byte[] { 0xFF, 0x01, 0xCC, 0xAA };
            var stream = new BitStream(bytes);
            var reader = new BitStreamReader(stream);
            var readBytes = new byte[4];
            reader.Read(readBytes, 0, readBytes.Length);
            CollectionAssert.AreEqual(bytes, readBytes);
        }

        [Test]
        public void Should_ReadBitsAndBytes()
        {
            var bytes = new byte[] { 0xFF, 0x01, 0xCC, 0xAA };
            var stream = new BitStream(bytes);
            stream.AllowUnalignedOperations = true;
            var reader = new BitStreamReader(stream);
            reader.ReadBit();
            reader.ReadBit();
            var readBytes = new byte[4];
            reader.Read(readBytes, 0, readBytes.Length);
            // all bits will be shifted by 2
            CollectionAssert.AreEqual(new byte[] { 127, 0, 179, 234 }, readBytes);
        }

        [Test]
        public void ShouldNot_ReadBitsAndBytes()
        {
            var bytes = new byte[] { 0xFF, 0x01, 0xCC, 0xAA };
            var stream = new BitStream(bytes);
            stream.AllowUnalignedOperations = false;
            var reader = new BitStreamReader(stream);
            var bit1 = reader.ReadBit();
            var bit2 = reader.ReadBit();
            var readBytes = new byte[4];
            Assert.Throws<StreamUnalignedException>(() => reader.Read(readBytes, 0, readBytes.Length));
        }

        [Test]
        public void ShouldNot_AssertMixedBehavior()
        {
            var bytes = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            var stream = new BitStream(bytes);
            stream.AllowUnalignedOperations = true;
            var reader = new BitStreamReader(stream);
            var bit1 = reader.ReadUInt4();
            Assert.AreEqual(4, stream.BitsPosition);
            Assert.AreEqual(0, stream.Position);
            var bit2 = reader.ReadUInt4();
            Assert.AreEqual(0, stream.BitsPosition);
            Assert.AreEqual(1, stream.Position);
            var bit3 = reader.ReadUInt24();
            Assert.AreEqual(0, stream.BitsPosition);
            Assert.AreEqual(4, stream.Position);
            var bit4 = reader.ReadUInt24();
            Assert.AreEqual(0, stream.BitsPosition);
            Assert.AreEqual(7, stream.Position);
            var bit5 = reader.ReadUInt32();
            Assert.AreEqual(0, stream.BitsPosition);
            Assert.AreEqual(11, stream.Position);
            var bit6 = reader.ReadUInt4();
            Assert.AreEqual(4, stream.BitsPosition);
            Assert.AreEqual(11, stream.Position);
            var bit7 = reader.ReadUInt12();
            Assert.AreEqual(0, stream.BitsPosition);
            Assert.AreEqual(13, stream.Position);
        }

        [Test]
        public void Should_ReadInt32()
        {
            var bytes = BitConverter.GetBytes(123);
            var stream = new BitStream(bytes);
            var reader = new BitStreamReader(stream);
            var val = reader.ReadInt32();
            Assert.AreEqual(sizeof(int), stream.Length);
            Assert.AreEqual(bytes[0], val);
        }

        [Test]
        public void Should_ReadInt64()
        {
            var bytes = BitConverter.GetBytes(123435L);
            var stream = new BitStream(bytes);
            var reader = new BitStreamReader(stream);
            var val = reader.ReadInt64();
            Assert.AreEqual(sizeof(long), stream.Length);
            Assert.AreEqual(123435L, val);
            CollectionAssert.AreEqual(bytes, BitConverter.GetBytes(val));
        }

        [Test]
        public void Should_ReadInt2()
        {
            var bytes = new byte[] { 0x01 };
            var stream = new BitStream(bytes);
            var reader = new BitStreamReader(stream);
            var val = reader.ReadInt2();
            Assert.AreEqual(Int2.ByteSize, stream.Length);
            Assert.AreEqual(bytes[0], val);
        }

        [Test]
        public void Should_ReadInt4()
        {
            var bytes = new byte[] { 6 };
            var stream = new BitStream(bytes);
            var reader = new BitStreamReader(stream);
            var val = reader.ReadInt4();
            Assert.AreEqual(Int4.ByteSize, stream.Length);
            Assert.AreEqual(bytes[0], val);
        }

        [Test]
        public void Should_ReadInt7()
        {
            var bytes = new byte[] { 61 };
            var stream = new BitStream(bytes);
            var reader = new BitStreamReader(stream);
            var val = reader.ReadInt7();
            Assert.AreEqual(Int7.ByteSize, stream.Length);
            Assert.AreEqual(bytes[0], val);
        }

        [Test]
        public void Should_ReadInt10()
        {
            var bytes = new byte[] { 232, 1 };
            var stream = new BitStream(bytes);
            var reader = new BitStreamReader(stream);
            var val = reader.ReadInt10();
            Assert.AreEqual(Int10.ByteSize, stream.Length);
            Assert.AreEqual(488, val);
        }

        [Test]
        public void Should_ReadInt12()
        {
            var bytes = new byte[] { 59, 6 };
            var stream = new BitStream(bytes);
            var reader = new BitStreamReader(stream);
            var val = reader.ReadInt12();
            Assert.AreEqual(Int12.ByteSize, stream.Length);
            Assert.AreEqual(1595, val);
        }

        [Test]
        public void Should_ReadInt24()
        {
            var bytes = new byte[] { 159, 231, 119 }; // 7858079
            var stream = new BitStream(bytes);
            var reader = new BitStreamReader(stream);
            var val = reader.ReadInt24();
            Assert.AreEqual(Int24.ByteSize, stream.Length);
            Assert.AreEqual(7858079, val);
        }

        [Test]
        public void Should_ReadInt48()
        {
            var bytes = new byte[] { 127, 166, 110, 232, 91, 122 }; // 134535160178303
            var stream = new BitStream(bytes);
            var reader = new BitStreamReader(stream);
            var val = reader.ReadInt48();
            Assert.AreEqual(Int48.ByteSize, stream.Length);
            Assert.AreEqual(134535160178303L, val);
        }

        [Test]
        public void Should_ReadUe()
        {
            var bytes = new byte[] { 0, 24, 185 }; // 6300
            var stream = new BitStream(bytes);
            var reader = new BitStreamReader(stream);
            var val = reader.ReadUe(out var bitCount);
            Assert.AreEqual(6300, val);
            Assert.AreEqual(23, bitCount);
        }

        [Test]
        public void Should_ReadSe()
        {
            var bytes = new byte[] { 0, 48, 114, 2 }; // -6300
            var stream = new BitStream(bytes);
            var reader = new BitStreamReader(stream);
            var val = reader.ReadSe(out var bitCount);
            Assert.AreEqual(-6300, val);
            Assert.AreEqual(26, bitCount);
        }

        [Test]
        public void Should_ReadTe()
        {
            var bytes = new byte[] { 0, 24, 185 }; // 6300
            var stream = new BitStream(bytes);
            var reader = new BitStreamReader(stream);
            var val = reader.ReadTe(2, out var bitCount);
            Assert.AreEqual(6300, val);
            Assert.AreEqual(23, bitCount);
        }
    }
}
