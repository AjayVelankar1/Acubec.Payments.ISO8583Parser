using Acubec.Payments.ISO8583Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583Parser.Definitions;

internal class EBCDICEncoderFormator : IEncoderFormator
{
    public byte[] Decode(string value)
    {
        Encoding ebcdicEncoding = Encoding.GetEncoding("IBM037");
        byte[] encodedBytes = ebcdicEncoding.GetBytes(value);
        return encodedBytes;
    }

    public string Encode(byte[] value)
    {
        Encoding ebcdicEncoding = Encoding.GetEncoding("IBM037");
        var str = ebcdicEncoding.GetString(value);
        return str;
    }

    public string Encode(ReadOnlySpan<byte> value)
    {
        Encoding ebcdicEncoding = Encoding.GetEncoding("IBM037");
        return ebcdicEncoding.GetString(value);
    }

    public Span<byte> DecodeSpan(string value)
    {
        Encoding ebcdicEncoding = Encoding.GetEncoding("IBM037");
        ReadOnlySpan<char> charSpan = value.AsSpan();
        Span<byte> bytes = new Span<byte>();
        ebcdicEncoding.GetBytes(charSpan, bytes);
        return bytes;
    }
}
