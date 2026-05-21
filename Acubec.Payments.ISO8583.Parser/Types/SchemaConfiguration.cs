using MemoryPack;
using System.Text.Json.Serialization;

namespace Acubec.Payments.ISO8583.Parser.Types;

public class SchemaConfiguration
{
    public string HeaderParserName { get; set; }
    public string NetworkName { get; set; }
    public string NetworkDescription { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DataEncoding SchemaEncoding { get; set; }

    public List<Field> Fields { get; set; }

    public List<Message> Messages { get; set; }
    public int ByteMapLength { get; set; }
    public string SchemaName { get; set; }
}

[MemoryPackable]
public sealed partial record Field
{
    public int Index { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public string Type { get; set; }

    public string Size { get; set; }
    public int SizeInt => int.Parse(Size);

    public string DataType { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DataEncoding DataEncoding { get; set; }

    public string? PaddingChractor { get; set; }
    public bool? Mask { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DataEncoding HeaderLengthEncoding { get; set; }
    public int? HeaderLength { get; set; }
}

[MemoryPackable]
public sealed partial class Message
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Alias { get; set; }
    public string Type { get; set; }
    public string[] Identifier { get; set; }
    public string[] Fields { get; set; }
    public bool? IsAdviceMessage { get; set; }
    public bool? IsRepeatMessage { get; set; }
    public bool? IsResponseMessage { get; set; }
    public bool? IsNetworkMessage { get; set; }
}
