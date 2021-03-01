using NUnit.Framework;

namespace AnyBitStream.Tests
{
    [TestFixture]
    public class TypeEqualityTests
    {
        [Test]
        public void Should_Int2_Equals()
        {
            var value = new Int2(1);
            Assert.IsTrue(value.Equals(1));
            Assert.IsTrue(value.Equals((object)1));
            Assert.IsTrue(value.Equals(new Int2(1)));
            Assert.IsTrue(value.Equals(new UInt2(1)));
            Assert.IsTrue(value.Equals((object)new Int2(1)));
            Assert.IsTrue(value == new Int2(1));
            Assert.IsTrue(value == new UInt2(1));
            Assert.IsTrue(value == (object)new Int2(1));
            Assert.IsTrue(value == 1);
            Assert.IsTrue(value == 1L);
            Assert.IsTrue(1 == value);
        }

        [Test]
        public void ShouldNot_Int2_Equals()
        {
            var value = new Int2(1);
            Assert.IsTrue(!value.Equals(2));
            Assert.IsTrue(!value.Equals((object)2));
            Assert.IsTrue(!value.Equals(new Int2(2)));
            Assert.IsTrue(!value.Equals(new UInt2(2)));
            Assert.IsTrue(!value.Equals((object)new Int2(2)));
            Assert.IsTrue(value != new Int2(2));
            Assert.IsTrue(value != new UInt2(2));
            Assert.IsTrue(value != (object)new Int2(2));
            Assert.IsTrue(value != 2);
            Assert.IsTrue(value != 2L);

            Assert.IsFalse(value == new Int2(2));
            Assert.IsFalse(value == new UInt2(2));
            Assert.IsFalse(value == (object)new Int2(2));
            Assert.IsFalse(value == 2);
            Assert.IsFalse(value == 2L);

            Assert.IsFalse(new Int2(-1) == new UInt2(1));
            Assert.IsFalse(new Int2(-1).Equals(new UInt2(1)));
            Assert.IsTrue(new Int2(-1) != new UInt2(1));

        }
    }
}
