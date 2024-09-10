namespace Acubec.Payments.ISO8583Parser.Interfaces;

public interface IMTIParser
{
    public string ParseMTI(byte[] isoMessage);

    public byte[] WriteMTI(string mti);
}
