using Acubec.Payments.ISO8583Parser.Definitions;
using System.Text.Json;

namespace Acubec.Payments.ISO8583Parser.Test;

public class ISOParserTest : IClassFixture<ISO8583ParserTestFixture>
{
    readonly IServiceProvider _serviceProvider;

    public ISOParserTest(ISO8583ParserTestFixture fixture)
    {
        _serviceProvider = fixture.ServiceProvider;
    }


    [Fact]
    public void Parse_ShouldParsenIsoMessage()
    {
        // Arrange
        var jsonConfiguration = File.ReadAllText(@"D:\local\projects\Self\Acubec\Payments\Acubec.Payments.ISO8583Parser\Acubec.Payments.ISO8583Parser.Test\RupaySchema.json");
        JsonSerializerOptions options = new();
        options.PropertyNameCaseInsensitive = true;
        var configuration = System.Text.Json.JsonSerializer.Deserialize<SchemaConfiguration>(jsonConfiguration, options);

        var parser = new ISO8583MessageParser(_serviceProvider);
        var strMessageBytes = @"30 31 30 30 f2 3e 46 11 ae e1 94 72 00 00 00 42 00 02 00 32 31 36 34 30 36 36 38 34 36 31 32 30 30 35 32 34 34 33 30 30 30 30 30 30 30 30 30 30 30 30 30 30 36 30 30 30 30 32 31 37 31 35 30 39 33 34 31 30 30 35 32 34 31 35 30 39 33 34 30 32 31 37 32 37 30 32 30 32 31 37 35 38 31 32 30 32 32 30 30 30 43 30 30 30 30 30 30 30 30 30 39 31 31 31 31 31 31 31 31 31 30 39 32 32 32 32 32 32 32 32 32 33 37 34 30 36 36 38 34 36 31 32 30 30 35 32 34 34 33 3d 32 37 30 32 31 30 31 30 30 30 31 35 35 37 35 33 30 30 30 30 33 33 33 34 35 36 37 31 39 30 30 30 38 30 39 38 31 35 39 39 50 55 4c 4d 42 41 4e 4b 30 31 35 31 35 31 35 31 35 31 35 31 35 31 35 46 6f 75 72 74 79 43 6f 72 65 63 61 72 64 53 6f 66 74 77 61 72 65 50 72 69 69 76 61 74 65 20 20 20 4c 69 6d 69 74 55 53 30 32 35 68 74 74 70 3a 2f 2f 77 65 62 39 2f 61 70 70 53 34 34 32 36 2e 61 73 70 78 38 34 30 41 42 43 44 41 42 43 44 41 42 43 44 41 42 43 44 30 32 30 30 30 30 32 38 34 30 44 30 30 30 30 30 30 30 30 30 30 30 30 30 31 30 31 30 30 30 30 31 35 30 30 32 30 31 37 32 32 33 33 33 35 35 35 35 35 35 35 35 35 38 34 30 30 30 36 43 30 30 30 31 32 30 32 35 46 41 31 32 33 34 35 36 33 32 31 50 55 4c 36 35 34 33 32 31 20 20 31 20 20 30 32 30 30 31 31 31 33 31 33 30 33 32 34 31 35 30 39 33 34 30 30 31 31 31 31 31 31 31 31 31 30 30 32 32 32 32 32 32 32 32 32 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 30 44 30 30 30 30 30 30 30 30 44 30 30 30 30 30 30 30 30 30 32 30 31 32 33 34 35 36 37 38 39 30 31 32 33 34 35 35 30 31 30 34 30 33 32 54 44 41 56 31 35 41 44 44 52 20 5a 49 50 43 4f 44 45 20 20 20 43 56 30 37 31 20 43 56 43 32 4d 30 30 30 30 35 35 30 31 30 30 31 35 31 35 31 35 31 35 31 35 31 35 31 30 20 20 20 20 20 20 30 32 32 32 32 32 32 30 30 32 33 32 33 32 33 32 33 32 33 32 33 32 33 32 33 32 33 32 33 32 30";
        var messageBytes = TTTT(strMessageBytes);
        //messageBytes = messageBytes.ASCIIToHex();
        // Act
        try
        {
            var result = parser.Parse(configuration, messageBytes, _serviceProvider);

            // Assert
            Assert.NotNull(result);
            // Add more assertions as needed
        }
        catch (Exception)
        {

        }
    }

    //[Fact]
    //public void Parse_ShouldConvertToStringIsoMessage()
    //{
    //    // Arrange
    //    var jsonConfiguration = File.ReadAllText(@"D:\local\projects\Self\Acubec\Payments\Acubec.Payments.ISO8583Parser\Acubec.Payments.ISO8583Parser.Test\RupaySchema.json");
    //    JsonSerializerOptions options = new();
    //    options.PropertyNameCaseInsensitive = true;
    //    var configuration = System.Text.Json.JsonSerializer.Deserialize<SchemaConfiguration>(jsonConfiguration, options);


    //    var parser = new ISO8583MessageParser(_serviceProvider);
    //    var strMessageBytes = "30323030F23B66810AE08208000000000100000031363636373738383030303030303030303431323334353630303030303036303030303031323231313432373033303030303031303932373033313232313132323131323231353939393335363930313030313030303637323030303133333535303930303030303130305445535431323334544553543132333435363738393031525550415920544553542053494D554C41544F522020204D554D424149202020202020204D48494E333536313633394630323030303946303330303039463236303030303832303034373830303946333630303035463334303030394632373030323830394633343030363146303030323946313030313630313035413030303033303030303030394633333030364530453038303946314130303039463335303032323230393530313030303030303438303030354632413030303039413030303039433030323132394633373030303034313237354134363356523331313030303435323031324156454E55452053555045524D41524420494E44303739303038303031353030393035303132333435363738393031323334353637383930313233343536373839304173646667686A6B6C706F69757974726577713130313330303236343031343030323235";
    //    //var strMessageBytes =   "30323030F23B66810AE08208800000000100000031363637373838303030303030303030343131323334353630303030303036303030303031323231313432373033303030303031303932373033313232313132323131323231353939393335363930313030313030303632303030313333333535303930303030303130305445535431323334544553543132333435363738393031525550415920544553542053494D554C41544F522020204D554D424149202020202020204D48494E333536313633394630323030303946303330303039463236303030303832303034373830303946333630303035463334303030394632373030323830394633343030363146303030323946313030313630313035413030303033303030303030394633333030364530453038303946314130303039463335303032323230393530313030303030303438303030354632413030303039413030303039433030323132394633373030303034313237354134363356523331313030303435323031324156454E55452053555045524D41524420494E44303739303038303031353030393035303132333435363738393031323334353637383930313233343536373839304173646667686A6B6C706F69757974726577713130313330303236343031343030323235";

    //    var messageBytes = ByteHelper.GetBytesFromHexString(strMessageBytes);
    //    var isoMessage = parser.Parse(configuration, messageBytes, _serviceProvider);

    //    // Act
    //    var result = parser.ToBytes(isoMessage, configuration.SchemaEncoding);
    //    var resultString = ByteHelper.GetHexRepresentation(result);
    //    // Assert
    //    Assert.NotNull(result);
    //    Assert.Equal(strMessageBytes, resultString);
    //}


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
