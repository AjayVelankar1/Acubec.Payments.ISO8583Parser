using Acubec.Payments.ISO8583.Parser.Implementation;
using Acubec.Payments.ISO8583.Parser.Interfaces;
using Acubec.Payments.ISO8583.Parser.Types;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583.Parser.Fields;

public class IsoVariableLengthField : BaseIsoField, IIsoField
{
    //private readonly short _bitMapLength;
    private int _originalLength;
    protected DataEncoding _headerLengthEncoding;
    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="IsoFieldLL"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="isMandatory">if set to <c>true</c> [is mandatory].</param>
    public IsoVariableLengthField(Field field, ByteMaps byteMaps, DataEncoding messageEncoding, IServiceProvider serviceProvider)
        : base(field, byteMaps, messageEncoding, serviceProvider)
    {
        _originalLength = field.SizeInt;
        _headerLengthEncoding = field.HeaderLengthEncoding;
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

    public override ReadOnlySpan<byte> ValueSpans
    {
        get
        {
            if (!IsSet)
            {
                return Span<byte>.Empty;
            }

            var encoder = _serviceProvider.GetKeyedService<IEncoder>(_encoding.ToString())!;
            var lengthEncoder = _serviceProvider.GetKeyedService<IEncoder>(_headerLengthEncoding.ToString())!;
            var messageEncoder = _serviceProvider.GetKeyedService<IEncoder>(base._messageEncoding.ToString());
            var length = _value.Length.ToString();

            if (_headerLengthEncoding == DataEncoding.ASCII)
                length = length.ToString(CultureInfo.InvariantCulture).PadLeft(_originalLength, '0');


            var lengthString = lengthEncoder.Decode(length);
            var valueBytes = encoder.Decode(_value.AsSpan());
            var value = ByteHelper.Combine(lengthString, valueBytes).ToReadOnlySpan();
            return value;
        }
    }

    public override string ToString()
    {
        if (!IsSet)
        {
            return string.Empty;
        }

        //var lengthString = string.Empty;
        //var valueString = string.Empty;

        var encoder = _serviceProvider.GetKeyedService<IEncoder>(_encoding.ToString())!;
        var lengthEncoder = _serviceProvider.GetKeyedService<IEncoder>(_headerLengthEncoding.ToString())!;
        var messageEncoder = _serviceProvider.GetKeyedService<IEncoder>(base._messageEncoding.ToString())!;
        
        var lengthString = lengthEncoder.Decode(_value.Length.ToString());
        var valueBytes = encoder.Decode(_value.AsSpan());
        var value = ByteHelper.Combine(lengthString, valueBytes).ToReadOnlySpan();
        var valueString = messageEncoder.Encode(value);
        return valueString.ToString();
    }

    public override int SetValueBytes(ReadOnlySpan<byte> dataByte, int offset)
    {
        var encoder = _serviceProvider.GetKeyedService<IEncoder>(_encoding.ToString());
        var lengthEncoder = _serviceProvider.GetKeyedService<IEncoder>(_headerLengthEncoding.ToString());
        var length = _field.HeaderLength == 0 ? _field.SizeInt : _field.HeaderLength;
        
        if (_headerLengthEncoding == DataEncoding.Binary || _headerLengthEncoding == DataEncoding.BinaryPlus)
        {
            length = 1;
            //throw new Exception($"Invalid length for field {Name} at index {_messageIndex}");
        }


        var bytes = dataByte.GetByteSlice(length, offset);
        var strLen = lengthEncoder.Encode(bytes);

        var dataLength = Convert.ToInt32(strLen.ToString(), CultureInfo.InvariantCulture);

        if (_encoding == DataEncoding.HEX)
        {
            dataLength = dataLength / 2;
            if (dataLength % 2 != 0) ++dataLength;
        }

        bytes = dataByte.GetByteSlice(dataLength, offset + length);
        Value = encoder.Encode(bytes).ToString();
        return dataLength + length;
    }


    #endregion Public Methods
}


