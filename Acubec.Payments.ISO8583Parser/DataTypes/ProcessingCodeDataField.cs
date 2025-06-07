using Acubec.Payments.ISO8583Parser.Interfaces;

namespace Acubec.Payments.ISO8583Parser.DataTypes;

public class ProcessingCodeDataField : IsoBaseFixedField
{
    private string _accountType;
    private string _accountFrom;
    private string _accountTo;

    public string AccountType
    {
        get => _accountType;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (value.Length != 2) throw new Exception("Invalid Processing Code");

            _accountType = value;
        }

    }
    public string AccountFrom
    {
        get => _accountFrom;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (value.Length != 2) throw new Exception("Invalid Processing Code");

            _accountFrom = value;
        }

    }
    public string AccountTo
    {
        get => _accountTo;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (value.Length != 2) throw new Exception("Invalid Processing Code");

            _accountTo = value;
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

            if (value.Length != 6) throw new Exception("Invalid Processing Code");

            AccountType = value[0..2];
            AccountFrom = value[2..4];
            AccountTo = value[4..];
            _value = value;
            base.IsSet = true;
        }

    }

    public ProcessingCodeDataField(string name, int length, int messageIndex, 
            ByteMaps byteMap, IServiceProvider serviceProvider, DataEncoding messageEncoding, DataEncoding dataEncoding , DataEncoding headerLengthEncoding) : base(name, length, messageIndex, byteMap, serviceProvider,messageEncoding, dataEncoding, headerLengthEncoding)
    {
    }

    public override string ToString()
    {
        if (AccountType != null && AccountFrom != null && AccountTo != null)
        {
            Value = AccountType + AccountFrom + AccountTo;
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
            AccountType = _value[0..2];
            AccountFrom = _value[2..4];
            AccountTo = _value[4..];
        }
        return length;
    }
    public override string LogDump()
    {
        return $@"{base.LogDump()}
            AccountType:{_accountType}
            AccountFrom:{_accountFrom}
            AccountTo:{_accountTo}{Environment.NewLine}";

    }
}
