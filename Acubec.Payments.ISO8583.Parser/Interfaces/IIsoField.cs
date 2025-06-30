using Acubec.Payments.ISO8583.Parser.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583.Parser.Implementation;

public interface IIsoField
{
    #region Public Properties

    bool IsSet { get; }

    int Length { get; }

    string Name { get; }

    IsoFieldType Type { get; }

    string Value { get; set; }

    int MessageIndex { get; }

    public bool Mask { get; set; }
    ReadOnlySpan<byte> ValueSpans { get; }

    #endregion Public Properties

    #region Methods

    ReadOnlySpan<byte> GetValueBytes();

    int SetValueBytes(ReadOnlySpan<byte> dataByte, int offset);

    //int SetValueBytes(byte[] dataByte, int offset) => SetValueBytes(dataByte.AsSpan(), offset);

    string LogDump();

    string ToString();

    #endregion Methods
}