using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    ASCII = 0,
    Binary = 1,
    BinaryPlus = 2,
    EBCDIC = 3,
    HEX = 4,
    BCDUnPacked = 5,
    BinaryUnPacked = 6,
    HexUnPacked = 7
}


internal static class EnumExtensions
{
    public static bool IsPackedEncoding(this DataEncoding encoding)
    {
        if( encoding == DataEncoding.Binary ||
            encoding == DataEncoding.BinaryPlus ||
            encoding == DataEncoding.HEX  )
        
            return true;

        return false;
    }
}