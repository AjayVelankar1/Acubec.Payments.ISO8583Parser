using Acubec.Payments.ISO8583Parser.Interfaces;
using System.Text;

namespace Acubec.Payments.ISO8583Parser.Messages;


internal class IsoRequest : IIsoMessage
{
    protected ByteMaps _byteMap;
    protected StringBuilder _logDump;
    protected string _messageType;
    protected DataEncoding _messageEncoding;
    Dictionary<int, IIsoField> _fields;
    protected int _byteMapLength;

    public Dictionary<int, IIsoField> Fields => _fields;

    public IsoRequest(string messageType, DataEncoding encoding, int byteMapLength)
    {
        _messageType = messageType;
        _byteMapLength = byteMapLength;
        _byteMap = new ByteMaps(MessageType, ByteMapLength);
        _logDump = new StringBuilder();
        _messageEncoding = encoding;
        _fields = new();
    }

    public int ByteMapLength => _byteMapLength;

    public bool IsAdviceMessage
    {
        get; set;
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

    public ByteMaps ByteMap => _byteMap;

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
    protected IsoResponse(string messageType, DataEncoding encoding, int byteMapLength) : base(messageType, encoding, byteMapLength)
    {
    }

    public string ResponseCode { get; }

}
