using Acubec.Payments.ISO8583Parser.Helpers;
using Acubec.Payments.ISO8583Parser.Interfaces;
using System.Text;

namespace Acubec.Payments.ISO8583Parser.DataTypes;

internal sealed class ASCIIMTIParser : IMTIParser
{
    public string ParseMTI(byte[] isoMessage)
    {
        if (isoMessage == null || isoMessage.Length < 4)
            throw new ArgumentException("Invalid ISO Message");

        var mti = Encoding.ASCII.GetString(isoMessage, 0, 4);
        return mti;
    }

    public byte[] WriteMTI(string mti)
    {
        Span<byte> mtiBytes = stackalloc byte[4];
        Encoding.ASCII.TryGetBytes(mti, mtiBytes, out var output);
        return mtiBytes.ToArray();
    }
}

internal sealed class BinaryMTIParser : IMTIParser
{
    public string ParseMTI(byte[] isoMessage)
    {
        if (isoMessage == null || isoMessage.Length < 4)
            throw new ArgumentException("Invalid ISO Message");

        var span = new Span<byte>(isoMessage);
        var mti = ByteHelper.GetHexRepresentation(span.Slice(0, 2).ToArray());
        return mti;
    }

    public byte[] WriteMTI(string mti)
    {
        Span<byte> mtiBytes = stackalloc byte[4];
        Encoding.ASCII.TryGetBytes(mti, mtiBytes, out var output);
        return mtiBytes.ToArray();
    }
}