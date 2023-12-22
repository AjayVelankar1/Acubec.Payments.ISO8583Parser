using Acubec.Payments.ISO8583Parser.DataTypes;
using Acubec.Payments.ISO8583Parser.Definitions;
using Acubec.Payments.ISO8583Parser.Helpers;
using Acubec.Payments.ISO8583Parser.Interfaces;
using Acubec.Payments.ISO8583Parser.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583Parser;

public sealed class ISO8583MessageParser
{
    private readonly IServiceProvider _serviceProvider;

    public ISO8583MessageParser(IServiceProvider serviceProvider)
    {
        this._serviceProvider = serviceProvider;
    }

    public IIsoMessage Parse(SchemaConfiguration schemaConfiguration, byte[] messageBytes)
    {
        var mtiParser = (IMTIParser)_serviceProvider.GetService(typeof(IMTIParser));
        var mti = mtiParser.ParseMTI(messageBytes);
        
        var isoMessage = new IsoRequest(mti, schemaConfiguration.SchemaEncoding);
        foreach (var field in schemaConfiguration.Fields)
        {
            isoMessage.Fields[field.Index] = getField(field, isoMessage.ByteMap);
        }
        
        var messageConfiguration = schemaConfiguration.Messages.Where(c => c.Alias == isoMessage.MessageType).FirstOrDefault();
        isoMessage.IsAdviceMessage = messageConfiguration.IsAdviceMessage;
        isoMessage.IsRepeatMessage= messageConfiguration.IsRepeatMessage;
        isoMessage.IsResponseMessage = messageConfiguration.IsResponseMessage;
        isoMessage.IsNetworkMessage = messageConfiguration.IsNetworkMessage;
        isoMessage.IsFinancialTransaction = messageConfiguration.IsNetworkMessage;

        return isoMessage;
    }

    private IIsoField getField(Field field, ByteMap byteMap)
    {
        try
        {
            var customFieldFactory = (ICustomFiledFactory)_serviceProvider.GetService(typeof(ICustomFiledFactory));

            var isoFiled = field.Type switch
            {
                "Fixed" => (IIsoField)new IsoAlphaNumericFixedField(field.Name, (short)field.SizeInt, field.Index, byteMap),
                "Variable" => (IIsoField)new IsoBaseVariableLengthField(field.Name, (short)field.SizeInt, field.Index, byteMap, field.DataEncoding),
                "TagValueSubField" => (IIsoField)new TagValueSubField(field.Name, (short)field.SizeInt, field.Index, byteMap, field.DataEncoding),
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
}