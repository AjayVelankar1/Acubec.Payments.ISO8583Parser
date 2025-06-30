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
        public void BinaryEncoder_Decode_InvalidLength_Throws()
        {
            var encoder = new BinaryEncoder();
            Assert.Throws<ArgumentException>(() => encoder.Decode("ABC".AsSpan()));
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
    }
}
