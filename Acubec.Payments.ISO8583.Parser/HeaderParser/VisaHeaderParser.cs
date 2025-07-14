using Acubec.Payments.ISO8583.Parser.Implementation;
using Acubec.Payments.ISO8583.Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583.Parser.HeaderParser;

internal sealed class VisaHeaderParser: IHeaderParser
{
    int _headerLength;
    int _messageLength;
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
    Dictionary<string, string> _otherProperties;

    public int HeaderLength => 24;// _headerLength + 1 + 8;

    public VisaHeaderParser()
    {
        _otherProperties = new Dictionary<string, string>();
    }

    public Dictionary<string, string> OtherProperties => _otherProperties;

    public string ReadMTI(Span<byte> isoMessage)
    {
        
        if (isoMessage == null || isoMessage.Length < 4)
            throw new ArgumentException("Invalid ISO Message");

        _messageLength = Convert.ToInt32(ByteHelper.ByteSpanToHexString(isoMessage[0..1]), 10);

        var lengthBytes = isoMessage[1..2];
        _headerLength = ByteHelper.ByteSpanToHexString(lengthBytes).TryParse(0);

        lengthBytes = isoMessage[2..3];
        _headerFlagAndFormat = ByteHelper.ByteSpanToHexString(lengthBytes);

        lengthBytes = isoMessage[3..4];
        _textFormat = ByteHelper.ByteSpanToHexString(lengthBytes);

        lengthBytes = isoMessage[4..6];
        _totalMessageLength = ByteHelper.ByteSpanToHexString(lengthBytes);

        lengthBytes = isoMessage[6..9];
        _designatedStationId = ByteHelper.ByteSpanToHexString(lengthBytes);

        lengthBytes = isoMessage[9..12];
        _sourceStationId = ByteHelper.ByteSpanToHexString(lengthBytes);

        lengthBytes = isoMessage[12..13];
        _roundTripControlInformation = ByteHelper.ByteSpanToHexString(lengthBytes);

        lengthBytes = isoMessage[13..16];
        _messageStatusFlag = ByteHelper.ByteSpanToHexString(lengthBytes);

        lengthBytes = isoMessage[16..17];
        _userInformation = ByteHelper.ByteSpanToHexString(lengthBytes);

        lengthBytes = isoMessage[22..24];
        _mti = ByteHelper.ByteSpanToHexString(lengthBytes);


        return _mti;
    }

    public Span<byte> WriteMTI(string mti, int totalLength = 0)
    {
        Span<byte> headerBytes = stackalloc byte[25];

        // 0-1: Message Length (2 bytes, big-endian)
        headerBytes[0] = byte.Parse("16"); // High byte
        
        // 2: Header Length (1 byte, always 16 for Visa)
        if (_headerLength == 0) _headerLength  = 16;
        headerBytes[1] = Convert.ToByte(_headerLength.ToString(),16);
        

        // 3: Header Flag and Format (1 byte)
        headerBytes[2] = Convert.ToByte(_headerFlagAndFormat ?? "01", 16);

        // 4: Text Format (1 byte)
        headerBytes[3] = Convert.ToByte(_textFormat ?? "02", 16);

        // 5-6: Total Message Length (2 bytes)

        headerBytes[4] = (byte)((totalLength >> 8) & 0xFF); // High byte
        headerBytes[5] = (byte)(totalLength & 0xFF);        // Low byte

        // 7-9: Designated Station Id (3 bytes)
        string designatedStationId = _designatedStationId ?? "000000";
        headerBytes[6] = Convert.ToByte(designatedStationId[0..2], 16);
        headerBytes[7] = Convert.ToByte(designatedStationId[2..4], 16);
        headerBytes[8] = Convert.ToByte(designatedStationId[4..6], 16);

        // 10-12: Source Station Id (3 bytes)
        string sourceStationId = _sourceStationId ?? "000000";
        headerBytes[9] = Convert.ToByte(sourceStationId[0..2], 16);
        headerBytes[10] = Convert.ToByte(sourceStationId[2..4], 16);
        headerBytes[11] = Convert.ToByte(sourceStationId[4..6], 16);

        // 13: Round Trip Control Information (1 byte)
        headerBytes[12] = Convert.ToByte(_roundTripControlInformation ?? "00", 16);

        // 14-16: Message Status Flag (3 bytes)
        string messageStatusFlag = _messageStatusFlag ?? "000000";
        headerBytes[13] = Convert.ToByte(messageStatusFlag[0..2], 16);
        headerBytes[14] = Convert.ToByte(messageStatusFlag[2..4], 16);
        headerBytes[15] = Convert.ToByte(messageStatusFlag[4..6], 16);

        // 17: User Information (1 byte)
        headerBytes[16] = Convert.ToByte(_userInformation ?? "00", 16);

        // 18-23: Reserved/Unused (set to 0)
        for (int i = 17; i <= 22; i++)
            headerBytes[i] = 0x00;

        // 24-25: MTI (2 bytes)
        headerBytes[23] = Convert.ToByte(mti[0..2], 16);
        headerBytes[24] = Convert.ToByte(mti[2..4], 16);

        return headerBytes.ToArray();
    }
}
