using Acubec.Payments.ISO8583Parser.Helpers;
using Acubec.Payments.ISO8583Parser.Interfaces;
using System.Text;


namespace Acubec.Payments.ISO8583Parser.Definitions;

internal sealed class BinaryEncoderFormator: IEncoderFormator
{
    public string Encode(byte[] value)
    {
        return Encoding.UTF8.GetString(value);
    }

    public byte[] Decode(string value)
    {
        return Encoding.UTF8.GetBytes(value);
    }
}

