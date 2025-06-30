using Acubec.Payments.ISO8583.Parser.Implementation;
using Acubec.Payments.ISO8583.Parser.Types;

namespace Acubec.Payments.ISO8583.Parser.Fields;

public class IsoAlphaNumericFixedField : IsoBaseFixedField
{
    public IsoAlphaNumericFixedField(Field field, ByteMaps byteMaps, DataEncoding messageEncoding, IServiceProvider serviceProvider)
          : base(field, byteMaps, messageEncoding, serviceProvider)
    {
    }

    public override string Value
    {
        get
        {
            return IsSet ? _value : string.Empty;
        }

        set
        {
            if (string.IsNullOrEmpty(value)) return;
            //_value = value.TryRightPadding(Length, ' ');
            _value = value;
            if (!string.IsNullOrEmpty(_value))
            {
                IsSet = true;
                _byteMap.SetValue(_messageIndex, this);
            }
        }
    }

    
}

public class IsoNumericFixedField : IsoAlphaNumericFixedField
{
    public IsoNumericFixedField(Field field, ByteMaps byteMaps, DataEncoding messageEncoding, IServiceProvider serviceProvider)
          : base(field, byteMaps, messageEncoding, serviceProvider)
    {
    }

    public override string Value
    {
        get
        {
            return base.Value;
        }

        set
        {
            if (!value.All(char.IsDigit))
            {
                throw new Exception("Numeric field value must be numeric only.");
            }

            if (string.IsNullOrEmpty(value)) return;
            //_value = value.TryRightPadding(Length, ' ');
            _value = value;
            if (!string.IsNullOrEmpty(_value))
            {
                IsSet = true;
                _byteMap.SetValue(_messageIndex, this);
            }
        }
    }
}
