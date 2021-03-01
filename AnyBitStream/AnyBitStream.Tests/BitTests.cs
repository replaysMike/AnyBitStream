using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
