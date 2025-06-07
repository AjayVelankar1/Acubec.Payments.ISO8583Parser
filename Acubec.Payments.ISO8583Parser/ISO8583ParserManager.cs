using Acubec.Payments.ISO8583Parser.DataTypes;
using Acubec.Payments.ISO8583Parser.Definitions;
using Acubec.Payments.ISO8583Parser.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace Acubec.Payments.ISO8583Parser;

public static class ISO8583ParserManager
{
    public static IServiceCollection AddISO8583Parser(this IServiceCollection services)
    {
        services.AddKeyedSingleton<IMTIParser, BinaryMTIParser>("Binary");
        services.AddKeyedSingleton<IMTIParser, ASCIIMTIParser>("ASCII");
        services.AddKeyedSingleton<IMTIParser, VisaMTIParser>("VisaMTIParser");
        services.AddKeyedSingleton<IEncoderFormator>(DataEncoding.ASCII.ToString(), new AsciiEncoderFormator());
        services.AddKeyedSingleton<IEncoderFormator>(DataEncoding.Binary.ToString(), new BinaryEncoderFormator());
        services.AddKeyedSingleton<IEncoderFormator>(DataEncoding.BinaryPlus.ToString(), new BinaryEncoderFormator());
        services.AddKeyedSingleton<IEncoderFormator>(DataEncoding.EBCDIC.ToString(), new EBCDICEncoderFormator());
        services.AddKeyedSingleton<IEncoderFormator>(DataEncoding.HEX.ToString(), new HexFormator());

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        return services;
    }
}
