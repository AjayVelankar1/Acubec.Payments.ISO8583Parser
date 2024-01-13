using Acubec.Payments.ISO8583Parser.Helpers;
using Acubec.Payments.ISO8583Parser.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583Parser.DataTypes;

internal class IsoBaseVariableLengthField : BaseIsoField, IIsoField
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
        DataEncoding dataEncoding = DataEncoding.ASCII, 
        DataEncoding headerLengthEncoding = DataEncoding.ASCII,
        short bitMapLength = 2)
        : base(name, IsoTypes.LLVar, length, messageIndex, byteMap, serviceProvider, dataEncoding)
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

        //var encoder = _serviceProvider.GetKeyedService<IEncoderFormator>(_encoding.ToString());
        //var lengthEncoder = _serviceProvider.GetKeyedService<IEncoderFormator>(_headerLengthEncoding.ToString());
        //string strLength =  _value.Length.ToString(CultureInfo.InvariantCulture).PadLeft(_orignalLength, '0')
        //var length = lengthEncoder.Encode(strLength);

        return Value.Length.ToString(CultureInfo.InvariantCulture).PadLeft(_originalLength, '0') + Value;
    }

    public override int SetValueBytes(byte[] dataByte, int offset)
    {
        var encoder = _serviceProvider.GetKeyedService<IEncoderFormator>(_encoding.ToString());
        var lengthEncoder = _serviceProvider.GetKeyedService<IEncoderFormator>(_headerLengthEncoding.ToString());

        var bytes = dataByte.GetByteSlice( _length, offset);
        string strLen = lengthEncoder.Encode(bytes);
        var dataLength = Convert.ToInt32(strLen, CultureInfo.InvariantCulture);
        bytes = dataByte.GetByteSlice(dataLength, offset + _length);
        Value = encoder.Encode(bytes);

        //string strLen = Encoding.UTF8.GetString(dataByte, offset, _length);
        //var dataLength = Convert.ToInt32(strLen, CultureInfo.InvariantCulture);
        //Value = Encoding.UTF8.GetString(dataByte, offset + _bitMapLength+ 1, dataLength);

        return dataLength + Length;
    }


    #endregion Public Methods
}
