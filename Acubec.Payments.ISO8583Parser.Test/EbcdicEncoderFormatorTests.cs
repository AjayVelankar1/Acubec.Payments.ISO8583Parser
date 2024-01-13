using Acubec.Payments.ISO8583Parser.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583Parser.Test;

public class EbcdicEncoderFormatorTests:IClassFixture<ISO8583ParserTestFixture>
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
        byte[] input = encoder.GetBytes("Test");
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
