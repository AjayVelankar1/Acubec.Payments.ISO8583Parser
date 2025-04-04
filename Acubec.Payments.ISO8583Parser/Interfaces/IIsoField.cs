namespace Acubec.Payments.ISO8583Parser.Interfaces;

public enum IsoTypes
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

public interface IIsoField
{
    #region Public Properties

    bool IsSet { get; }

    int Length { get; }

    string Name { get; }

    IsoTypes Type { get; }

    string Value { get; set; }

    int MessageIndex { get; }

    public bool Mask { get; set; }

    #endregion Public Properties

    #region Methods

    byte[] GetValueBytes();

    int SetValueBytes(Span<byte> dataByte, int offset);

    int SetValueBytes(byte[] dataByte, int offset) => SetValueBytes(dataByte.AsSpan(),offset);

    string LogDump();

    string ToString();

    #endregion Methods
}