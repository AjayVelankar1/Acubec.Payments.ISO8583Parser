using Acubec.Payments.ISO8583Parser.Helpers;
using Acubec.Payments.ISO8583Parser.Interfaces;
using System.Text;

namespace Acubec.Payments.ISO8583Parser.DataTypes;

internal sealed class ASCIIMTIParser : IMTIParser
{
    public int SkipBytes => 4;

    public string ParseMTI(Span<byte> isoMessage)
    {
        if (isoMessage == null || isoMessage.Length < 4)
            throw new ArgumentException("Invalid ISO Message");

        var mti = Encoding.ASCII.GetString(isoMessage.ToArray(), 0, 4);
        return mti;
    }

    public Span<byte> WriteMTI(string mti)
    {
        Span<byte> mtiBytes = stackalloc byte[4];
        Encoding.ASCII.TryGetBytes(mti, mtiBytes, out var output);
        return mtiBytes.ToArray();
    }
}

internal sealed class VisaMTIParser : IMTIParser
{
    int _length;
    string _headerFlagAndFormat;
    string _textFormat;
    string _totalMessageLength;
    string _designatedStationId;
    string _sourceStationId;
    private string _roundTripControlInformation;
    private string _baseIFlag;
    private string _messageStatusFlag;
    private string _batchNumber;
    private string _userInformation;
    private string _mti;

    public int SkipBytes => _length + 2 + 8;

    public string ParseMTI(Span<byte> isoMessage)
    {
        if (isoMessage == null || isoMessage.Length < 4)
            throw new ArgumentException("Invalid ISO Message");

        var lengthBytes = isoMessage[2..3];
        _length = ByteHelper.ByteSpanToHexString(lengthBytes).TryParse<int>(0);

        lengthBytes = isoMessage[3..4];
        _headerFlagAndFormat = ByteHelper.ByteSpanToHexString(lengthBytes);

        lengthBytes = isoMessage[4..5];
        _textFormat = ByteHelper.ByteSpanToHexString(lengthBytes);

        lengthBytes = isoMessage[5..7];
        _totalMessageLength = ByteHelper.ByteSpanToHexString(lengthBytes);

        lengthBytes = isoMessage[7..10];
        _designatedStationId = ByteHelper.ByteSpanToHexString(lengthBytes);

        lengthBytes = isoMessage[10..13];
        _sourceStationId = ByteHelper.ByteSpanToHexString(lengthBytes);

        lengthBytes = isoMessage[13..14];
        _roundTripControlInformation = ByteHelper.ByteSpanToHexString(lengthBytes);

        lengthBytes = isoMessage[14..17];
        _messageStatusFlag = ByteHelper.ByteSpanToHexString(lengthBytes);

        lengthBytes = isoMessage[17..18];
        _userInformation = ByteHelper.ByteSpanToHexString(lengthBytes);

        lengthBytes = isoMessage[24..26];
        _mti = ByteHelper.ByteSpanToHexString(lengthBytes);

        
       
        
        return _mti;
    }

    public Span<byte> WriteMTI(string mti)
    {
        Span<byte> mtiBytes = stackalloc byte[4];
        Encoding.ASCII.TryGetBytes(mti, mtiBytes, out var output);
        return mtiBytes.ToArray();
    }
}


internal sealed class BinaryMTIParser : IMTIParser
{
    public int SkipBytes => 4;

    public string ParseMTI(Span<byte> isoMessage)
    {
        if (isoMessage == null || isoMessage.Length < 4)
            throw new ArgumentException("Invalid ISO Message");

        var span = new Span<byte>(isoMessage.ToArray());
        var mti = ByteHelper.GetHexRepresentation(span.Slice(0, 2).ToArray());
        return mti;
    }

    public Span<byte> WriteMTI(string mti)
    {
        Span<byte> mtiBytes = stackalloc byte[4];
        Encoding.ASCII.TryGetBytes(mti, mtiBytes, out var output);
        return mtiBytes.ToArray();
    }
}