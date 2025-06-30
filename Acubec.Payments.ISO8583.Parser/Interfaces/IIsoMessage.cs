using Acubec.Payments.ISO8583.Parser.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583.Parser.Interfaces;

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

    public string ToJson(bool maskData = true);
    public IIsoMessage FromJson(string message);
    #endregion Public Properties
}
