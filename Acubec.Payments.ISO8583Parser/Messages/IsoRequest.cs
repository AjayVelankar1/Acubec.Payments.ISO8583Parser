using Acubec.Payments.ISO8583Parser.Helpers;
using Acubec.Payments.ISO8583Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583Parser.Messages;

internal class IsoRequest : IIsoMessage
{
    protected ByteMap _byteMap;
    protected StringBuilder _logDump;
    protected string _messageType;
    protected string _messageEncoding;
    Dictionary<int, IIsoField> _fields;

    public Dictionary<int, IIsoField> Fields => _fields;

    public IsoRequest(string messageType, string encoding)
    {
        _messageType = messageType;
        _byteMap = new ByteMap(MessageType);
        _logDump = new StringBuilder();
        _messageEncoding = encoding;    
    }

    public bool IsAdviceMessage
    {
        get;set;
    }

    public bool IsFinancialTransaction
    {
        get; set;
    }

    public bool IsNetworkMessage
    {
        get; set;
    }

    public bool IsResponseMessage
    {
        get; set;
    }

    public string MessageId
    {
        get; set;
    }

    public string MessageType => _messageType;

    public ByteMap ByteMap => ByteMap;

    public string STANValue { get; }
    public string RRNValue { get; }

    public Dictionary<int, IIsoField> GetFieldList()
    {
        return _fields;
    }

    #region Public Properties

    public bool IsRepeatMessage { get; set; }
    public DateTime NetworkSentTime { get; set; }
    public DateTime NetworkReceivedTime { get; set; }
    public string SAFId { get; set; }
    public string TransactionId { get; set; }
   
    public override string ToString()
    {
        return base.ToString();
    }
    #endregion Public Properties
}

internal abstract class IsoResponse : IsoRequest
{
    protected IsoResponse(string messageType, string encoding) : base(messageType, encoding)
    {
    }

    public string ResponseCode { get; }

}

