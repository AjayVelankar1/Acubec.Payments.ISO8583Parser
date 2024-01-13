using Acubec.Payments.ISO8583Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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
}

