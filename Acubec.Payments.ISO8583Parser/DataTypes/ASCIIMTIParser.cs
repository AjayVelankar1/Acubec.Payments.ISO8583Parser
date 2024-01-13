using Acubec.Payments.ISO8583Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583Parser.DataTypes;

internal sealed class ASCIIMTIParser:IMTIParser
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
        Encoding.ASCII.TryGetBytes(mti, mtiBytes,out var output);
        return mtiBytes.ToArray();
    }
}
