using System;
using NUnit.Framework;
using Acubec.Payments.ISO8583.Parser.Implementation;

namespace Acubec.Payments.ISO8583.Parser.Test
{
    [TestFixture]
    public class EncodersTests: GlobalTestSetup
    {
        [Test]
        public void AsciiEncoder_EncodeDecode_RoundTrip()
        {
            var encoder = new AsciiEncoder();
            var input = "ABC".AsSpan();
            var bytes = encoder.Decode(input);
            var chars = encoder.Encode(bytes);
            Assert.That(chars.ToString(), Is.EqualTo("ABC"));
        }

        [Test]
        public void UTF8Encoder_EncodeDecode_RoundTrip()
        {
            var encoder = new UTF8Encoder();
            var input = "Hello".AsSpan();
            var bytes = encoder.Decode(input);
            var chars = encoder.Encode(bytes);
            Assert.That(chars.ToString(), Is.EqualTo("Hello"));
        }

        [Test]
        public void UTF32Encoder_EncodeDecode_RoundTrip()
        {
            var encoder = new UTF32Encoder();
            var input = "Test".AsSpan();
            var bytes = encoder.Decode(input);
            var chars = encoder.Encode(bytes);
            Assert.That(chars.ToString(), Is.EqualTo("Test"));
        }

        [Test]
        public void BinaryEncoder_EncodeDecode_RoundTrip()
        {
            var encoder = new BinaryEncoder();
            var input = "0A1B".AsSpan();
            var bytes = encoder.Decode(input);
            var chars = encoder.Encode(bytes);
            Assert.That(chars.ToString(), Is.EqualTo("0A1B"));
        }

        [Test]
        public void HexEncoder_EncodeDecode_RoundTrip()
        {
            var encoder = new HexEncoder();
            var input = "0A1B".AsSpan();
            var bytes = encoder.Decode(input);
            var chars = encoder.Encode(bytes);
            Assert.That(chars.ToString(), Is.EqualTo("0A1B"));
        }

        [Test]
        public void HexEncoder_Decode_InvalidLength_Throws()
        {
            var encoder = new HexEncoder();
            Assert.Throws<ArgumentException>(() => encoder.Decode("123".AsSpan()));
        }

        [Test]
        public void EBCDICFEncoder_EncodeDecode_RoundTrip()
        {
            var encoder = new EBCDICFEncoder();
            var input = "HELLO".AsSpan();
            var bytes = encoder.Decode(input);
            var chars = encoder.Encode(bytes);
            Assert.That(chars.ToString().Trim(), Is.EqualTo("HELLO"));
        }

        [Test]
        public void BinaryUnPackedEncoder_Decode_ValidDigits_ReturnsExpectedBytes()
        {
            var encoder = new BinaryUnPackedEncoder();
            var input = "1234567890".AsSpan();
            var bytes = encoder.Decode(input).ToArray();
            Assert.That(bytes, Is.EqualTo(new byte[] { 1,2,3,4,5,6,7,8,9,0 }));
        }

        [Test]
        public void BinaryUnPackedEncoder_Decode_InvalidCharacter_Throws()
        {
            var encoder = new BinaryUnPackedEncoder();
            Assert.Throws<ArgumentException>(() => encoder.Decode("12A4".AsSpan()));
        }

        [Test]
        public void BinaryUnPackedEncoder_Decode_Empty_Throws()
        {
            var encoder = new BinaryUnPackedEncoder();
            Assert.Throws<ArgumentException>(() => encoder.Decode(ReadOnlySpan<char>.Empty));
        }

        [Test]
        public void BinaryUnPackedEncoder_Encode_ValidBytes_ReturnsExpectedString()
        {
            var encoder = new BinaryUnPackedEncoder();
            var input = new byte[] { 1,2,3,4,5,6,7,8,9,0 };
            var chars = encoder.Encode(input).ToString();
            Assert.That(chars, Is.EqualTo("1234567890"));
        }

        [Test]
        public void BinaryUnPackedEncoder_Encode_ByteGreaterThan9_Throws()
        {
            var encoder = new BinaryUnPackedEncoder();
            var input = new byte[] { 1, 10, 3 };
            Assert.Throws<ArgumentException>(() => encoder.Encode(input));
        }

        [Test]
        public void BinaryUnPackedEncoder_Encode_Empty_Throws()
        {
            var encoder = new BinaryUnPackedEncoder();
            Assert.Throws<ArgumentNullException>(() => encoder.Encode(ReadOnlySpan<byte>.Empty));
        }

        [Test]
        public void BinaryUnPackedEncoder_RoundTrip_EncodeDecode()
        {
            var encoder = new BinaryUnPackedEncoder();
            var original = "9876543210".AsSpan();
            var bytes = encoder.Decode(original);
            var chars = encoder.Encode(bytes).ToString();
            Assert.That(chars, Is.EqualTo("9876543210"));
        }
    }
}
