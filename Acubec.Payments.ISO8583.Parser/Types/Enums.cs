using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583.Parser.Types;

public enum IsoFieldType
{
    Fixed = 0,
    Binary,
    LVar,
    LLVar,
    LLLVar,
    XLLLVar,
    LLLLVar,
    LLLLLVar
}

public enum DataEncoding
{
    ASCII,
    Binary,
    BinaryPlus,
    EBCDIC,
    HEX,
    CardholderBillingConversionRate,
    PosEntryMode,
    CardAcceptorNameLocation,
    ReplacementAmounts,
}