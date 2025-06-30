using Acubec.Payments.ISO8583.Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583.Parser.Implementation;

internal sealed class UnpackedByteMapParser : IByteMapParser
{
    public UnpackedByteMapParser()
    {
        
    }

    public ReadOnlySpan<byte> GetBitMap(ByteMap byteMap)
    {
        throw new NotImplementedException();
    }

    public ByteMap SetBitMap(ReadOnlySpan<byte> byteMap, StringBuilder logDump, IsoRequest isoRequest)
    {
        logDump.Append($"Start converting byte array to ISOMessage MTI: {isoRequest.MessageType}{Environment.NewLine}");
        logDump.Append($"Raw byte array is{Environment.NewLine} {ByteHelper.ByteSpanToHexString(byteMap)}{Environment.NewLine}");

        ByteMap map = new ByteMap();
        int i = 0;
        foreach (var item in byteMap)
        {
            var v = byteMap[0] >> 7 & 0x01;
            //map[i] = v == '1';
            i++;
        }
        return map;
    }
}
