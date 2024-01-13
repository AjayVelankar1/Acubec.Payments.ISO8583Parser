using Acubec.Payments.ISO8583Parser.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583Parser.Interfaces;

public interface ICustomFiledFactory
{
    BaseIsoField GetField(Field field, ByteMaps byteMap);
}
