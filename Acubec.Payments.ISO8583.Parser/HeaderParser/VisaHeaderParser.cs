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

        string hexValue = totalLength.ToString("X4");
        headerBytes[0] = Convert.ToByte(hexValue[0..2], 16); // First byte
        headerBytes[1] = Convert.ToByte(hexValue[2..4], 16); // Second byte

        if (_headerLength == 0) _headerLength = 16;
        headerBytes[2] = Convert.ToByte(_headerLength);


        hexValue = _headerLength.ToString("X4");
        headerBytes[3] = Convert.ToByte(hexValue[0..2], 16); // First byte
        headerBytes[4] = Convert.ToByte(hexValue[2..4], 16); // Second byte

        headerBytes[4] = Convert.ToByte(_headerFlagAndFormat ?? "01", 16);
        headerBytes[5] = Convert.ToByte(_textFormat ?? "02", 16);


        headerBytes[6] = headerBytes[0];
        headerBytes[7] = headerBytes[1];

        _designatedStationId = _designatedStationId ?? "000000";

        headerBytes[8] = Convert.ToByte(_designatedStationId[0..2], 16);
        headerBytes[9] = Convert.ToByte(_designatedStationId[2..4], 16);
        headerBytes[10] = Convert.ToByte(_designatedStationId[4..6], 16);


        _sourceStationId = _sourceStationId ?? "000000";

        headerBytes[11] = Convert.ToByte(_sourceStationId[0..2], 16);
        headerBytes[12] = Convert.ToByte(_sourceStationId[2..4], 16);
        headerBytes[13] = Convert.ToByte(_sourceStationId[4..6], 16);

        headerBytes[14] = Convert.ToByte(_roundTripControlInformation ?? "00", 16);

        _baseIFlag = _baseIFlag ?? "0000";
        headerBytes[15] = Convert.ToByte(_baseIFlag[0..2], 16);
        headerBytes[16] = Convert.ToByte(_baseIFlag[2..4], 16);



        _messageStatusFlag = _messageStatusFlag ?? "000000";

        headerBytes[17] = Convert.ToByte(_messageStatusFlag[0..2], 16);
        headerBytes[18] = Convert.ToByte(_messageStatusFlag[2..4], 16);
        headerBytes[19] = Convert.ToByte(_messageStatusFlag[4..6], 16);

        headerBytes[20] = Convert.ToByte(_batchNumber ?? "00", 16);

        headerBytes[21] = Convert.ToByte("00", 16);
        headerBytes[22] = Convert.ToByte("00", 16);
        headerBytes[23] = Convert.ToByte("00", 16);
        headerBytes[24] = Convert.ToByte(_userInformation ?? "00", 16);

        headerBytes[25] = Convert.ToByte(mti[0..2], 16);
        headerBytes[25] = Convert.ToByte(mti[2..4], 16);

        return headerBytes.ToArray();
    }
}
