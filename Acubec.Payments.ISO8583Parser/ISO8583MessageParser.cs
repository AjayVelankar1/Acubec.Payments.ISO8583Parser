using Acubec.Payments.ISO8583Parser.DataTypes;
using Acubec.Payments.ISO8583Parser.Definitions;
using Acubec.Payments.ISO8583Parser.Helpers;
using Acubec.Payments.ISO8583Parser.Interfaces;
using Acubec.Payments.ISO8583Parser.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583Parser;

public sealed class ISO8583MessageParser
{
    private readonly IServiceProvider _serviceProvider;
    StringBuilder _logDump;
    StringBuilder _hexDump;
    public string LogDump =>_logDump.ToString();
    public string HexDump => _hexDump.ToString();
    public ISO8583MessageParser(IServiceProvider serviceProvider)
    {
        _logDump = new();
        _hexDump = new();    
        this._serviceProvider = serviceProvider;
    }

    public IIsoMessage Parse(SchemaConfiguration schemaConfiguration, byte[] messageBytes, IServiceProvider serviceProvider)
    {
        var mtiParser = (IMTIParser)_serviceProvider.GetService(typeof(IMTIParser));

        //Parsing MTI
        var mti = mtiParser.ParseMTI(messageBytes);
        var isoMessage = new IsoRequest(mti, schemaConfiguration.SchemaEncoding, schemaConfiguration.ByteMapLength);

        //setting fields from configuration
        foreach (var field in schemaConfiguration.Fields)
        {
            var isoField = getField(field, isoMessage.ByteMap, serviceProvider);
            isoMessage.Fields.Add(field.Index, isoField);
        }

        //Setting Message as per configurations
        var messageConfiguration = schemaConfiguration.Messages.Where(c => c.Alias == isoMessage.MessageType).FirstOrDefault();
        isoMessage.IsAdviceMessage = messageConfiguration.IsAdviceMessage;
        isoMessage.IsRepeatMessage = messageConfiguration.IsRepeatMessage;
        isoMessage.IsResponseMessage = messageConfiguration.IsResponseMessage;
        isoMessage.IsNetworkMessage = messageConfiguration.IsNetworkMessage;
        isoMessage.IsFinancialTransaction = messageConfiguration.IsNetworkMessage;

        parse(isoMessage, messageBytes, schemaConfiguration.SchemaEncoding);

        return isoMessage;
    }

    public byte[] ToBytes(IIsoMessage message, DataEncoding encoding)
    {
        var mtiParser = (IMTIParser)_serviceProvider.GetService(typeof(IMTIParser));
        return ((IsoRequest)message).ByteMap.GetDataByte(encoding, mtiParser);
    }

    private IIsoField getField(Field field, ByteMaps byteMap, IServiceProvider serviceProvider)
    {
        try
        {
            ICustomFiledFactory customFieldFactory = default;
            try
            {
                customFieldFactory = (ICustomFiledFactory)_serviceProvider.GetService(typeof(ICustomFiledFactory));
            }
            catch
            {

            }

            var isoFiled = field.Type switch
            {
                "Fixed" => (IIsoField)new IsoAlphaNumericFixedField(field.Name, (short)field.SizeInt, field.Index, byteMap, serviceProvider),
                "Variable" => (IIsoField)new IsoBaseVariableLengthField(field.Name, (short)field.SizeInt, field.Index, byteMap, serviceProvider, field.DataEncoding, field.HeaderLengthEncoding),
                "TagValueSubField" => (IIsoField)new TagValueSubField(field.Name, (short)field.SizeInt, field.Index, byteMap, serviceProvider, field.DataEncoding),
                _ => customFieldFactory.GetField(field, byteMap)
            };
            if (isoFiled != null)
            {
                isoFiled.Mask = field.Mask;
            }
            return isoFiled;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private void parse(IsoRequest isoRequest, byte[] messageBytes, DataEncoding encoding)
    {
        StringBuilder logDump = new();
        var offSet = parseByteMap(isoRequest, messageBytes, encoding);
        parseData(isoRequest, messageBytes, encoding, offSet);
        _hexDump.Append(ByteHelper.HexDump(messageBytes));
    }

    private int parseByteMap(IsoRequest isoRequest, byte[] messageBytes, DataEncoding encoding)
    {
        var dataByte = messageBytes;
        int skipBytes = 0;
        int multiplyer = 1;
        byte[] b = Array.Empty<byte>();

        StringBuilder _logDump = new();
        _logDump.Append($"Start converting byte array to ISOMessage MTI: {isoRequest.MessageType}{Environment.NewLine}");
        _logDump.Append($"Raw byte array is{Environment.NewLine} {BitConverter.ToString(messageBytes)}{Environment.NewLine}");

        Dictionary<int, IIsoField> fields = isoRequest.Fields;
        int offset = skipBytes + 4;

        for (int i = 0; i < isoRequest.ByteMapLength; i++)
        {
             
            if (encoding == DataEncoding.Binary)
            {
                multiplyer = 1;
                b = dataByte.GetByteSlice(8 * multiplyer, offset);
            }
            else
            {
                multiplyer = 2;
                b = dataByte.GetByteSlice(8 * multiplyer, offset);
                var str = Encoding.ASCII.GetString(b);
                b = ByteHelper.convertHexToBinaryUsingConvert(str);
            }
            isoRequest.ByteMap.SetBitMap(i, b);
            offset += 8 * multiplyer;
        }
        return offset;
    }

    private int parseData(IsoRequest isoRequest, byte[] messageBytes, DataEncoding encoding, int offset)
    {
        var dataByte = messageBytes;
        int maxElements = isoRequest.ByteMapLength * 64;
        Dictionary<int, IIsoField> fields = isoRequest.GetFieldList();

        for (int l = 1; l < maxElements; l++)
        {
            var i = l;
            int len = 0;
            int bitMapIndex = l / 64;

            if (!fields.ContainsKey(l))
                continue;

            if (i <= 64 && (isoRequest.ByteMap.getBitMap(0)[(i - 1) / 8] >> 8 - (i - 8 * ((i - 1) / 8)) & 0x01) == 0x01)
            {

                len = fields[l].SetValueBytes(dataByte, offset);
                offset += len;
                _logDump.Append($"{Environment.NewLine}DE-[{l}]:[{fields[l].Name}]=> [{fields[l].LogDump()}]");
            }
            else if (i > 64 && (isoRequest.ByteMap.getBitMap(bitMapIndex)[(i - (64 * bitMapIndex) - 1) / 8] >> 8 - (i - (64 * bitMapIndex) - 8 * ((i - (64 * bitMapIndex) - 1) / 8)) & 0x01) == 0x01)
            {
                len = fields[l].SetValueBytes(dataByte, offset);
                offset += len;
                _logDump.Append($"{Environment.NewLine}DE-[{l}]:[{fields[l].Name}]=> [{fields[l].LogDump()}]");
            }
        }
        _logDump.Append($"End Data Message{Environment.NewLine}");
        return 0;
    }
}