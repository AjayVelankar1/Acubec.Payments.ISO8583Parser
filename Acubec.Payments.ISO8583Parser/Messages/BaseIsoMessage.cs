using Acubec.Payments.ISO8583Parser.Helpers;
using Acubec.Payments.ISO8583Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583Parser.Messages;

internal abstract class BaseIsoMessage : IIsoMessage
{
    protected ByteMap _byteMap;
    protected StringBuilder _logDump;
    protected string _messageType;
    protected string _messageEncoding;


    public BaseIsoMessage(string messageType, string encoding)
    {
        _messageType = messageType;
        _byteMap = new ByteMap(MessageType);
        _logDump = new StringBuilder();
        _messageEncoding = encoding;
    }

    public abstract bool IsAdviceMessage
    {
        get;
    }

    public abstract bool IsFinancialTransaction
    {
        get;
    }

    public abstract bool IsNetworkMessage
    {
        get;
    }

    public abstract bool IsResponseMessage
    {
        get;
    }

    public abstract string MessageId
    {
        get;
    }

    public string MessageType => _messageType;

    public abstract string STANValue { get; }
    public abstract string RRNValue { get; }

    public abstract Dictionary<int, IIsoField> GetFieldList();

    #region Public Properties

    public abstract bool IsRepeatMessage { get; }
    public DateTime NetworkSentTime { get; set; }
    public DateTime NetworkReceivedTime { get; set; }
    public string TransactionId { get; set; }
    public virtual void ProcessMessage(byte[] dataByte)
    {
        int skipBytes = 0;

        _logDump.Append($"Start converting byte array to ISOMessage MTI: {MessageType}{Environment.NewLine}");
        _logDump.Append($"Raw byte array is{Environment.NewLine} {BitConverter.ToString(dataByte)}{Environment.NewLine}");

        Dictionary<int, IIsoField> fields = GetFieldList();

        //check if secondary bitmap is present if yes
        int offset = skipBytes + 4;
        int maxElements = 64;
        // set primary Bit Map
        _byteMap.SetPrimaryBitMap(dataByte.GetByteSlice(8, offset));
        offset += 8;

        if ((_byteMap.PrimaryBitMap[0] >> 7 & 0x01) == 0x01)
        {
            _byteMap.SetSecondaryBitMap(dataByte.GetByteSlice(8, offset));
            offset += 8;
            maxElements = 128;
        }

        //Seting each field
        for (int i = 1; i < maxElements; i++)
        {
            int len = 0;

            if (!fields.ContainsKey(i))
                continue;

            if (i <= 64 && (_byteMap.PrimaryBitMap[(i - 1) / 8] >> 8 - (i - 8 * ((i - 1) / 8)) & 0x01) == 0x01)
            {

                len = fields[i].SetValueBytes(dataByte, offset);
                offset += len;
                _logDump.Append($"{Environment.NewLine}DE-[{i}]:[{fields[i].Name}]=> [{fields[i].LogDump()}]");
            }
            else if (i > 64 && (_byteMap.SecondaryBitMap[(i - 64 - 1) / 8] >> 8 - (i - 64 - 8 * ((i - 64 - 1) / 8)) & 0x01) == 0x01)
            {
                if (!fields.ContainsKey(i))
                    continue;
                len = fields[i].SetValueBytes(dataByte, offset);
                offset += len;
                _logDump.Append($"{Environment.NewLine}DE-[{i}]:[{fields[i].Name}]=> [{fields[i].LogDump()}]");
            }
        }
        _logDump.Append($"End Data Message{Environment.NewLine}");
        //AppContextProvider.Logger.WriteInfoEntry(() => _logDump.ToString());
    }
    public byte[] ToBytes()
    {
        return _byteMap.GetDataByte(_messageEncoding);
    }

    public override string ToString()
    {
        return base.ToString();
    }
    #endregion Public Properties
}
