using Acubec.Payments.ISO8583.Parser.Implementation;
using NUnit.Framework.Legacy;
using NUnit.Framework;

namespace Acubec.Payments.ISO8583.Parser.Test
{
    [TestFixture]
    public class ByteHelperTests
    {
        [Test]
        public void Combine_ThreeSpans_ReturnsConcatenatedBytes()
        {
            var a = new byte[] { 1, 2 };
            var b = new byte[] { 3, 4 };
            var c = new byte[] { 5, 6 };
            var result = ByteHelper.Combine(a, b, c).ToArray();
            Assert.That(result, Is.EquivalentTo(new byte[] { 1, 2, 3, 4, 5, 6 }));
        }

        [Test]
        public void Combine_TwoSpans_ReturnsConcatenatedBytes()
        {
            var a = new byte[] { 1, 2 };
            var b = new byte[] { 3, 4 };
            var result = ByteHelper.Combine(a, b).ToArray();
            Assert.That(result, Is.EquivalentTo(new byte[] { 1, 2, 3, 4 }));
        }

        [Test]
        public void GetByteSlice_Valid_ReturnsSlice()
        {
            var data = new byte[] { 10, 20, 30, 40 };
            var slice = ByteHelper.GetByteSlice(data, 2, 1).ToArray();
            Assert.That(slice, Is.EquivalentTo(new byte[] { 20, 30 }));
        }

        [Test]
        public void GetByteSlice_Invalid_Throws()
        {
            var data = new byte[] { 1, 2, 3 };
            Assert.Throws<ArgumentOutOfRangeException>(() => ByteHelper.GetByteSlice(data, 5, 0));
        }

        [Test]
        public void ByteSpanToHexString_ConvertsCorrectly()
        {
            var data = new byte[] { 0x0A, 0xFF };
            var hex = ByteHelper.ByteSpanToHexString(data);
            Assert.That(hex, Is.EqualTo("0AFF"));
        }

        [Test]
        public void HexDump_ReturnsStringBuilder()
        {
            var data = new byte[] { 0x41, 0x42, 0x43 };
            var sb = ByteHelper.HexDump(data, 2);
            Assert.That(sb.ToString().Contains("41 42"), Is.True);
        }

        [Test]
        public void ToReadOnlySpan_ReturnsReadOnlySpan()
        {
            Span<int> span = stackalloc int[] { 1, 2 };
            var ro = ByteHelper.ToReadOnlySpan(span);
            Assert.That(ro.Length, Is.EqualTo(2));
            Assert.That(ro[0], Is.EqualTo(1));
        }

        [Test]
        public void GetByteSlice_Span_Valid_ReturnsSlice()
        {
            Span<byte> data = stackalloc byte[] { 1, 2, 3, 4 };
            var slice = ByteHelper.GetByteSlice(data, 2, 1).ToArray();
            Assert.That(slice, Is.EquivalentTo(new byte[] { 2, 3 }));
        }

        [Test]
        public void GetByteSlice_Span_Invalid_Throws()
        {
            void Act()
            {
                Span<byte> data = stackalloc byte[] { 1, 2 };
                ByteHelper.GetByteSlice(data, 3, 0);
            }
            Assert.Throws<ArgumentOutOfRangeException>(Act);
        }

        [Test]
        public void ConvertHexToBinaryUsingConvert_ValidHex_ReturnsBytes()
        {
            var hex = "0A1B".AsSpan();
            var bytes = ByteHelper.convertHexToBinaryUsingConvert(hex).ToArray();
            Assert.That(bytes, Is.EquivalentTo(new byte[] { 0x0A, 0x1B }));
        }

        [Test]
        public void ConvertHexToBinaryUsingConvert_InvalidHex_Throws()
        {
            void Act()
            {
                var hex = "ABC".AsSpan();
                ByteHelper.convertHexToBinaryUsingConvert(hex);
            }
            Assert.Throws<ArgumentException>(Act);
        }
    }
}
