using Acubec.Payments.ISO8583Parser.Helpers;
using Acubec.Payments.ISO8583Parser.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace Acubec.Payments.ISO8583Parser.Test;

public class EbcdicEncoderFormatorTests : IClassFixture<ISO8583ParserTestFixture>
{
    private readonly IEncoderFormator _asciiEncoderFormator;
    readonly IServiceProvider _serviceProvider;
    public EbcdicEncoderFormatorTests(ISO8583ParserTestFixture fixture)
    {

        _serviceProvider = fixture.ServiceProvider;
        _asciiEncoderFormator = _serviceProvider.GetKeyedService<IEncoderFormator>(DataEncoding.EBCDIC.ToString());
    }

    [Fact]
    public void Encode_ShouldReturnCorrectString()
    {
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        var encoder = Encoding.GetEncoding("IBM037");
        byte[] input = ByteHelper.HexStringToByteArray("C1C2C3C4C5C6C7C8C9D1D2D3D4D5D6D7D8D9E2E3E4E5E6E7E8E9");

        var t = Encoding.Convert(encoder, Encoding.ASCII, input);
        string expected = "ABC...XYZ123...789";

        var qq = encoder.GetString(t);

        string actual = _asciiEncoderFormator.Encode(t);

        Encoding ascii = Encoding.ASCII;
        Encoding ebcdic = Encoding.GetEncoding("IBM037");

        var qq1 = Encoding.Convert(ascii, ebcdic, Encoding.ASCII.GetBytes(expected));



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
