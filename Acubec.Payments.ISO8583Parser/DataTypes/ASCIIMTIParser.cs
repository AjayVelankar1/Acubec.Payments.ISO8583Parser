﻿using Acubec.Payments.ISO8583Parser.Helpers;
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

    public Span<byte> WriteMTI(string mti, int totalLength = 0)
    {
        Span<byte> mtiBytes = stackalloc byte[4];
        Encoding.ASCII.TryGetBytes(mti, mtiBytes, out var output);
        return mtiBytes.ToArray();
    }
}

internal sealed class VisaMTIParser : IMTIParser
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

    public int SkipBytes => _headerLength + 2 + 8;

    public string ParseMTI(Span<byte> isoMessage)
    {
        if (isoMessage == null || isoMessage.Length < 4)
            throw new ArgumentException("Invalid ISO Message");

        _messageLength = Convert.ToInt32(ByteHelper.ByteSpanToHexString(isoMessage[0..2]), 16);

        var lengthBytes = isoMessage[2..3];
        _headerLength = ByteHelper.ByteSpanToHexString(lengthBytes).TryParse<int>(0);

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

        headerBytes[4] = Convert.ToByte(this._headerFlagAndFormat ?? "01", 16);
        headerBytes[5] = Convert.ToByte(this._textFormat ?? "02", 16);


        headerBytes[6] = headerBytes[0];
        headerBytes[7] = headerBytes[1];

        this._designatedStationId = this._designatedStationId ?? "000000";

        headerBytes[8] = Convert.ToByte(this._designatedStationId[0..2], 16);
        headerBytes[9] = Convert.ToByte(this._designatedStationId[2..4], 16);
        headerBytes[10] = Convert.ToByte(this._designatedStationId[4..6], 16);


        this._sourceStationId = this._sourceStationId ?? "000000";

        headerBytes[11] = Convert.ToByte(this._sourceStationId[0..2], 16);
        headerBytes[12] = Convert.ToByte(this._sourceStationId[2..4], 16);
        headerBytes[13] = Convert.ToByte(this._sourceStationId[4..6], 16);

        headerBytes[14] = Convert.ToByte(this._roundTripControlInformation ?? "00", 16);

        this._baseIFlag = this._baseIFlag ?? "0000";
        headerBytes[15] = Convert.ToByte(this._baseIFlag[0..2], 16);
        headerBytes[16] = Convert.ToByte(this._baseIFlag[2..4], 16);



        this._messageStatusFlag = this._messageStatusFlag ?? "000000";

        headerBytes[17] = Convert.ToByte(this._messageStatusFlag[0..2], 16);
        headerBytes[18] = Convert.ToByte(this._messageStatusFlag[2..4], 16);
        headerBytes[19] = Convert.ToByte(this._messageStatusFlag[4..6], 16);

        headerBytes[20] = Convert.ToByte(this._batchNumber ?? "00", 16);

        headerBytes[21] = Convert.ToByte("00", 16);
        headerBytes[22] = Convert.ToByte("00", 16);
        headerBytes[23] = Convert.ToByte("00", 16);
        headerBytes[24] = Convert.ToByte(this._userInformation ?? "00", 16);

        headerBytes[25] = Convert.ToByte(mti[0..2], 16);
        headerBytes[25] = Convert.ToByte(mti[2..4], 16);

        return headerBytes.ToArray();
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

    public Span<byte> WriteMTI(string mti, int totalLength = 0)
    {
        Span<byte> mtiBytes = stackalloc byte[4];
        Encoding.ASCII.TryGetBytes(mti, mtiBytes, out var output);
        return mtiBytes.ToArray();
    }
}