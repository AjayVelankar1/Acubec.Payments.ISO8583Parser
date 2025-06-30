using Acubec.Payments.ISO8583.Parser.Implementation;
using Acubec.Payments.ISO8583.Parser.Interfaces;
using Acubec.Payments.ISO8583.Parser.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583.Parser.Fields;

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
    protected readonly Field _field;
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
    protected BaseIsoField(Field field, ByteMaps byteMaps, DataEncoding messageEncoding, IServiceProvider serviceProvider)
    {
        _field = field;
        Name = field.Name;
        //Type = field.Type;
        _messageIndex = field.Index;
        _byteMap = byteMaps;
        _messageEncoding = messageEncoding;
        _encoding = field.DataEncoding;
        _serviceProvider = serviceProvider;
        _headerLengthEncoding = field.HeaderLengthEncoding;
    }

    #endregion Protected Constructors

    #region Public Properties

    public virtual bool IsSet { get; protected set; }

    public bool Mask { get; set; }

    public int Length { get { return _length; } protected set { _length = value; } }

    public string Name { get; protected set; }

    public IsoFieldType Type { get; protected set; }

    public abstract string Value { get; set; }

    #endregion Public Properties

    #region Public Methods

    public virtual ReadOnlySpan<Byte> GetValueBytes()
    {
        var encoder = _serviceProvider.GetKeyedService<IEncoder>(_encoding.ToString());
        return encoder.Decode(ToString().AsSpan());
    }

    public virtual string LogDump()
    {
        if (Mask)
        {
            if (_value?.Length > 4)
            {
                return string.Format($"****-{_value.Substring(_value.Length - 4)}");
            }
            else
            {
                return "****";
            }
        }
        return _value;
    }

    public abstract override string ToString();
    public abstract int SetValueBytes(ReadOnlySpan<byte> dataByte, int offset);

    public int MessageIndex { get { return _messageIndex; } }

    public abstract ReadOnlySpan<byte> ValueSpans { get; }

    #endregion Public Methods
}
