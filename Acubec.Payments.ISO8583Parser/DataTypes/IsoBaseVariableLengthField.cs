using Acubec.Payments.ISO8583Parser.Definitions;
using Acubec.Payments.ISO8583Parser.Helpers;
using Acubec.Payments.ISO8583Parser.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;

namespace Acubec.Payments.ISO8583Parser.DataTypes;

public class IsoBaseVariableLengthField : BaseIsoField, IIsoField
{
    private readonly short _bitMapLength;
    private int _originalLength;
    protected DataEncoding _headerLengthEncoding;
    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="IsoFieldLL"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="isMandatory">if set to <c>true</c> [is mandatory].</param>
    public IsoBaseVariableLengthField(string name, short length, int messageIndex,
        ByteMaps byteMap,
        IServiceProvider serviceProvider,
        DataEncoding messageEncoding,
        DataEncoding dataEncoding,
        DataEncoding headerLengthEncoding,
        short bitMapLength = 2)
        : base(name, IsoTypes.LLVar, length, messageIndex, byteMap, serviceProvider, messageEncoding, dataEncoding, headerLengthEncoding)
    {
        _bitMapLength = bitMapLength;
        _originalLength = length;
        _headerLengthEncoding = headerLengthEncoding;
    }

    #endregion Public Constructors

    #region Public Methods

    /// <summary>
    /// Gets or sets the value of IsoFieldLL data element.
    /// </summary>
    /// <value>
    /// The value.
    /// </value>
    /// <exception cref="Exception"></exception>
    public override string Value
    {
        get { return IsSet ? _value : string.Empty; }
        set
        {
            _value = value;

            if (!string.IsNullOrEmpty(_value))
                IsSet = true;
            else
                IsSet = false;

            _byteMap.SetValue(_messageIndex, this);
        }
    }

    public override string ToString()
    {
        if (!IsSet)
        {
            return string.Empty;
        }

        var lengthString = string.Empty;
        var valueString = string.Empty;

        var encoder = _serviceProvider.GetKeyedService<IEncoderFormator>(_encoding.ToString());
        var lengthEncoder = _serviceProvider.GetKeyedService<IEncoderFormator>(_headerLengthEncoding.ToString());
        string strLength = _value.Length.ToString(CultureInfo.InvariantCulture).PadLeft(_length, '0');
        var messageEncoder = _serviceProvider.GetKeyedService<IEncoderFormator>(base._messageEncoding.ToString());

        if (_headerLengthEncoding == DataEncoding.HEX || _headerLengthEncoding == DataEncoding.Binary || _headerLengthEncoding == DataEncoding.BinaryPlus)
        {
            lengthString = BitConverter.ToString([(byte)Value.Length]);
        }
        else
        {
            lengthString = Value.Length.ToString(CultureInfo.InvariantCulture).PadLeft(_originalLength, '0');
        }

        if (_encoding == DataEncoding.HEX)
        {
            valueString = ByteHelper.GetHexRepresentation(encoder.Decode(Value));
        }
        else if (_encoding == DataEncoding.ASCII)
        {
            valueString = Value;
        }
        else
        {
            valueString = messageEncoder.Encode(encoder.Decode(Value));
        }
        return lengthString + valueString;
    }

    public override int SetValueBytes(Span<byte> dataByte, int offset)
    {
        var encoder = _serviceProvider.GetKeyedService<IEncoderFormator>(_encoding.ToString());
        var lengthEncoder = _serviceProvider.GetKeyedService<IEncoderFormator>(_headerLengthEncoding.ToString());
        var length = _length;
        if (_headerLengthEncoding == DataEncoding.HEX || _headerLengthEncoding == DataEncoding.Binary || _headerLengthEncoding == DataEncoding.BinaryPlus)
        {
            length = 1;
        }

        var bytes = dataByte.GetByteSlice(length, offset);
        string strLen = lengthEncoder.Encode(bytes);

        if (_headerLengthEncoding == DataEncoding.HEX || _headerLengthEncoding == DataEncoding.Binary 
                || _headerLengthEncoding == DataEncoding.BinaryPlus)
        {
            strLen = bytes[0].ToString();
        }

        var dataLength = Convert.ToInt32(strLen, CultureInfo.InvariantCulture);
        
        if (_headerLengthEncoding == DataEncoding.HEX || _headerLengthEncoding == DataEncoding.Binary)
        {
            dataLength = dataLength / 2;
            if (dataLength % 2 != 0) ++dataLength;
        }
        //strLen = dataLength.ToString(CultureInfo.InvariantCulture).PadLeft(_originalLength, '0');

        bytes = dataByte.GetByteSlice(dataLength, offset + length);
        Value = encoder.Encode(bytes);
        return dataLength + length;
    }


    #endregion Public Methods
}

