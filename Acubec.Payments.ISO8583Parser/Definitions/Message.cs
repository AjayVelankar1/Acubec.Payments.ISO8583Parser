namespace Acubec.Payments.ISO8583Parser.Definitions;

public sealed class Message
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Alias { get; set; }
    public string Type { get; set; }
    public string[] Identifier { get; set; }
    public string[] Fields { get; set; }
    public bool IsAdviceMessage { get; set; }
    public bool IsRepeatMessage { get; set; }
    public bool IsResponseMessage { get; set; }
    public bool IsNetworkMessage { get; set; }
}