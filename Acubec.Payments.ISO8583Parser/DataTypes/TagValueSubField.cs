using Acubec.Payments.ISO8583Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583Parser.DataTypes;

internal class TagValueSubField : IsoBaseVariableLengthField
{

    readonly Dictionary<string, string> _subField = new Dictionary<string, string>();

    public TagValueSubField(string name, short length, int messageIndex, ByteMaps byteMap, IServiceProvider serviceProvider, DataEncoding dataEncoding = DataEncoding.ASCII, DataEncoding headerLengthEncoding = DataEncoding.ASCII, short bitMapLength = 2) : base(name, length, messageIndex, byteMap, serviceProvider, dataEncoding, headerLengthEncoding,bitMapLength)
    {
    }

    public Dictionary<string, string> SubFields { get { return _subField; } }

    public override int SetValueBytes(byte[] dataByte, int offset)
    {
        var s = base.SetValueBytes(dataByte, offset);

        foreach (var field in _subField)
        {
            if (!string.IsNullOrEmpty(field.Value))
            {
                _value = _value + field.Key + field.Value.Length.ToString().PadLeft(3, '0') + field.Value;
            }
        }

        return s;
    }

    public override string Value
    {
        get
        {
            GetValueString();
            return _value;
        }
        set
        {
            _subField.Clear();
            _value = value;
        }
    }

    public override bool IsSet
    {
        get
        {
            var isSet = false;
            foreach (var field in _subField)
            {
                if (!string.IsNullOrEmpty(field.Value))
                {
                    isSet = true;
                }
            }

            return isSet;
        }
        protected set { }
    }

    private string GetValueString()
    {
        var value = "";

        foreach (var field in _subField)
        {
            if (!string.IsNullOrEmpty(field.Value))
            {
                value = value + field.Key + field.Value.Length.ToString().PadLeft(3, '0') + field.Value;
            }
        }
        _value = value;
        return value;
    }

}
