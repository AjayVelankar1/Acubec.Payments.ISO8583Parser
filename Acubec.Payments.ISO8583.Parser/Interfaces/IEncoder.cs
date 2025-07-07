using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583.Parser.Interfaces;

public interface IEncoder
{
    ReadOnlySpan<char> Encode(ReadOnlySpan<byte> value);
    ReadOnlySpan<byte> Decode(ReadOnlySpan<char> value);

}
