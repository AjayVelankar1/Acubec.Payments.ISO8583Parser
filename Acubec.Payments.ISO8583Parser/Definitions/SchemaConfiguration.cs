using Acubec.Payments.ISO8583Parser.Interfaces;
using System.Text.Json.Serialization;

namespace Acubec.Payments.ISO8583Parser.Definitions;

public class SchemaConfiguration
{

    public string MTIParser { get; set; }
    public string SchemaName { get; set; }
    public string SchemaDescription { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DataEncoding SchemaEncoding { get; set; }

    public List<Field> Fields { get; set; }

    public List<Message> Messages { get; set; }
    public int ByteMapLength { get; set; }
}