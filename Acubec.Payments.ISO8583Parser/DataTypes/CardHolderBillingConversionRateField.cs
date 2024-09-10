using Acubec.Payments.ISO8583Parser.Interfaces;

namespace Acubec.Payments.ISO8583Parser.DataTypes;

public class CardHolderBillingConversionRateField : IsoBaseFixedField
{
    private string _decimalIndicator;
    private string _cardholderBillingConversionRate;

    public CardHolderBillingConversionRateField(string name, int length, int messageIndex, ByteMaps byteMap, IServiceProvider serviceProvider, DataEncoding dataEncoding = DataEncoding.ASCII) : base(name, length, messageIndex, byteMap, serviceProvider, dataEncoding)
    {
    }
    public string DecimalIndicator
    {
        get => _decimalIndicator;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (value.Length != 1) throw new Exception("Invalid Decimal Indicator");

            _decimalIndicator = value;
        }

    }
    public string CardholderBillingConversionRate
    {
        get => _cardholderBillingConversionRate;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (value.Length != 7) throw new Exception("Invalid CardHolder Billing Conversion Rate");

            _cardholderBillingConversionRate = value;
        }

    }
    public override string Value
    {
        get
        {
            return _value;

        }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (value.Length != 8) throw new Exception("Invalid CardHolder Billing Conversion Rate");

            DecimalIndicator = value[0..1];
            CardholderBillingConversionRate = value[1..];
            _value = value;
            base.IsSet = true;
        }

    }
    public override string ToString()
    {
        if (DecimalIndicator != null && CardholderBillingConversionRate != null)
        {
            Value = DecimalIndicator + CardholderBillingConversionRate;
        }

        if (Value.Length != _length)
        {
            throw new ArgumentException();
        }
        return Value;
    }
    public override byte[] GetValueBytes()
    {
        var length = base.GetValueBytes();
        /*
         This will set Value filed as 000000
         */

        if (base.IsSet)
        {
            DecimalIndicator = _value[0..1];
            CardholderBillingConversionRate = _value[1..];
        }
        return length;
    }
    public override string LogDump()
    {
        return $@"{base.LogDump()}
            DecimalIndicator:{_decimalIndicator}
            CardholderBillingConversionRate:{_cardholderBillingConversionRate}{Environment.NewLine}";
    }
}