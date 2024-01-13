using Acubec.Payments.ISO8583Parser.Interfaces;
using Acubec.Payments.ISO8583Parser.Test;
using Microsoft.Extensions.DependencyInjection;


namespace Acubec.Payments.ISO8583Parser.DataTypes.UnitTests
{

    public class ASCIIMTIParserTests: IClassFixture<ISO8583ParserTestFixture>
    {
        private IMTIParser _parser;
        readonly IServiceProvider _serviceProvider;

        public ASCIIMTIParserTests(ISO8583ParserTestFixture fixture)
        {

            _serviceProvider = fixture.ServiceProvider;
            _parser = _serviceProvider.GetService<IMTIParser>();
        }

        [Fact]
        public void ParseMTI_ValidISOMessage_ReturnsMTI()
        {
            // Arrange
            byte[] isoMessage = new byte[] { 0x30, 0x31, 0x32, 0x33 };

            // Act
            string mti = _parser.ParseMTI(isoMessage);

            // Assert
            Assert.Equal("0123", mti);
        }

        [Fact]
        public void ParseMTI_NullISOMessage_ThrowsArgumentException()
        {
            // Arrange
            byte[] isoMessage = null;

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _parser.ParseMTI(isoMessage));
        }

        [Fact]
        public void ParseMTI_InvalidISOMessageLength_ThrowsArgumentException()
        {
            // Arrange
            byte[] isoMessage = new byte[] { 0x30, 0x31, 0x32 };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _parser.ParseMTI(isoMessage));
        }

        [Fact]
        public void WriteMTI_ValidMTI_ReturnsMTIBytes()
        {
            // Arrange
            string mti = "0123";

            // Act
            byte[] mtiBytes = _parser.WriteMTI(mti);

            // Assert
            Assert.Equal(new byte[] { 0x30, 0x31, 0x32, 0x33 }, mtiBytes);
        }
    }
}
