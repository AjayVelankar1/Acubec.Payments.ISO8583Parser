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

    int SetValueBytes(byte[] dataByte, int offset);

    string LogDump();

    string ToString();

    #endregion Methods
}