using NUnit.Framework;

namespace AnyBitStream.Tests
{
    [TestFixture]
    public class BitTests
    {
        [Test]
        public void Should_Equal()
        {
            var bit = new Bit(1);
            Assert.AreEqual(1, bit);
            Assert.AreEqual(true, bit);
            Assert.IsTrue(bit == 1);
            Assert.IsTrue(bit.Equals(1));
            bit = new Bit(0);
            Assert.AreEqual(0, bit);
            Assert.AreEqual(false, bit);
            Assert.IsTrue(bit == false);
            Assert.IsTrue(bit == 0);
        }

        [Test]
        public void Should_NotEqual()
        {
            var bit = new Bit(1);
            Assert.AreNotEqual(0, bit);
            Assert.AreNotEqual(false, bit);
            Assert.IsFalse(bit == false);
            Assert.IsFalse(bit.Equals(false));
            bit = new Bit(0);
            Assert.AreNotEqual(1, bit);
            Assert.AreNotEqual(true, bit);
            Assert.IsFalse(bit == true);
            Assert.IsFalse(bit.Equals(true));
        }

        [Test]
        public void Should_Add()
        {
            var bit = new Bit(1);
            Assert.AreEqual(15, bit + 14);
            Assert.AreEqual(15, 14 + bit);
            bit = 0;
            Assert.AreEqual(0, bit);
            bit++;
            Assert.AreEqual(1, bit);
        }

        [Test]
        public void Should_Subtract()
        {
            var bit = new Bit(1);
            Assert.AreEqual(-13, bit - 14);
            Assert.AreEqual(13, 14 - bit);
            Assert.AreEqual(1, bit);
            bit--;
            Assert.AreEqual(0, bit);
        }

        [Test]
        public void Should_ShiftLeft()
        {
            var bit = new Bit(1);
            Assert.AreEqual(0, bit >> 1);
        }

        [Test]
        public void Should_ShiftRight()
        {
            var bit = new Bit(1);
            Assert.AreEqual(2, bit << 1);
            Assert.AreEqual(4, bit << 2);
            Assert.AreEqual(8, bit << 3);
            Assert.AreEqual(16, bit << 4);
            Assert.AreEqual(32, bit << 5);
        }
    }
}
