using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583Parser.Interfaces;

internal interface IMTIParser
{
    public string ParseMTI(byte[] isoMessage);

    public byte[] WriteMTI(string mti);
}
