using Acubec.Payments.ISO8583Parser.Interfaces;

namespace Acubec.Payments.ISO8583Parser.DataTypes;

public class POSEntryModeField : IsoBaseFixedField
{
    private string _pOSTerminalPANEntryMode;
    private string _pOSTerminalPINEntryMode;

    public POSEntryModeField(string name, int length, int messageIndex, ByteMaps byteMap, IServiceProvider serviceProvider, DataEncoding messageEncoding, DataEncoding dataEncoding , DataEncoding headerLengthEncoding) 
        : base(name, length, messageIndex, byteMap, serviceProvider, messageEncoding, dataEncoding,headerLengthEncoding)
    {
    }
    public string POSTerminalPANEntryMode
    {
        get => _pOSTerminalPANEntryMode;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (value.Length != 2) throw new Exception("Invalid POS Terminal PAN Entry Mode");

            _pOSTerminalPANEntryMode = value;
        }

    }
    public string POSTerminalPINEntryMode
    {
        get => _pOSTerminalPINEntryMode;
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            if (value.Length != 1) throw new Exception("Invalid POS Terminal PIN Entry Mode");

            _pOSTerminalPINEntryMode = value;
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

            if (value.Length != 3) throw new Exception("Invalid Point Of Service(POS) Entry Mode");

            POSTerminalPANEntryMode = value[0..2];
            POSTerminalPINEntryMode = value[2..];
            _value = value;
            base.IsSet = true;
        }

    }
    public override string ToString()
    {
        if (POSTerminalPANEntryMode != null && POSTerminalPINEntryMode != null)
        {
            Value = POSTerminalPANEntryMode + POSTerminalPINEntryMode;
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
            POSTerminalPANEntryMode = _value[0..2];
            POSTerminalPINEntryMode = _value[2..];
        }
        return length;
    }
    public override string LogDump()
    {
        return $@"{base.LogDump()}
            POSTerminalPANEntryMode:{_pOSTerminalPANEntryMode}
            POSTerminalPINEntryMode:{_pOSTerminalPINEntryMode}{Environment.NewLine}";
    }
}
