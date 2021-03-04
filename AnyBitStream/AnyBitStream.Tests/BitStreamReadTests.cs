using NUnit.Framework;

namespace AnyBitStream.Tests
{
    [TestFixture]
    public class BitStreamReadTests
    {
        private byte[] _testData1 = new byte[] { 30 };
        private byte[] _testData2 = new byte[] { 94, 57, 151, 222, 27 };
        private byte[] _testData3 = new byte[] { 222, 63 };

        [Test]
        public void Should_ReadBits()
        {
            var stream = new BitStream(_testData1);
            var bits1 = stream.ReadBits(2);
            var bits2 = stream.ReadBits(4);
            Assert.AreEqual(6, stream.BitsPosition);
            Assert.AreEqual(0, stream.Position);
            Assert.AreEqual(2, bits1);
            Assert.AreEqual(7, bits2);
        }

        [Test]
        public void Should_ReadManyBits()
        {
            var stream = new BitStream(_testData2);
            var bits1 = stream.ReadBits(2);
            var bits2 = stream.ReadBits(4);
            var bits3 = stream.ReadBits(12);
            var bits4 = stream.ReadBits(10);
            var bits5 = stream.ReadBits(5);
            var bits6 = stream.ReadBits(5);
            Assert.AreEqual(6, stream.BitsPosition);
            Assert.AreEqual(4, stream.Position);
            Assert.AreEqual(2, bits1);
            Assert.AreEqual(7, bits2);
            Assert.AreEqual(3301, bits3);
            Assert.AreEqual(933, bits4);
            Assert.AreEqual(29, bits5);
            Assert.AreEqual(13, bits6);
        }

        [Test]
        public void Should_ReadCustomBits()
        {
            var stream = new BitStream(_testData1);
            var bits1 = stream.ReadUInt2();
            var bits2 = stream.ReadUInt4();
            Assert.AreEqual(6, stream.BitsPosition);
            Assert.AreEqual(0, stream.Position);
            Assert.AreEqual(2, bits1);
            Assert.AreEqual(7, bits2);
        }

        [Test]
        public void Should_ReadManyCustomBits()
        {
            var stream = new BitStream(_testData2);
            var bits1 = stream.ReadUInt2();
            var bits2 = stream.ReadUInt4();
            var bits3 = stream.ReadUInt12();
            var bits4 = stream.ReadUInt10();
            var bits5 = stream.ReadUInt5();
            var bits6 = stream.ReadUInt5();
            Assert.AreEqual(6, stream.BitsPosition);
            Assert.AreEqual(4, stream.Position);
            Assert.AreEqual(2, bits1);
            Assert.AreEqual(7, bits2);
            Assert.AreEqual(3301, bits3);
            Assert.AreEqual(933, bits4);
            Assert.AreEqual(29, bits5);
            Assert.AreEqual(13, bits6);
        }

        [Test]
        public void Should_ReadBitsThenBytes()
        {
            var stream = new BitStream(_testData3);
            stream.AllowUnalignedOperations = true;
            var bits1 = stream.ReadUInt2();
            var bits2 = stream.ReadUInt4();
            // 6 bits written
            Assert.AreEqual(6, stream.BitsPosition);
            Assert.AreEqual(0, stream.Position);
            Assert.AreEqual(2, bits1);
            Assert.AreEqual(7, bits2);
            // read a full byte unaligned
            var byte1 = stream.ReadByte();
            Assert.AreEqual(0xFF, byte1);
        }

        [Test]
        public void ShouldNot_ReadBitsThenBytes()
        {
            var stream = new BitStream(_testData3);
            var bits1 = stream.ReadUInt2();
            var bits2 = stream.ReadUInt4();
            // 6 bits written
            Assert.AreEqual(6, stream.BitsPosition);
            Assert.AreEqual(0, stream.Position);
            Assert.AreEqual(2, bits1);
            Assert.AreEqual(7, bits2);
            // we are unaligned, this should fail
            Assert.Throws<StreamUnalignedException>(() => stream.ReadByte());
        }
    }
}
