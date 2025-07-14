using Acubec.Payments.ISO8583.Parser.HeaderParser;
using Acubec.Payments.ISO8583.Parser.Implementation;
using Acubec.Payments.ISO8583.Parser.Interfaces;
using Acubec.Payments.ISO8583.Parser.Types;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583.Parser;

public static class ISO8583ParserManager
{
    public static IServiceCollection AddISO8583Parser(this IServiceCollection services)
    {
        //services.AddKeyedSingleton<IMTIParser, BinaryMTIParser>("Binary");
        //services.AddKeyedSingleton<IMTIParser, ASCIIMTIParser>("ASCII");
        services.AddKeyedSingleton<IHeaderParser, VisaHeaderParser>("VisaMTIParser");
        services.AddKeyedSingleton<IHeaderParser, ASCIIParser>("ASCIIParser");
        services.AddKeyedSingleton<IEncoder>(DataEncoding.ASCII.ToString(), new AsciiEncoder());
        services.AddKeyedSingleton<IEncoder>(DataEncoding.Binary.ToString(), new BinaryEncoder());
        services.AddKeyedSingleton<IEncoder>(DataEncoding.BinaryPlus.ToString(), new BinaryPlusEncoder());
        services.AddKeyedSingleton<IEncoder>(DataEncoding.EBCDIC.ToString(), new EBCDICFEncoder());
        services.AddKeyedSingleton<IEncoder>(DataEncoding.HEX.ToString(), new HexEncoder());
        services.AddKeyedSingleton<IEncoder>(DataEncoding.HexUnPacked.ToString(), new HexEncoder());
        services.AddKeyedSingleton<IEncoder>(DataEncoding.BCDUnPacked.ToString(), new BinaryUnPackedEncoder());
        services.AddKeyedSingleton<IEncoder>(DataEncoding.BinaryUnPacked.ToString(), new BinaryEncoder());

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        return services;
    }
}