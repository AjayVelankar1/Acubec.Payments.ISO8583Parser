using Acubec.Payments.ISO8583Parser.Helpers;
using Acubec.Payments.ISO8583Parser.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Acubec.Payments.ISO8583Parser.DataTypes;

public abstract class IsoBaseFixedField : BaseIsoField, IIsoField
{
    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="IsoFieldFixed"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="length">The length.</param>
    /// <param name="isMandatory">if set to <c>true</c> [is mandatory].</param>
    public IsoBaseFixedField(string name, int length, int messageIndex, ByteMaps byteMap, IServiceProvider serviceProvider, DataEncoding dataEncoding = DataEncoding.ASCII)
        : base(name, IsoTypes.Fixed, length, messageIndex, byteMap, serviceProvider, dataEncoding)
    {

    }

    #endregion Public Constructors

    #region Public Methods

    public override string ToString()
    {
        if (Value.Length != _length)
        {
            throw new ArgumentException();
        }

        return Value;
    }

    public override int SetValueBytes(byte[] dataByte, int offset)
    {
        var encoder = _serviceProvider.GetKeyedService<IEncoderFormator>(_encoding.ToString());
        var bytes = dataByte.GetByteSlice(Length, offset);
        Value = encoder.Encode(bytes);
        _byteMap.SetValue(MessageIndex, this);
        return Length;
    }

    #endregion Public Methods
}
