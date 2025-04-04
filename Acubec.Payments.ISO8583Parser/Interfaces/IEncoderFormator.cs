using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583Parser.Interfaces;

public interface IEncoderFormator
{
    string Encode(byte[] value);
    byte[] Decode(string value);

    public string Encode(ReadOnlySpan<byte> value);

    public Span<byte> DecodeSpan(string value);
}
