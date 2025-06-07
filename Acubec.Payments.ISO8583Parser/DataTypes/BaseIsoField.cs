using Microsoft.Extensions.DependencyInjection;

namespace Acubec.Payments.ISO8583Parser.Interfaces;
public enum DataEncoding
{
    ASCII,
    Binary,
    BinaryPlus,
    EBCDIC,
    HEX,
    CardholderBillingConversionRate,
    PosEntryMode,
    CardAcceptorNameLocation,
    ReplacementAmounts,
}

public abstract class BaseIsoField : IIsoField
{
    #region Fields
    protected int _length;
    protected string _value;
    protected ByteMaps _byteMap;
    protected readonly DataEncoding _messageEncoding;
    protected int _messageIndex;
    protected DataEncoding _encoding;
    protected IServiceProvider _serviceProvider;
    protected DataEncoding _headerLengthEncoding;
    #endregion Fields

    #region Protected Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="IsoField"/> class.
    /// </summary>
    /// <param name="name">Name of the field.</param>
    /// <param name="type">Type of the field.</param>
    /// <param name="isMandatory">if set to <c>true</c> [is mandatory].</param>

    /// <summary>
    /// Initializes a new instance of the <see cref="IsoField"/> class.
    /// </summary>
    /// <param name="name">Name of the field.</param>
    /// <param name="type">Type of the field.</param>
    /// <param name="length">Length of the message.</param>
    /// <param name="isMandatory">if set to <c>true</c> [is mandatory].</param>
    protected BaseIsoField(string name, IsoTypes type, int length, int messageIndex
        , ByteMaps byteMap, IServiceProvider serviceProvider, DataEncoding messageEncoding,
        DataEncoding dataEncoding , DataEncoding headerLengthEncoding)
    {
        Name = name;
        Type = type;
        _length = length;
        _messageIndex = messageIndex;
        _byteMap = byteMap;
        _messageEncoding = messageEncoding;
        _encoding = dataEncoding;
        _serviceProvider = serviceProvider;
        _headerLengthEncoding = headerLengthEncoding;
    }

    #endregion Protected Constructors

    #region Public Properties

    public virtual bool IsSet { get; protected set; }

    public bool Mask { get; set; }

    public int Length { get { return _length; } protected set { _length = value; } }

    public string Name { get; protected set; }

    public IsoTypes Type { get; protected set; }

    public abstract string Value { get; set; }

    #endregion Public Properties

    #region Public Methods

    public virtual byte[] GetValueBytes()
    {
        var encoder = _serviceProvider.GetKeyedService<IEncoderFormator>(_encoding.ToString());
        return encoder.Decode(ToString());
    }

    public virtual string LogDump()
    {
        var encoder = _serviceProvider.GetKeyedService<IEncoderFormator>(_encoding.ToString());
        var value = _encoding == DataEncoding.Binary ? BitConverter.ToString(encoder.Decode(Value)): Value;
        if (Mask)
        {
            if (value?.Length > 4)
            {
                return string.Format($"****-{value.Substring(value.Length - 4)}");
            }
            else
            {
                return "****";
            }
        }
        return value;
    }

    public abstract override string ToString();
    public abstract int SetValueBytes(Span<byte> dataByte, int offset);

    public int MessageIndex { get { return _messageIndex; } }

    #endregion Public Methods
}
