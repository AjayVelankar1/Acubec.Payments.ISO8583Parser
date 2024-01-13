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

internal abstract class IsoBaseFixedField : BaseIsoField, IIsoField
{
    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="IsoFieldFixed"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="length">The length.</param>
    /// <param name="isMandatory">if set to <c>true</c> [is mandatory].</param>
    public IsoBaseFixedField(string name, int length, int messageIndex, ByteMaps byteMap, IServiceProvider serviceProvider,  DataEncoding dataEncoding = DataEncoding.ASCII)
        : base(name, IsoTypes.Fixed, length, messageIndex, byteMap, serviceProvider, dataEncoding)
    {

    }

    #endregion Public Constructors

    #region Public Methods

    public override string ToString()
    {
        return Value;
    }

    public override int SetValueBytes(byte[] dataByte, int offset)
    {
        var encoder = _serviceProvider.GetKeyedService<IEncoderFormator>(_encoding.ToString());
        var bytes = dataByte.GetByteSlice(Length, offset);
        Value = encoder.Encode(bytes);
        _byteMap.SetValue(MessageIndex, this);
        return Length;

        //if (_encoding == DataEncoding.ASCII)
        //{
        //    Value = Encoding.ASCII.GetString(dataByte, offset, Length);
        //    _byteMap.SetValue(MessageIndex, this);
        //    return Length;
        //}
        //else if (_encoding == DataEncoding.ASCII)
        //{
        //    string strLen = Encoding.UTF8.GetString(dataByte, offset, Length);
        //    Length = Convert.ToInt32(strLen, CultureInfo.InvariantCulture);
        //    var bytes = new byte[Length];

        //    Array.Copy(dataByte, offset + 3, bytes, 0, Length);
        //    Value = ByteHelper.GetHexRepresentation(bytes);

        //    return bytes.Length + 3;
        //}

        //return 0;
    }

    #endregion Public Methods
}
