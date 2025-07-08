using Acubec.Payments.ISO8583.Parser.Interfaces;
using Acubec.Payments.ISO8583.Parser.Types;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583.Parser.Test;

[TestFixture]
public class ISOParserTest: GlobalTestSetup
{



    [TestCase(@"D:\Local\Projects\Self\Acubec.Payments.ISO8583Parser\Acubec.Payments.ISO8583.Parser.Test\Scemas\Visa.json",
        @"16 01 02 00 D4 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 04 00 F2 3C 64 91 08 E0 80 06 00 00 00 40 00 00 00 00 10 41 23 70 99 99 00 00 29 00 00 00 00 00 00 00 20 00 03 27 14 47 09 00 00 06 10 47 09 03 27 31 12 53 10 07 72 90 10 00 C4 F0 F0 F0 F0 F0 F0 F0 F0 0B 01 23 45 67 89 01 F0 F0 F0 F0 F0 F0 F0 F0 F0 F0 F0 F0 E3 C5 D9 D4 C9 C4 F0 F1 F0 F0 F0 F0 F0 F0 F0 F0 F0 F0 F0 F0 F0 F0 F0 C1 C3 D8 E4 C9 D9 C5 D9 40 D5 C1 D4 C5 40 40 40 40 40 40 40 40 40 40 40 40 C3 C9 E3 E8 40 D5 C1 D4 C5 40 40 40 40 E4 E2 08 40 08 00 00 00 00 00 00 00 00 08 A0 20 00 00 03 25 01 E9 02 00 00 00 00 00 00 00 00 00 12 34 56 78 90 10 00 00 00 00 00")]

    [TestCase(@"D:\Local\Projects\Self\Acubec.Payments.ISO8583Parser\Acubec.Payments.ISO8583.Parser.Test\Scemas\Visa.json",
        @"16 01 02 01 32 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 02 00 62 3E 66 81 28 E0 9A 12 10 52 88 98 00 00 00 16 15 00 00 00 07 08 00 26 14 00 00 24 20 26 14 07 07 25 12 07 07 60 11 08 40 05 10 00 01 00 0B 01 23 45 67 89 01 1B 01 05 97 20 00 12 34 94 8D 25 12 20 15 49 F5 F1 F8 F9 F0 F0 F0 F0 F0 F0 F2 F4 C1 E3 D4 F0 F1 40 40 40 C3 C1 D9 C4 40 C1 C3 C3 C5 D7 E3 D6 D9 40 40 C1 C3 D8 E4 C9 D9 C5 D9 40 D5 C1 D4 C5 40 40 40 40 40 40 40 40 40 40 40 40 C3 C9 E3 E8 40 D5 C1 D4 C5 40 40 40 40 E4 E2 08 40 3E 39 22 B1 DB 25 0D D7 20 01 01 01 00 00 00 00 6C 01 00 69 9F 33 03 20 40 00 95 05 80 00 01 00 00 9F 37 04 9B AD BC AB 9F 10 07 06 01 0A 03 A0 00 00 9F 26 08 01 23 45 67 89 AB CD EF 9F 36 02 00 FF 82 02 00 00 9C 01 00 9F 1A 02 08 40 9A 03 01 01 01 9F 02 06 00 00 00 01 23 00 5F 2A 02 08 40 9F 03 06 00 00 00 00 00 00 C0 08 3E 39 22 B1 DB 25 0D D7 84 07 A0 00 00 00 03 10 10 04 25 00 00 10 06 80 20 00 00 02 F0")]
    [TestCase(@"D:\Local\Projects\Self\Acubec.Payments.ISO8583Parser\Acubec.Payments.ISO8583.Parser.Test\Scemas\Visa.json",
        @"16 01 02 00 2B 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 08 00 82 20 00 00 08 00 00 00 04 00 00 01 00 00 00 00 00 07 07 19 09 28 00 00 00 01 F0 F0 F0 F0 F0 F1 F1 F5 F1 F1 F0 F2 00 06 04 02 09 43")]
    [TestCase(@"D:\Local\Projects\Self\Acubec.Payments.ISO8583Parser\Acubec.Payments.ISO8583.Parser.Test\Scemas\RupaySchema.json"
        , @"30 31 30 30 f2 3e 46 11 ae e1 94 72 00 00 00 42 00 02 00 32 31 36 34 30 36 36 38 34 36 31 32 30 30 35 32 34 34 33 30 30 30 30 30 30 30 30 30 30 30 30 30 30 36 30 30 30 30 32 31 37 31 35 30 39 33 34 31 30 30 35 32 34 31 35 30 39 33 34 30 32 31 37 32 37 30 32 30 32 31 37 35 38 31 32 30 32 32 30 30 30 43 30 30 30 30 30 30 30 30 30 39 31 31 31 31 31 31 31 31 31 30 39 32 32 32 32 32 32 32 32 32 33 37 34 30 36 36 38 34 36 31 32 30 30 35 32 34 34 33 3d 32 37 30 32 31 30 31 30 30 30 31 35 35 37 35 33 30 30 30 30 33 33 33 34 35 36 37 31 39 30 30 30 38 30 39 38 31 35 39 39 50 55 4c 4d 42 41 4e 4b 30 31 35 31 35 31 35 31 35 31 35 31 35 31 35 46 6f 75 72 74 79 43 6f 72 65 63 61 72 64 53 6f 66 74 77 61 72 65 50 72 69 69 76 61 74 65 20 20 20 4c 69 6d 69 74 55 53 30 32 35 68 74 74 70 3a 2f 2f 77 65 62 39 2f 61 70 70 53 34 34 32 36 2e 61 73 70 78 38 34 30 41 42 43 44 41 42 43 44 41 42 43 44 41 42 43 44 30 32 30 30 30 30 32 38 34 30 44 30 30 30 30 30 30 30 30 30 30 30 30 30 31 30 31 30 30 30 30 31 35 30 30 32 30 31 37 32 32 33 33 33 35 35 35 35 35 35 35 35 35 38 34 30 30 30 36 43 30 30 30 31 32 30 32 35 46 41 31 32 33 34 35 36 33 32 31 50 55 4c 36 35 34 33 32 31 20 20 31 20 20 30 32 30 30 31 31 31 33 31 33 30 33 32 34 31 35 30 39 33 34 30 30 31 31 31 31 31 31 31 31 31 30 30 32 32 32 32 32 32 32 32 32 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 44 30 30 30 30 30 30 30 30 44 30 30 30 30 30 30 30 30 30 32 30 31 32 33 34 35 36 37 38 39 30 31 32 33 34 35 35 30 31 30 34 30 33 32 54 44 41 56 31 35 41 44 44 52 20 5a 49 50 43 4f 44 45 20 20 20 43 56 30 37 31 20 43 56 43 32 4d 30 30 30 30 35 35 30 31 30 30 31 35 31 35 31 35 31 35 31 35 31 35 31 30 20 20 20 20 20 20 30 32 32 32 32 32 32 30 30 32 33 32 33 32 33 32 33 32 33 32 33 32 33 32 33 32 33 32 33 32 30")]

    public void Parse_ShouldParseASCIIIsoMessage(string filePath, string strMessageBytes)
    {
        var _serviceProvider = _provider;
        // Arrange
        var jsonConfiguration = File.ReadAllText(filePath);
        JsonSerializerOptions options = new();
        options.PropertyNameCaseInsensitive = true;
        var configuration = System.Text.Json.JsonSerializer.Deserialize<SchemaConfiguration>(jsonConfiguration, options);

        var parser = new ISO8583MessageParser(_serviceProvider, configuration);
        var messageBytes = TTTT(strMessageBytes);
        //messageBytes = messageBytes.ASCIIToHex();
        // Act
        IIsoMessage result;
        result = parser.Parse(messageBytes, _serviceProvider);

        parser = new ISO8583MessageParser(_serviceProvider, configuration);
        var message = parser.GetMTI(messageBytes);
        foreach (var field in message.Fields)
        {
            message.Fields[field.Key].Value = result.Fields[field.Key].Value;
        }


        ReadOnlySpan<byte> bytes;
        try
        {
            bytes = parser.ToBytes(message, DataEncoding.HEX);
        }
        catch (Exception)
        {

            throw;
        }


        var str = BitConverter.ToString(bytes.ToArray());
        messageBytes = TTTT(strMessageBytes);

        parser = new ISO8583MessageParser(_serviceProvider, configuration);
        var result1 = parser.Parse(messageBytes, _serviceProvider);

        for (int i = 0; i < result.Fields.Count; i++)
        {
            if (!result.Fields.ContainsKey(i))
            {
                if (result1.Fields.ContainsKey(i))
                {
                    Assert.Fail($"Field {i} not found in result");
                }
                continue;
            }

            var field = result.Fields[i];
            var field1 = result1.Fields[i];

            if (field?.Value != field1?.Value)
            {
                Assert.That(field?.Value, Is.EqualTo(field1?.Value));
            }
        }
    }


    static byte[] TTTT(string asciiString)
    {
        String[] arr = asciiString.Split(' ');
        byte[] array = new byte[arr.Length];
        for (int i = 0; i < arr.Length; i++) array[i] = Convert.ToByte(arr[i], 16);
        return array;
    }

    static byte[] StringToHexByteArray(string str)
    {
        // Get bytes from the ASCII string
        byte[] bytes = System.Text.Encoding.ASCII.GetBytes(str);

        // Create a byte array to hold the hex representation
        byte[] hexArray = new byte[bytes.Length * 2];

        for (int i = 0; i < bytes.Length; i++)
        {
            // Convert each byte to its hex representation and store in the array
            hexArray[i * 2] = GetHexValue((bytes[i] >> 4) & 0x0F);       // High nibble
            hexArray[i * 2 + 1] = GetHexValue(bytes[i] & 0x0F); // Low nibble
        }

        return hexArray;
    }

    static byte GetHexValue(int nibble)
    {
        // Convert a nibble (4 bits) to its hex ASCII value
        return (byte)(nibble < 10 ? nibble + '0' : nibble - 10 + 'A');
    }
}
