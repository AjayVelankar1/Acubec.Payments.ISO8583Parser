using Acubec.Payments.ISO8583Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583Parser.Definitions;

public class SchemaConfiguration
{
    public string SchemaName { get; set; }
    public string SchemaDescription { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public DataEncoding SchemaEncoding { get; set; }

    public List<Field> Fields { get; set; }

    public List<Message> Messages { get; set; }
    public int ByteMapLength { get; set; }
}
