using NUnit.Framework;

namespace AnyBitStream.Tests
{
    [TestFixture]
    public class TypeTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Should_SerializeInt2()
        {
            Int2 value = Int2.MaxValue; // range -1 to 1
            Assert.AreEqual(false, value._sign);
            Assert.AreEqual(1, value.GetBit(0));
            Assert.AreEqual(0, value.GetBit(1));
            Assert.AreEqual(new Bit[] { 1, 0 }, value.GetBits());

            value = Int2.MinValue;
            Assert.AreEqual(true, value._sign);
            Assert.AreEqual(1, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(new Bit[] { 1, 1 }, value.GetBits());

            // test overflow
            value = (Int2)7;
            Assert.AreEqual(1, value);
            Assert.AreEqual(false, value._sign);
            Assert.AreEqual(1, value.GetBit(0));
            Assert.AreEqual(0, value.GetBit(1));
            Assert.AreEqual(new Bit[] { 1, 0 }, value.GetBits());
        }

        [Test]
        public void Should_SerializeUInt2()
        {
            UInt2 value = UInt2.MaxValue; // range 0 to 3
            Assert.AreEqual(1, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(new Bit[] { 1, 1 }, value.GetBits());

            value = (UInt2)2;
            Assert.AreEqual(0, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(new Bit[] { 0, 1 }, value.GetBits());

            // test overflow
            value = (UInt2)5;
            Assert.AreEqual(1, value);
            Assert.AreEqual(1, value.GetBit(0));
            Assert.AreEqual(0, value.GetBit(1));
            Assert.AreEqual(new Bit[] { 1, 0 }, value.GetBits());
        }

        [Test]
        public void Should_SerializeInt3()
        {
            Int3 value = Int3.MaxValue; // range -3 to 3
            Assert.AreEqual(false, value._sign);
            Assert.AreEqual(1, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(new Bit[] { 1, 1, 0 }, value.GetBits());

            value = Int3.MinValue;
            Assert.AreEqual(true, value._sign);
            Assert.AreEqual(1, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(new Bit[] { 1, 1, 1 }, value.GetBits());

            // test overflow
            value = (Int3)5;
            Assert.AreEqual(1, value);
            Assert.AreEqual(false, value._sign);
            Assert.AreEqual(1, value.GetBit(0));
            Assert.AreEqual(0, value.GetBit(1));
            Assert.AreEqual(new Bit[] { 1, 0, 0 }, value.GetBits());
        }

        [Test]
        public void Should_SerializeUInt3()
        {
            UInt3 value = UInt3.MaxValue; // range 0 to 7
            Assert.AreEqual(1, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(1, value.GetBit(2));
            Assert.AreEqual(new Bit[] { 1, 1, 1 }, value.GetBits());

            value = (UInt3)2;
            Assert.AreEqual(0, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(new Bit[] { 0, 1, 0 }, value.GetBits());

            // test overflow
            value = (UInt3)12;
            Assert.AreEqual(4, value);
            Assert.AreEqual(0, value.GetBit(0));
            Assert.AreEqual(0, value.GetBit(1));
            Assert.AreEqual(1, value.GetBit(2));
            Assert.AreEqual(new Bit[] { 0, 0, 1 }, value.GetBits());
        }

        [Test]
        public void Should_SerializeInt4()
        {
            Int4 value = Int4.MaxValue; // range -7 to 7
            Assert.AreEqual(false, value._sign);
            Assert.AreEqual(1, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(1, value.GetBit(2));
            Assert.AreEqual(new Bit[] { 1, 1, 1, 0 }, value.GetBits());

            value = Int4.MinValue;
            Assert.AreEqual(true, value._sign);
            Assert.AreEqual(1, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(1, value.GetBit(2));
            Assert.AreEqual(new Bit[] { 1, 1, 1, 1 }, value.GetBits());

            // test overflow
            value = (Int4)10;
            Assert.AreEqual(2, value);
            Assert.AreEqual(false, value._sign);
            Assert.AreEqual(0, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(0, value.GetBit(2));
            Assert.AreEqual(new Bit[] { 0, 1, 0, 0 }, value.GetBits());
        }

        [Test]
        public void Should_SerializeUInt4()
        {
            UInt4 value = UInt4.MaxValue; // range 0 to 15
            Assert.AreEqual(1, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(1, value.GetBit(2));
            Assert.AreEqual(1, value.GetBit(3));
            Assert.AreEqual(new Bit[] { 1, 1, 1, 1 }, value.GetBits());

            value = (UInt4)2;
            Assert.AreEqual(0, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(0, value.GetBit(2));
            Assert.AreEqual(0, value.GetBit(3));
            Assert.AreEqual(new Bit[] { 0, 1, 0, 0 }, value.GetBits());

            // test overflow
            value = (UInt4)20;
            Assert.AreEqual(4, value);
            Assert.AreEqual(0, value.GetBit(0));
            Assert.AreEqual(0, value.GetBit(1));
            Assert.AreEqual(1, value.GetBit(2));
            Assert.AreEqual(0, value.GetBit(3));
            Assert.AreEqual(new Bit[] { 0, 0, 1, 0 }, value.GetBits());
        }

        [Test]
        public void Should_SerializeInt5()
        {
            Int5 value = Int5.MaxValue; // range -15 to 15
            Assert.AreEqual(false, value._sign);
            Assert.AreEqual(1, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(1, value.GetBit(2));
            Assert.AreEqual(1, value.GetBit(3));
            Assert.AreEqual(new Bit[] { 1, 1, 1, 1, 0 }, value.GetBits());

            value = Int5.MinValue;
            Assert.AreEqual(true, value._sign);
            Assert.AreEqual(1, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(1, value.GetBit(2));
            Assert.AreEqual(1, value.GetBit(3));
            Assert.AreEqual(new Bit[] { 1, 1, 1, 1, 1 }, value.GetBits());

            // test overflow
            value = (Int5)20;
            Assert.AreEqual(4, value);
            Assert.AreEqual(false, value._sign);
            Assert.AreEqual(0, value.GetBit(0));
            Assert.AreEqual(0, value.GetBit(1));
            Assert.AreEqual(1, value.GetBit(2));
            Assert.AreEqual(0, value.GetBit(3));
            Assert.AreEqual(new Bit[] { 0, 0, 1, 0, 0 }, value.GetBits());
        }

        [Test]
        public void Should_SerializeUInt5()
        {
            UInt5 value = UInt5.MaxValue; // range 0 to 31
            Assert.AreEqual(1, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(1, value.GetBit(2));
            Assert.AreEqual(1, value.GetBit(3));
            Assert.AreEqual(1, value.GetBit(4));
            Assert.AreEqual(new Bit[] { 1, 1, 1, 1, 1 }, value.GetBits());

            value = (UInt5)2;
            Assert.AreEqual(0, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(0, value.GetBit(2));
            Assert.AreEqual(0, value.GetBit(3));
            Assert.AreEqual(0, value.GetBit(4));
            Assert.AreEqual(new Bit[] { 0, 1, 0, 0, 0 }, value.GetBits());

            // test overflow
            value = (UInt5)36;
            Assert.AreEqual(4, value);
            Assert.AreEqual(0, value.GetBit(0));
            Assert.AreEqual(0, value.GetBit(1));
            Assert.AreEqual(1, value.GetBit(2));
            Assert.AreEqual(0, value.GetBit(3));
            Assert.AreEqual(0, value.GetBit(4));
            Assert.AreEqual(new Bit[] { 0, 0, 1, 0, 0 }, value.GetBits());
        }

        [Test]
        public void Should_SerializeInt6()
        {
            Int6 value = Int6.MaxValue; // range -31 to 31
            Assert.AreEqual(false, value._sign);
            Assert.AreEqual(1, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(1, value.GetBit(2));
            Assert.AreEqual(1, value.GetBit(3));
            Assert.AreEqual(1, value.GetBit(4));
            Assert.AreEqual(new Bit[] { 1, 1, 1, 1, 1, 0 }, value.GetBits());

            value = Int6.MinValue;
            Assert.AreEqual(true, value._sign);
            Assert.AreEqual(1, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(1, value.GetBit(2));
            Assert.AreEqual(1, value.GetBit(3));
            Assert.AreEqual(1, value.GetBit(4));
            Assert.AreEqual(new Bit[] { 1, 1, 1, 1, 1, 1 }, value.GetBits());

            // test overflow
            value = (Int6)40;
            Assert.AreEqual(8, value);
            Assert.AreEqual(false, value._sign);
            Assert.AreEqual(0, value.GetBit(0));
            Assert.AreEqual(0, value.GetBit(1));
            Assert.AreEqual(0, value.GetBit(2));
            Assert.AreEqual(1, value.GetBit(3));
            Assert.AreEqual(0, value.GetBit(4));
            Assert.AreEqual(new Bit[] { 0, 0, 0, 1, 0, 0 }, value.GetBits());
        }

        [Test]
        public void Should_SerializeUInt6()
        {
            UInt6 value = UInt6.MaxValue; // range 0 to 63
            Assert.AreEqual(1, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(1, value.GetBit(2));
            Assert.AreEqual(1, value.GetBit(3));
            Assert.AreEqual(1, value.GetBit(4));
            Assert.AreEqual(1, value.GetBit(5));
            Assert.AreEqual(new Bit[] { 1, 1, 1, 1, 1, 1 }, value.GetBits());

            value = (UInt6)2;
            Assert.AreEqual(0, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(0, value.GetBit(2));
            Assert.AreEqual(0, value.GetBit(3));
            Assert.AreEqual(0, value.GetBit(4));
            Assert.AreEqual(0, value.GetBit(5));
            Assert.AreEqual(new Bit[] { 0, 1, 0, 0, 0, 0 }, value.GetBits());

            // test overflow
            value = (UInt6)76;
            Assert.AreEqual(12, value);
            Assert.AreEqual(0, value.GetBit(0));
            Assert.AreEqual(0, value.GetBit(1));
            Assert.AreEqual(1, value.GetBit(2));
            Assert.AreEqual(1, value.GetBit(3));
            Assert.AreEqual(0, value.GetBit(4));
            Assert.AreEqual(0, value.GetBit(5));
            Assert.AreEqual(new Bit[] { 0, 0, 1, 1, 0, 0 }, value.GetBits());
        }

        [Test]
        public void Should_SerializeInt7()
        {
            Int7 value = Int7.MaxValue; // range -63 to 63
            Assert.AreEqual(false, value._sign);
            Assert.AreEqual(1, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(1, value.GetBit(2));
            Assert.AreEqual(1, value.GetBit(3));
            Assert.AreEqual(1, value.GetBit(4));
            Assert.AreEqual(1, value.GetBit(5));
            Assert.AreEqual(new Bit[] { 1, 1, 1, 1, 1, 1, 0 }, value.GetBits());

            value = Int7.MinValue;
            Assert.AreEqual(true, value._sign);
            Assert.AreEqual(1, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(1, value.GetBit(2));
            Assert.AreEqual(1, value.GetBit(3));
            Assert.AreEqual(1, value.GetBit(4));
            Assert.AreEqual(1, value.GetBit(5));
            Assert.AreEqual(new Bit[] { 1, 1, 1, 1, 1, 1, 1 }, value.GetBits());

            // test overflow
            value = (Int7)80;
            Assert.AreEqual(16, value);
            Assert.AreEqual(false, value._sign);
            Assert.AreEqual(0, value.GetBit(0));
            Assert.AreEqual(0, value.GetBit(1));
            Assert.AreEqual(0, value.GetBit(2));
            Assert.AreEqual(0, value.GetBit(3));
            Assert.AreEqual(1, value.GetBit(4));
            Assert.AreEqual(0, value.GetBit(5));
            Assert.AreEqual(new Bit[] { 0, 0, 0, 0, 1, 0, 0 }, value.GetBits());
        }

        [Test]
        public void Should_SerializeUInt7()
        {
            UInt7 value = UInt7.MaxValue; // range 0 to 127
            Assert.AreEqual(1, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(1, value.GetBit(2));
            Assert.AreEqual(1, value.GetBit(3));
            Assert.AreEqual(1, value.GetBit(4));
            Assert.AreEqual(1, value.GetBit(5));
            Assert.AreEqual(1, value.GetBit(6));
            Assert.AreEqual(new Bit[] { 1, 1, 1, 1, 1, 1, 1 }, value.GetBits());

            value = (UInt7)2;
            Assert.AreEqual(0, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(0, value.GetBit(2));
            Assert.AreEqual(0, value.GetBit(3));
            Assert.AreEqual(0, value.GetBit(4));
            Assert.AreEqual(0, value.GetBit(5));
            Assert.AreEqual(0, value.GetBit(6));
            Assert.AreEqual(new Bit[] { 0, 1, 0, 0, 0, 0, 0 }, value.GetBits());

            // test overflow
            value = (UInt7)136;
            Assert.AreEqual(8, value);
            Assert.AreEqual(0, value.GetBit(0));
            Assert.AreEqual(0, value.GetBit(1));
            Assert.AreEqual(0, value.GetBit(2));
            Assert.AreEqual(1, value.GetBit(3));
            Assert.AreEqual(0, value.GetBit(4));
            Assert.AreEqual(0, value.GetBit(5));
            Assert.AreEqual(0, value.GetBit(6));
            Assert.AreEqual(new Bit[] { 0, 0, 0, 1, 0, 0, 0 }, value.GetBits());
        }

        [Test]
        public void Should_SerializeInt10()
        {
            Int10 value = Int10.MaxValue; // range -511 to 511
            Assert.AreEqual(false, value._sign);
            Assert.AreEqual(1, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(1, value.GetBit(2));
            Assert.AreEqual(1, value.GetBit(3));
            Assert.AreEqual(1, value.GetBit(4));
            Assert.AreEqual(1, value.GetBit(5));
            Assert.AreEqual(1, value.GetBit(6));
            Assert.AreEqual(1, value.GetBit(7));
            Assert.AreEqual(1, value.GetBit(8));
            Assert.AreEqual(new Bit[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 }, value.GetBits());

            value = Int10.MinValue;
            Assert.AreEqual(true, value._sign);
            Assert.AreEqual(1, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(1, value.GetBit(2));
            Assert.AreEqual(1, value.GetBit(3));
            Assert.AreEqual(1, value.GetBit(4));
            Assert.AreEqual(1, value.GetBit(5));
            Assert.AreEqual(1, value.GetBit(6));
            Assert.AreEqual(1, value.GetBit(7));
            Assert.AreEqual(1, value.GetBit(8));
            Assert.AreEqual(new Bit[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, value.GetBits());

            // test overflow
            value = (Int10)700;
            Assert.AreEqual(188, value);
            Assert.AreEqual(false, value._sign);
            Assert.AreEqual(0, value.GetBit(0));
            Assert.AreEqual(0, value.GetBit(1));
            Assert.AreEqual(1, value.GetBit(2));
            Assert.AreEqual(1, value.GetBit(3));
            Assert.AreEqual(1, value.GetBit(4));
            Assert.AreEqual(1, value.GetBit(5));
            Assert.AreEqual(0, value.GetBit(6));
            Assert.AreEqual(1, value.GetBit(7));
            Assert.AreEqual(0, value.GetBit(8));
            Assert.AreEqual(new Bit[] { 0, 0, 1, 1, 1, 1, 0, 1, 0, 0 }, value.GetBits());
        }

        [Test]
        public void Should_SerializeUInt10()
        {
            UInt10 value = UInt10.MaxValue; // range 0 to 1023
            Assert.AreEqual(1, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(1, value.GetBit(2));
            Assert.AreEqual(1, value.GetBit(3));
            Assert.AreEqual(1, value.GetBit(4));
            Assert.AreEqual(1, value.GetBit(5));
            Assert.AreEqual(1, value.GetBit(6));
            Assert.AreEqual(1, value.GetBit(7));
            Assert.AreEqual(1, value.GetBit(8));
            Assert.AreEqual(1, value.GetBit(9));
            Assert.AreEqual(new Bit[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, value.GetBits());

            value = (UInt10)2;
            Assert.AreEqual(0, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(0, value.GetBit(2));
            Assert.AreEqual(0, value.GetBit(3));
            Assert.AreEqual(0, value.GetBit(4));
            Assert.AreEqual(0, value.GetBit(5));
            Assert.AreEqual(0, value.GetBit(6));
            Assert.AreEqual(0, value.GetBit(7));
            Assert.AreEqual(0, value.GetBit(8));
            Assert.AreEqual(0, value.GetBit(9));
            Assert.AreEqual(new Bit[] { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 }, value.GetBits());

            // test overflow
            value = (UInt10)1900;
            Assert.AreEqual(876, value);
            Assert.AreEqual(0, value.GetBit(0));
            Assert.AreEqual(0, value.GetBit(1));
            Assert.AreEqual(1, value.GetBit(2));
            Assert.AreEqual(1, value.GetBit(3));
            Assert.AreEqual(0, value.GetBit(4));
            Assert.AreEqual(1, value.GetBit(5));
            Assert.AreEqual(1, value.GetBit(6));
            Assert.AreEqual(0, value.GetBit(7));
            Assert.AreEqual(1, value.GetBit(8));
            Assert.AreEqual(1, value.GetBit(9));
            Assert.AreEqual(new Bit[] { 0, 0, 1, 1, 0, 1, 1, 0, 1, 1 }, value.GetBits());
        }

        [Test]
        public void Should_SerializeInt12()
        {
            Int12 value = Int12.MaxValue; // range -2047 to 2047
            Assert.AreEqual(false, value._sign);
            Assert.AreEqual(1, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(1, value.GetBit(2));
            Assert.AreEqual(1, value.GetBit(3));
            Assert.AreEqual(1, value.GetBit(4));
            Assert.AreEqual(1, value.GetBit(5));
            Assert.AreEqual(1, value.GetBit(6));
            Assert.AreEqual(1, value.GetBit(7));
            Assert.AreEqual(1, value.GetBit(8));
            Assert.AreEqual(1, value.GetBit(9));
            Assert.AreEqual(1, value.GetBit(10));
            Assert.AreEqual(new Bit[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 }, value.GetBits());

            value = Int12.MinValue;
            Assert.AreEqual(true, value._sign);
            Assert.AreEqual(1, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(1, value.GetBit(2));
            Assert.AreEqual(1, value.GetBit(3));
            Assert.AreEqual(1, value.GetBit(4));
            Assert.AreEqual(1, value.GetBit(5));
            Assert.AreEqual(1, value.GetBit(6));
            Assert.AreEqual(1, value.GetBit(7));
            Assert.AreEqual(1, value.GetBit(8));
            Assert.AreEqual(1, value.GetBit(9));
            Assert.AreEqual(1, value.GetBit(10));
            Assert.AreEqual(new Bit[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, value.GetBits());

            // test overflow
            value = (Int12)3200;
            Assert.AreEqual(1152, value);
            Assert.AreEqual(false, value._sign);
            Assert.AreEqual(0, value.GetBit(0));
            Assert.AreEqual(0, value.GetBit(1));
            Assert.AreEqual(0, value.GetBit(2));
            Assert.AreEqual(0, value.GetBit(3));
            Assert.AreEqual(0, value.GetBit(4));
            Assert.AreEqual(0, value.GetBit(5));
            Assert.AreEqual(0, value.GetBit(6));
            Assert.AreEqual(1, value.GetBit(7));
            Assert.AreEqual(0, value.GetBit(8));
            Assert.AreEqual(0, value.GetBit(9));
            Assert.AreEqual(1, value.GetBit(10));
            Assert.AreEqual(new Bit[] { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 1, 0 }, value.GetBits());
        }

        [Test]
        public void Should_SerializeUInt12()
        {
            UInt12 value = UInt12.MaxValue; // range 0 to 4095
            Assert.AreEqual(1, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(1, value.GetBit(2));
            Assert.AreEqual(1, value.GetBit(3));
            Assert.AreEqual(1, value.GetBit(4));
            Assert.AreEqual(1, value.GetBit(5));
            Assert.AreEqual(1, value.GetBit(6));
            Assert.AreEqual(1, value.GetBit(7));
            Assert.AreEqual(1, value.GetBit(8));
            Assert.AreEqual(1, value.GetBit(9));
            Assert.AreEqual(1, value.GetBit(10));
            Assert.AreEqual(1, value.GetBit(11));
            Assert.AreEqual(new Bit[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, value.GetBits());

            value = (UInt12)2;
            Assert.AreEqual(0, value.GetBit(0));
            Assert.AreEqual(1, value.GetBit(1));
            Assert.AreEqual(0, value.GetBit(2));
            Assert.AreEqual(0, value.GetBit(3));
            Assert.AreEqual(0, value.GetBit(4));
            Assert.AreEqual(0, value.GetBit(5));
            Assert.AreEqual(0, value.GetBit(6));
            Assert.AreEqual(0, value.GetBit(7));
            Assert.AreEqual(0, value.GetBit(8));
            Assert.AreEqual(0, value.GetBit(9));
            Assert.AreEqual(0, value.GetBit(10));
            Assert.AreEqual(0, value.GetBit(11));
            Assert.AreEqual(new Bit[] { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, value.GetBits());

            // test overflow
            value = (UInt12)6000;
            Assert.AreEqual(1904, value);
            Assert.AreEqual(0, value.GetBit(0));
            Assert.AreEqual(0, value.GetBit(1));
            Assert.AreEqual(0, value.GetBit(2));
            Assert.AreEqual(0, value.GetBit(3));
            Assert.AreEqual(1, value.GetBit(4));
            Assert.AreEqual(1, value.GetBit(5));
            Assert.AreEqual(1, value.GetBit(6));
            Assert.AreEqual(0, value.GetBit(7));
            Assert.AreEqual(1, value.GetBit(8));
            Assert.AreEqual(1, value.GetBit(9));
            Assert.AreEqual(1, value.GetBit(10));
            Assert.AreEqual(0, value.GetBit(11));
            Assert.AreEqual(new Bit[] { 0, 0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 0 }, value.GetBits());
        }

        [Test]
        public void Should_SerializeInt24()
        {
            Int24 value = Int24.MaxValue; // range -8,388,607 to 8,388,607
            Assert.AreEqual(false, value._sign);
            Assert.AreEqual(new byte[] { 255, 255, 127 }, value.GetBytes());
            Assert.AreEqual(new Bit[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 }, value.GetBits());

            value = Int24.MinValue;
            Assert.AreEqual(true, value._sign);
            Assert.AreEqual(new byte[] { 255, 255, 255 }, value.GetBytes());
            Assert.AreEqual(new Bit[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, value.GetBits());

            // test overflow
            value = (Int24)9000000;
            Assert.AreEqual(611392, value);
            Assert.AreEqual(false, value._sign);
            Assert.AreEqual(new byte[] { 64, 84, 9 }, value.GetBytes());
            Assert.AreEqual(new Bit[] { 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0, 0 }, value.GetBits());
        }

        [Test]
        public void Should_SerializeUInt24()
        {
            UInt24 value = UInt24.MaxValue; // range 0 to 16,777,215
            Assert.AreEqual(new byte[] { 255, 255, 255 }, value.GetBytes());
            Assert.AreEqual(new Bit[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, value.GetBits());

            value = (UInt24)2;
            Assert.AreEqual(new byte[] { 2, 0, 0 }, value.GetBytes());
            Assert.AreEqual(new Bit[] { 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, value.GetBits());

            // test overflow
            value = (UInt24)18000000;
            Assert.AreEqual(1222784, value);
            Assert.AreEqual(new byte[] { 128, 168, 18 }, value.GetBytes());
            Assert.AreEqual(new Bit[] { 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 1, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0 }, value.GetBits());
        }

        [Test]
        public void Should_SerializeInt48()
        {
            Int48 value = Int48.MaxValue; // range -140,737,488,355,327 to 140,737,488,355,327
            Assert.AreEqual(false, value._sign);
            Assert.AreEqual(new byte[] { 255, 255, 255, 255, 255, 127 }, value.GetBytes());
            Assert.AreEqual(new Bit[] { 
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0
            }, value.GetBits());

            value = Int48.MinValue;
            Assert.AreEqual(true, value._sign);
            Assert.AreEqual(new byte[] { 255, 255, 255, 255, 255, 255 }, value.GetBytes());
            Assert.AreEqual(new Bit[] { 
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
            }, value.GetBits());

            // test overflow
            value = (Int48)140737488556378L;
            Assert.AreEqual(201050, value);
            Assert.AreEqual(false, value._sign);
            Assert.AreEqual(new byte[] { 90, 17, 3, 0, 0, 0 }, value.GetBytes());
            Assert.AreEqual(new Bit[] { 
                0, 1, 0, 1, 1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            }, value.GetBits());
        }

        [Test]
        public void Should_SerializeUInt48()
        {
            UInt48 value = UInt48.MaxValue; // range 0 to 281,474,976,710,655
            Assert.AreEqual(new byte[] { 255, 255, 255, 255, 255, 255 }, value.GetBytes());
            Assert.AreEqual(new Bit[] { 
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1
            }, value.GetBits());

            value = (UInt48)2;
            Assert.AreEqual(new byte[] { 2, 0, 0, 0, 0, 0 }, value.GetBytes());
            Assert.AreEqual(new Bit[] { 
                0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            }, value.GetBits());

            // test overflow
            value = (UInt48)281474981710655L;
            Assert.AreEqual(4999999, value);
            Assert.AreEqual(new byte[] { 63, 75, 76, 0, 0, 0 }, value.GetBytes());
            Assert.AreEqual(new Bit[] { 
                1, 1, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1, 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 1, 0,
                0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            }, value.GetBits());
        }
    }
}