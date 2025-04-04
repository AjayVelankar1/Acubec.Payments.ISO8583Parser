using Acubec.Payments.ISO8583Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583Parser.Definitions;

internal sealed class AsciiEncoderFormator: IEncoderFormator
{
    public string Encode(byte[] value)
    {
        return Encoding.ASCII.GetString(value);
    }

    public byte[] Decode(string value)
    {
        return Encoding.ASCII.GetBytes(value);
    }

    public string Encode(ReadOnlySpan<byte> value)
    {
        return Encoding.ASCII.GetString(value);
    }

    public Span<byte> DecodeSpan(string value)
    {
        ReadOnlySpan<char> charSpan = value.AsSpan();
        Span<byte> bytes = new Span<byte>();
        Encoding.ASCII.GetBytes(charSpan, bytes);
        return bytes;
    }
}

