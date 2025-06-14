﻿using Acubec.Payments.ISO8583Parser.Helpers;
using Acubec.Payments.ISO8583Parser.Interfaces;

namespace Acubec.Payments.ISO8583Parser.DataTypes;

public class IsoAlphaNumericFixedField : IsoBaseFixedField
{
    public IsoAlphaNumericFixedField(string name, int length, int messageIndex, ByteMaps byteMap, IServiceProvider serviceProvider, DataEncoding messageEncoding, DataEncoding dataEncoding, DataEncoding headerLengthEncoding)
          : base(name, length, messageIndex, byteMap, serviceProvider, messageEncoding, dataEncoding, headerLengthEncoding)
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
            _value = value.TryRightPadding(Length, ' ');

            if (!string.IsNullOrEmpty(_value))
            {
                IsSet = true;
                _byteMap.SetValue(_messageIndex, this);
            }
        }
    }
}
