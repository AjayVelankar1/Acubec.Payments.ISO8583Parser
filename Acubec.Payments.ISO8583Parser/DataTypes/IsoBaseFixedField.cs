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
    public IsoBaseFixedField(string name, int length, int messageIndex, ByteMaps byteMap, IServiceProvider serviceProvider, DataEncoding messageEncoding, DataEncoding dataEncoding = DataEncoding.ASCII, DataEncoding headerLengthEncoding = DataEncoding.ASCII)
        : base(name, IsoTypes.Fixed, length, messageIndex, byteMap, serviceProvider, messageEncoding, dataEncoding, headerLengthEncoding)
    {

    }

    #endregion Public Constructors

    #region Public Methods

    public override string ToString()
    {
        if (Value.Length != _length)
        {
            //throw new ArgumentException();
        }

        var encoder = _serviceProvider.GetKeyedService<IEncoderFormator>(_encoding.ToString());
        var messageEncoder = _serviceProvider.GetKeyedService<IEncoderFormator>(base._messageEncoding.ToString());

        return messageEncoder.Encode(encoder.Decode(Value));
    }

    public override int SetValueBytes(Span<byte> dataByte, int offset)
    {
        var encoder = _serviceProvider.GetKeyedService<IEncoderFormator>(_encoding.ToString());
        var length = _length;
        
        if (_headerLengthEncoding == DataEncoding.HEX || _headerLengthEncoding == DataEncoding.Binary)
        {
            length = Length / 2;
            if (Length % 2 != 0) ++length;
        }


        var bytes = dataByte.GetByteSlice(length, offset);
        Value = encoder.Encode(bytes);
        _byteMap.SetValue(MessageIndex, this);
        return length;
    }

    #endregion Public Methods
}
