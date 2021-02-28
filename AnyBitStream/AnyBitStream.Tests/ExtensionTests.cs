using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyBitStream.Tests
{
    [TestFixture]
    public class ExtensionTests
    {
        [Test]
        public void Should_ShiftBytes()
        {
            var bytes = new byte[] { 0xFF, 0xAA, 0xBB };
            var shiftedBytes = bytes.ShiftBits(2);
            Assert.AreEqual(bytes.Length + 1, shiftedBytes.Length);
            Assert.AreEqual(new byte[] { 0xFC, 0xAB, 0xEE, 0x03  }, shiftedBytes);
        }

        [Test]
        public void Should_ShiftByte()
        {
            byte value = 0xFF;
            var shiftedByte = value.ShiftBits(2);
            Assert.AreEqual(0xFC, shiftedByte);
        }

        [Test]
        public void Should_ShiftShort()
        {
            short value = 7031;
            var shiftedByte = value.ShiftBits(2);
            Assert.AreEqual(28124, shiftedByte);
        }

        [Test]
        public void Should_ShiftInt()
        {
            int value = 1539218615;
            var shiftedByte = value.ShiftBits(2);
            Assert.AreEqual(1861907164, shiftedByte);
        }

        [Test]
        public void Should_ShiftLong()
        {
            long value = 7301155571653538342L;
            var shiftedByte = value.ShiftBits(2);
            Assert.AreEqual(-7688865860804949864L, shiftedByte);
        }
    }
}
