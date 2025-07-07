using Acubec.Payments.ISO8583.Parser.Implementation;
using Acubec.Payments.ISO8583.Parser.Interfaces;
using Acubec.Payments.ISO8583.Parser.Types;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583.Parser.Fields;

public abstract class IsoBaseFixedField : BaseIsoField, IIsoField
{
    #region Public Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="IsoFieldFixed"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="length">The length.</param>
    /// <param name="isMandatory">if set to <c>true</c> [is mandatory].</param>
    public IsoBaseFixedField(Field field, ByteMaps byteMaps, DataEncoding messageEncoding, IServiceProvider serviceProvider)
        : base(field, byteMaps, messageEncoding, serviceProvider)
    {
        Type = IsoFieldType.Fixed;
    }

    #endregion Public Constructors

    #region Public Methods

    public override string ToString()
    {
        if (Value.Length != _length)
        {
            //throw new ArgumentException();
        }

        var encoder = _serviceProvider.GetKeyedService<IEncoder>(_encoding.ToString());
        var messageEncoder = _serviceProvider.GetKeyedService<IEncoder>(base._messageEncoding.ToString());

        return Value;
    }

    public override int SetValueBytes(ReadOnlySpan<byte> dataByte, int offset)
    {
        var encoder = _serviceProvider.GetKeyedService<IEncoder>(_encoding.ToString());
        var length = _field.SizeInt;

        if (_encoding == DataEncoding.HEX)
        {
            length = length / 2;
            if (length % 2 != 0) ++length;
        }

        var bytes = dataByte.GetByteSlice(length, offset);

        Value = encoder.Encode(bytes).ToString();
        _byteMap.SetValue(MessageIndex, this);
        return length;
    }


    public override ReadOnlySpan<byte> ValueSpans 
    {
        get
        {
            if (_length == 0) _length = base._field.SizeInt;

            if (Value.Length != _length)
            {
                //throw new ArgumentException();
            }

            var encoder = _serviceProvider.GetKeyedService<IEncoder>(_encoding.ToString())!;
            var result =  encoder.Decode(Value);



            if(_encoding == DataEncoding.HEX)
            {
                var length = _field.SizeInt / 2;

                if (length % 2 != 0) 
                    result = ByteHelper.Combine([(byte)0],result);
            }
            return result;
        }
    }
    #endregion Public Methods
}
