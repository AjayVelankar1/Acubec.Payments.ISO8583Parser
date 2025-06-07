using Acubec.Payments.ISO8583Parser.Interfaces;

namespace Acubec.Payments.ISO8583Parser.DataTypes;

public class ReplacementAmountsField : IsoBaseFixedField
{
    private string _actualTransactionAmounts;
    private string _actualSettlementAmounts;
    private string _actualTransactionFeeAmounts;
    private string _actualSettlementFeeAmounts;
    public string ActualTransactionAmounts
    {
        get => _actualTransactionAmounts;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (value.Length != 12) throw new Exception("Invalid Actual Transaction Amounts");

            _actualTransactionAmounts = value;
        }

    }

    public string ActualSettlementAmounts
    {
        get => _actualSettlementAmounts;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (value.Length != 12) throw new Exception("Invalid Actual Settlement Amounts");

            _actualSettlementAmounts = value;
        }

    }
    public string ActualTransactionFeeAmounts
    {
        get => _actualTransactionFeeAmounts;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (value.Length != 9) throw new Exception("Invalid Actual Transaction Fee Amounts");

            _actualTransactionFeeAmounts = value;
        }

    }
    public string ActualSettlementFeeAmounts
    {
        get => _actualSettlementFeeAmounts;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (value.Length != 9) throw new Exception("Invalid Actual Settlement Fee Amounts");

            _actualSettlementFeeAmounts = value;
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

            if (value.Length != 42) throw new Exception("Invalid Replacement Amounts");

            ActualTransactionAmounts = value[0..12];
            ActualSettlementAmounts = value[12..24];
            ActualTransactionFeeAmounts = value[24..33];
            ActualSettlementFeeAmounts = value[33..];
            _value = value;
            base.IsSet = true;
        }
    }

    public ReplacementAmountsField(string name, int length, int messageIndex, ByteMaps byteMap, IServiceProvider serviceProvider, DataEncoding messageEncoding, DataEncoding dataEncoding , DataEncoding headerLengthEncoding) : base(name, length, messageIndex, byteMap, serviceProvider,messageEncoding, dataEncoding,headerLengthEncoding)
    {
    }
    public override string ToString()
    {
        if (ActualTransactionAmounts != null && ActualSettlementAmounts != null && ActualTransactionFeeAmounts != null && ActualSettlementFeeAmounts != null)
        {
            Value = ActualTransactionAmounts + ActualSettlementAmounts + ActualTransactionFeeAmounts + ActualSettlementFeeAmounts;
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
            ActualTransactionAmounts = _value[0..12];
            ActualSettlementAmounts = _value[12..24];
            ActualTransactionFeeAmounts = _value[24..33];
            ActualSettlementFeeAmounts = _value[33..];
        }
        return length;
    }
    public override string LogDump()
    {
        return $@"{base.LogDump()}
            ActualTransactionAmounts:{_actualTransactionAmounts}
            ActualSettlementAmounts:{_actualSettlementAmounts}
            ActualTransactionFeeAmounts:{_actualTransactionFeeAmounts}
            ActualSettlementFeeAmounts:{_actualSettlementFeeAmounts}
            {Environment.NewLine}";
    }
}
