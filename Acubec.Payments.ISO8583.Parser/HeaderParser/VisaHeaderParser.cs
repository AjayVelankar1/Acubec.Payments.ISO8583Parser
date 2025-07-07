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

    public int HeaderLength => _headerLength + 2 + 8;

    public VisaHeaderParser()
    {
        _otherProperties = new Dictionary<string, string>();
    }

    public Dictionary<string, string> OtherProperties => _otherProperties;

    public string ReadMTI(Span<byte> isoMessage)
    {
        
        if (isoMessage == null || isoMessage.Length < 4)
            throw new ArgumentException("Invalid ISO Message");

        _messageLength = Convert.ToInt32(ByteHelper.ByteSpanToHexString(isoMessage[0..2]), 16);

        var lengthBytes = isoMessage[2..3];
        _headerLength = ByteHelper.ByteSpanToHexString(lengthBytes).TryParse(0);

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

    public Span<byte> WriteMTI(string mti, int totalLength = 0)
    {
        Span<byte> headerBytes = stackalloc byte[26];

        // 0-1: Message Length (2 bytes, big-endian)
        headerBytes[0] = (byte)((totalLength >> 8) & 0xFF); // High byte
        headerBytes[1] = (byte)(totalLength & 0xFF);        // Low byte

        // 2: Header Length (1 byte, always 16 for Visa)
        if(_headerLength == 0) _headerLength  = 16;
        headerBytes[2] = Convert.ToByte(_headerLength.ToString(),16);
        

        // 3: Header Flag and Format (1 byte)
        headerBytes[3] = Convert.ToByte(_headerFlagAndFormat ?? "01", 16);

        // 4: Text Format (1 byte)
        headerBytes[4] = Convert.ToByte(_textFormat ?? "02", 16);

        // 5-6: Total Message Length (2 bytes)

        headerBytes[5] = (byte)((totalLength >> 8) & 0xFF); // High byte
        headerBytes[6] = (byte)(totalLength & 0xFF);        // Low byte

        // 7-9: Designated Station Id (3 bytes)
        string designatedStationId = _designatedStationId ?? "000000";
        headerBytes[7] = Convert.ToByte(designatedStationId[0..2], 16);
        headerBytes[8] = Convert.ToByte(designatedStationId[2..4], 16);
        headerBytes[9] = Convert.ToByte(designatedStationId[4..6], 16);

        // 10-12: Source Station Id (3 bytes)
        string sourceStationId = _sourceStationId ?? "000000";
        headerBytes[10] = Convert.ToByte(sourceStationId[0..2], 16);
        headerBytes[11] = Convert.ToByte(sourceStationId[2..4], 16);
        headerBytes[12] = Convert.ToByte(sourceStationId[4..6], 16);

        // 13: Round Trip Control Information (1 byte)
        headerBytes[13] = Convert.ToByte(_roundTripControlInformation ?? "00", 16);

        // 14-16: Message Status Flag (3 bytes)
        string messageStatusFlag = _messageStatusFlag ?? "000000";
        headerBytes[14] = Convert.ToByte(messageStatusFlag[0..2], 16);
        headerBytes[15] = Convert.ToByte(messageStatusFlag[2..4], 16);
        headerBytes[16] = Convert.ToByte(messageStatusFlag[4..6], 16);

        // 17: User Information (1 byte)
        headerBytes[17] = Convert.ToByte(_userInformation ?? "00", 16);

        // 18-23: Reserved/Unused (set to 0)
        for (int i = 18; i <= 23; i++)
            headerBytes[i] = 0x00;

        // 24-25: MTI (2 bytes)
        headerBytes[24] = Convert.ToByte(mti[0..2], 16);
        headerBytes[25] = Convert.ToByte(mti[2..4], 16);

        return headerBytes.ToArray();
    }
}
