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
}
