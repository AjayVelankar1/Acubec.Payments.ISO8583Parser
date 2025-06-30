using Acubec.Payments.ISO8583.Parser.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583.Parser.Implementation;

internal static class ParserHelper
{
    public static T? TryParse<T>(this string str, T result, T defaultValue = default) where T : IParsable<T>
    {
        if (str == null)
        {
            return defaultValue;
        }

        return T.TryParse(str, null, result: out result) ? result : defaultValue;
    }

    public static string TryRightPadding(this string str, int totalWidth, char paddingChar)
    {
        if (str == null)
        {
            str = string.Empty;
        }

        return str.PadRight(totalWidth, paddingChar);
    }
    public static SchemaConfiguration GetSchemaConfiguration(string schemaPath)
    {
        var jsonConfiguration = File.ReadAllText(schemaPath);
        JsonSerializerOptions options = new();
        options.PropertyNameCaseInsensitive = true;
        var schemaConfiguration = JsonSerializer.Deserialize<SchemaConfiguration>(jsonConfiguration, options);
        return schemaConfiguration; ;
    }
}
