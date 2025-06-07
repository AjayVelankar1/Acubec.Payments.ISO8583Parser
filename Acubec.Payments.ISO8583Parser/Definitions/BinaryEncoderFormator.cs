using Acubec.Payments.ISO8583Parser.Helpers;
using Acubec.Payments.ISO8583Parser.Interfaces;
using System;
using System.Text;


namespace Acubec.Payments.ISO8583Parser.Definitions;

internal sealed class BinaryEncoderFormator : IEncoderFormator
{
    public string Encode(byte[] value)
    {
        return ByteHelper.ByteArrayToHexString(value);
    }

    public byte[] Decode(string value)
    {
        return Encoding.UTF8.GetBytes(value);
    }

    public string Encode(ReadOnlySpan<byte> value)
    {
        return Encoding.UTF8.GetString(value);
    }

    public Span<byte> DecodeSpan(string value)
    {
        ReadOnlySpan<char> charSpan = value.AsSpan();
        Span<byte> bytes = new Span<byte>();
        Encoding.UTF8.GetBytes(charSpan, bytes);
        return bytes;
    }
}


internal sealed class HexFormator : IEncoderFormator
{
    public string Encode(byte[] value)
    {
        return ByteHelper.GetHexRepresentation(value);
    }

    public byte[] Decode(string value)
    {
        return ByteHelper.GetBytesFromHexString(value);
    }

    public string Encode(ReadOnlySpan<byte> value)
    {
        return ByteHelper.ByteSpanToHexString(value);
    }

    public Span<byte> DecodeSpan(string value)
    {
        var bytes = ByteHelper.GetBytesFromHexString(value);
        return bytes.AsSpan<byte>();
    }
}

