namespace Acubec.Payments.ISO8583Parser.Interfaces;

public interface IMTIParser
{
    int SkipBytes { get; }

    public string ParseMTI(Span<byte> isoMessage);

    public Span<Byte> WriteMTI(string mti, int totalLength = 0);
}
