using Acubec.Payments.ISO8583Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583Parser.DataTypes;

internal class IsoBaseVariableLengthField : BaseIsoField, IIsoField
{
    private readonly short _bitMaplength;

    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="IsoFieldLL"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="isMandatory">if set to <c>true</c> [is mandatory].</param>
    public IsoBaseVariableLengthField(string name, short length, int messageIndex, ByteMap byteMap, DataEncoding dataEncoding = DataEncoding.ASCII, short bitMaplength = 2)
        : base(name, IsoTypes.LLVar, length, messageIndex, byteMap, dataEncoding)
    {
        _bitMaplength = bitMaplength;
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

        return Value.Length.ToString(CultureInfo.InvariantCulture).PadLeft(_bitMaplength, '0') + Value;
    }

    public override int SetValueBytes(byte[] dataByte, int offset)
    {

        string strLen = Encoding.UTF8.GetString(dataByte, offset, _bitMaplength);
        Length = Convert.ToInt32(strLen, CultureInfo.InvariantCulture);
        Value = Encoding.UTF8.GetString(dataByte, offset + _bitMaplength, Length);

        return Value.Length + _bitMaplength;
    }


    #endregion Public Methods
}
