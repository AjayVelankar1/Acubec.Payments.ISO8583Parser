using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583.Parser.Interfaces;

public interface IHeaderParser
{
    public Span<byte> WriteMTI(string mti, int totalLength = 0);

    public string ReadMTI(Span<byte> isoMessage);

    public int HeaderLength { get; }

    public Dictionary<string,string> OtherProperties { get; }
}
