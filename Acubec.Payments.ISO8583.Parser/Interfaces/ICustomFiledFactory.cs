using Acubec.Payments.ISO8583.Parser.Fields;
using Acubec.Payments.ISO8583.Parser.Implementation;
using Acubec.Payments.ISO8583.Parser.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583.Parser.Interfaces;

internal interface ICustomFiledFactory
{
    BaseIsoField GetField(Field field, ByteMaps byteMap);
}
