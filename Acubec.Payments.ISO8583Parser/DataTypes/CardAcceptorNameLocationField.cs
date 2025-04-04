using Acubec.Payments.ISO8583Parser.Interfaces;

namespace Acubec.Payments.ISO8583Parser.DataTypes;

public class CardAcceptorNameLocationField : IsoBaseFixedField
{
    private string _cardAcceptorName;
    private string _spaceSurchargeFreeIndicator;
    private string _cardAcceptorCitySubMerchantInfo;
    private string _space;
    private string _cardAcceptorStateProvinceCountryCode;

    public CardAcceptorNameLocationField(string name, int length, int messageIndex, ByteMaps byteMap, IServiceProvider serviceProvider, DataEncoding dataEncoding , DataEncoding headerLengthEncoding) : base(name, length, messageIndex, byteMap, serviceProvider, dataEncoding, headerLengthEncoding)
    {
    }
    public string CardAcceptorName
    {
        get => _cardAcceptorName;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (value.Length != 22) throw new Exception("Invalid Card Acceptor Name");

            _cardAcceptorName = value;
        }

    }
    public string SpaceSurchargeFreeIndicator
    {
        get => _spaceSurchargeFreeIndicator;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (value.Length != 1) throw new Exception("Invalid Space/Surcharge-Free Indicator");

            _spaceSurchargeFreeIndicator = value;
        }

    }
    public string CardAcceptorCitySubMerchantInfo
    {
        get => _cardAcceptorCitySubMerchantInfo;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (value.Length != 13) throw new Exception("Invalid Card Acceptor City/Sub-Merchant Info");

            _cardAcceptorCitySubMerchantInfo = value;
        }

    }
    public string Space
    {
        get => _space;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (value.Length != 1) throw new Exception("Invalid Space");

            _space = value;
        }

    }
    public string CardAcceptorStateProvinceCountryCode
    {
        get => _cardAcceptorStateProvinceCountryCode;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (value.Length != 3) throw new Exception("Invalid Card Acceptor State/Province/Country Code");

            _cardAcceptorStateProvinceCountryCode = value;
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

            if (value.Length != 40) throw new Exception("Invalid Card Acceptor Name Location");

            CardAcceptorName = value[0..22];
            SpaceSurchargeFreeIndicator = value[22..23];
            CardAcceptorCitySubMerchantInfo = value[23..36];
            Space = value[36..37];
            CardAcceptorStateProvinceCountryCode = value[37..];
            _value = value;
            base.IsSet = true;
        }

    }
    public override string ToString()
    {
        if (CardAcceptorName != null && SpaceSurchargeFreeIndicator != null && CardAcceptorCitySubMerchantInfo != null && Space != null && CardAcceptorStateProvinceCountryCode != null)
        {
            Value = CardAcceptorName + SpaceSurchargeFreeIndicator + CardAcceptorCitySubMerchantInfo + Space + CardAcceptorStateProvinceCountryCode;
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
        if (base.IsSet)
        {
            CardAcceptorName = _value[0..22];
            SpaceSurchargeFreeIndicator = _value[22..23];
            CardAcceptorCitySubMerchantInfo = _value[23..36];
            Space = _value[36..37];
            CardAcceptorStateProvinceCountryCode = _value[37..];
        }
        return length;
    }

    public override string LogDump()
    {
        return $@"{base.LogDump()}
            CardAcceptorName:{_cardAcceptorName}
            SpaceSurchargeFreeIndicator:{_spaceSurchargeFreeIndicator}
             CardAcceptorCitySubMerchantInfo:{_cardAcceptorCitySubMerchantInfo}
            Space:{_space}
                 CardAcceptorStateProvinceCountryCode:{_cardAcceptorStateProvinceCountryCode}{Environment.NewLine}";
    }

}
