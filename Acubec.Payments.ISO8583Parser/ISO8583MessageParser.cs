using Acubec.Payments.ISO8583Parser.DataTypes;
using Acubec.Payments.ISO8583Parser.Definitions;
using Acubec.Payments.ISO8583Parser.Helpers;
using Acubec.Payments.ISO8583Parser.Interfaces;
using Acubec.Payments.ISO8583Parser.Messages;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace Acubec.Payments.ISO8583Parser;
public sealed class ISO8583MessageParser
{
    private readonly IServiceProvider _serviceProvider;
    StringBuilder _logDump;
    StringBuilder _hexDump;
    private readonly SchemaConfiguration _schemaConfiguration;

    public string LogDump => _logDump.ToString();
    public string HexDump => _hexDump.ToString();
    public ISO8583MessageParser(IServiceProvider serviceProvider, SchemaConfiguration schemaConfiguration)
    {
        _logDump = new();
        _hexDump = new();
        _serviceProvider = serviceProvider;
        _schemaConfiguration = schemaConfiguration;
    }

    public IIsoMessage Parse(byte[] messageBytes, IServiceProvider serviceProvider)
    {
        var mtiParser = (IMTIParser)_serviceProvider.GetService(typeof(IMTIParser));

        //Parsing MTI
        var mti = mtiParser.ParseMTI(messageBytes);
        var isoMessage = getMessage(mti, _schemaConfiguration);
        parse(isoMessage, messageBytes, _schemaConfiguration.SchemaEncoding);
        return isoMessage;
    }

    public IIsoMessage GetMTI(byte[] messageBytes)
    {
        var mtiParser = (IMTIParser)_serviceProvider.GetService(typeof(IMTIParser));

        //Parsing MTI
        var mti = mtiParser.ParseMTI(messageBytes);
        var isoMessage = getMessage(mti, _schemaConfiguration);
        return isoMessage;
    }



    public byte[] ToBytes(IIsoMessage message, DataEncoding encoding)
    {
        var mtiParser = (IMTIParser)_serviceProvider.GetService(typeof(IMTIParser));
        return ((IsoRequest)message).ByteMap.GetDataByte(encoding, mtiParser);
    }

    private IsoRequest getMessage(string mti, SchemaConfiguration schemaConfiguration)
    {
        var isoMessage = new IsoRequest(mti, schemaConfiguration.SchemaEncoding, schemaConfiguration.ByteMapLength);

        //setting fields from configuration
        foreach (var field in schemaConfiguration.Fields)
        {
            var isoField = getField(field, isoMessage.ByteMap, _serviceProvider);
            isoMessage.Fields.Add(field.Index, isoField);
        }

        //Setting Message as per configurations
        var messageConfiguration = schemaConfiguration.Messages.Where(c => c.Alias == isoMessage.MessageType).FirstOrDefault();
        if (messageConfiguration != null)
        {
            isoMessage.IsAdviceMessage = messageConfiguration.IsAdviceMessage;
            isoMessage.IsRepeatMessage = messageConfiguration.IsRepeatMessage;
            isoMessage.IsResponseMessage = messageConfiguration.IsResponseMessage;
            isoMessage.IsNetworkMessage = messageConfiguration.IsNetworkMessage;
            isoMessage.IsFinancialTransaction = messageConfiguration.IsNetworkMessage;
        }

        return isoMessage;
    }

    public IIsoMessage GetEmptyMessage(string mti, SchemaConfiguration schemaConfiguration)
    {
        return getMessage(mti, schemaConfiguration);
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

                "Variable" => new IsoBaseVariableLengthField(field.Name, (short)field.SizeInt, field.Index, byteMap, serviceProvider, field.DataEncoding, field.HeaderLengthEncoding),
                "TagValueSubField" => (IIsoField)new TagValueSubField(field.Name, (short)field.SizeInt, field.Index, byteMap, serviceProvider, field.DataEncoding),
                "Fixed" => new IsoAlphaNumericFixedField(field.Name, (short)field.SizeInt, field.Index, byteMap, serviceProvider),
                "ProcessingCode" => (IIsoField)new ProcessingCodeDataField(field.Name, (short)field.SizeInt, field.Index, byteMap, serviceProvider, field.DataEncoding),
                "CardholderBillingConversionRate" => (IIsoField)new CardHolderBillingConversionRateField(field.Name, (short)field.SizeInt, field.Index, byteMap, serviceProvider, field.DataEncoding),
                "PosEntryMode" => (IIsoField)new POSEntryModeField(field.Name, (short)field.SizeInt, field.Index, byteMap, serviceProvider, field.DataEncoding),
                "CardAcceptorNameLocation" => (IIsoField)new CardAcceptorNameLocationField(field.Name, (short)field.SizeInt, field.Index, byteMap, serviceProvider, field.DataEncoding),
                "ReplacementAmounts" => (IIsoField)new ReplacementAmountsField(field.Name, (short)field.SizeInt, field.Index, byteMap, serviceProvider, field.DataEncoding),
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
        var encoder = _serviceProvider.GetKeyedService<IEncoderFormator>(encoding.ToString());

        StringBuilder _logDump = new();
        _logDump.Append($"Start converting byte array to ISOMessage MTI: {isoRequest.MessageType}{Environment.NewLine}");
        _logDump.Append($"Raw byte array is{Environment.NewLine} {BitConverter.ToString(messageBytes)}{Environment.NewLine}");

        Dictionary<int, IIsoField> fields = isoRequest.Fields;
        int offset = skipBytes + 4;
        multiplyer = 1;
        if (encoding == DataEncoding.ASCII) multiplyer = 2;

        for (int i = 0; i < isoRequest.ByteMapLength; i++)
        {

            int v;
            if (i > 0)
            {
                var pByteMap = isoRequest.ByteMap.getBitMap(i - 1);
                v = pByteMap[0] >> 7 & 0x01;
            }
            else
            {
                v = 1;
            }

            if (v == 0x01)
            {
                b = dataByte.GetByteSlice(8 * multiplyer, offset);
                var str = encoder.Encode(b);
                if (encoding == DataEncoding.ASCII)
                    b = ByteHelper.convertHexToBinaryUsingConvert(str);
            }
            else
            {
                break;
            }
            isoRequest.ByteMap.SetBitMap(i, b);
            offset += 8 * multiplyer;
        }
        return offset;
    }

    private int parseData(IsoRequest isoRequest, byte[] messageBytes, DataEncoding encoding, int offset)
    {
        var i = 0;
        try
        {
            var dataByte = messageBytes;
            int maxElements = isoRequest.ByteMapLength * 64;
            Dictionary<int, IIsoField> fields = isoRequest.GetFieldList();

            for (int l = isoRequest.ByteMapLength; l < maxElements; l++)
            {
                i = l;
                int len = 0;
                int bitMapIndex = l / 64;



                if (i <= 64 && (isoRequest.ByteMap.getBitMap(0)[(i - 1) / 8] >> 8 - (i - 8 * ((i - 1) / 8)) & 0x01) == 0x01)
                {
                    if (!fields.ContainsKey(l))
                        continue;

                    len = fields[l].SetValueBytes(dataByte, offset);
                    offset += len;
                    _logDump.Append($"{Environment.NewLine}DE-[{l}]:[{fields[l].Name}]=> [{fields[l].LogDump()}]");
                }
                else if (i > 64 && (isoRequest.ByteMap.getBitMap(bitMapIndex)[(i - 64 * bitMapIndex - 1) / 8] >> 8 - (i - 64 * bitMapIndex - 8 * ((i - 64 * bitMapIndex - 1) / 8)) & 0x01) == 0x01)
                {

                    if (!fields.ContainsKey(l))
                        continue;

                    len = fields[l].SetValueBytes(dataByte, offset);
                    offset += len;
                    _logDump.Append($"{Environment.NewLine}DE-[{l}]:[{fields[l].Name}]=> [{fields[l].LogDump()}]");
                }
            }
            _logDump.AppendLine($"End Data Message{Environment.NewLine}");
            return 1;
        }
        catch (Exception ex)
        {
            _logDump.Append($"Error occurred while Parsing. the Last Element parsed is {i} {Environment.NewLine} error: {ex.ToString()} {Environment.NewLine}");
            throw;
        }
    }
}