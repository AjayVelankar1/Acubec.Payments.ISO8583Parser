using Acubec.Payments.ISO8583.Parser.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583.Parser.Interfaces;

internal interface IByteMapParser
{
    ReadOnlySpan<byte> GetBitMap(ByteMap byteMap);
    ByteMap SetBitMap(ReadOnlySpan<byte> byteMap,StringBuilder logDump, IsoRequest isoRequest);
}
