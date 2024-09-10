namespace Acubec.Payments.ISO8583Parser.Interfaces;

public interface IIsoMessage
{
    #region Public Properties
    int ByteMapLength { get; }

    bool IsAdviceMessage { get; }
    bool IsFinancialTransaction { get; }
    bool IsNetworkMessage { get; }
    bool IsResponseMessage { get; }
    string MessageId { get; }
    string MessageType { get; }

    string STANValue { get; }

    string RRNValue { get; }

    DateTime NetworkSentTime { get; set; }
    DateTime NetworkReceivedTime { get; set; }
    public string TransactionId { get; set; }

    public Dictionary<int, IIsoField> Fields { get; }
    #endregion Public Properties
}
