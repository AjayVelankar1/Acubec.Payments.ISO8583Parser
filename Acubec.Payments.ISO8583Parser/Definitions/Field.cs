using Acubec.Payments.ISO8583Parser.Interfaces;
using System.Text.Json.Serialization;

namespace Acubec.Payments.ISO8583Parser.Definitions;

public enum FieldTypes
{
    Fixed,
    Variable,
    TagValueSubField,
}

public sealed class Field
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
    public bool isPaddingRequired { get; set; }
    public bool Mask { get; set; }
    public DataEncoding HeaderLengthEncoding { get; set; }
}
