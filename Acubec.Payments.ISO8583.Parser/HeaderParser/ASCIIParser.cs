using Acubec.Payments.ISO8583.Parser.Interfaces;
using System.Text;

namespace Acubec.Payments.ISO8583.Parser.HeaderParser;

internal sealed class ASCIIParser : IHeaderParser
{
    //int _headerLength;
    Dictionary<string, string> _otherProperties;
    public int HeaderLength => 4;

    public Dictionary<string, string> OtherProperties => _otherProperties;

    public string ReadMTI(Span<byte> isoMessage)
    {
        return Encoding.ASCII.GetString(isoMessage[0..4]);  
    }

    public Span<byte> WriteMTI(string mti, int totalLength = 0)
    {
        return Encoding.ASCII.GetBytes(mti);
    }
}