using Xunit;
using Acubec.Payments.ISO8583Parser.Definitions;
using System.Text;
using Acubec.Payments.ISO8583Parser.Test;
using Acubec.Payments.ISO8583Parser.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Acubec.Payments.ISO8583Parser.Tests
{
    public class AsciiEncoderFormatorTests : IClassFixture<ISO8583ParserTestFixture>
    {
        private readonly IEncoderFormator _asciiEncoderFormator;
        readonly IServiceProvider _serviceProvider;
        public AsciiEncoderFormatorTests(ISO8583ParserTestFixture fixture)
        {

            _serviceProvider = fixture.ServiceProvider;
            _asciiEncoderFormator = _serviceProvider.GetKeyedService<IEncoderFormator>(DataEncoding.ASCII.ToString());
        }

        [Fact]
        public void Encode_ShouldReturnCorrectString()
        {
            byte[] input = Encoding.ASCII.GetBytes("Test");
            string expected = "Test";

            string actual = _asciiEncoderFormator.Encode(input);

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Decode_ShouldReturnCorrectByteArray()
        {
            string input = "Test";
            byte[] expected = Encoding.ASCII.GetBytes(input);

            byte[] actual = _asciiEncoderFormator.Decode(input);

            Assert.Equal(expected, actual);
        }
    }
}
