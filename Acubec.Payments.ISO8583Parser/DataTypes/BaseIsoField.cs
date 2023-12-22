using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acubec.Payments.ISO8583Parser.Helpers;

namespace Acubec.Payments.ISO8583Parser.Interfaces;
public enum DataEncoding
{
    ASCII,
    Binary,
    ABSADIC,
    HEX
}

public abstract class BaseIsoField: IIsoField
{
    #region Fields
    protected int _length;
    protected string _value;
    protected ByteMap _byteMap;
    protected int _messageIndex;
    protected DataEncoding _encoding;

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
    protected BaseIsoField(string name, IsoTypes type, int length, int messageIndex, ByteMap byteMap, DataEncoding dataEncoding = DataEncoding.ASCII)
    {
        Name = name;
        Type = type;
        _length = length;
        _messageIndex = messageIndex;
        _byteMap = byteMap;
        _encoding = dataEncoding;
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
        if (_encoding == DataEncoding.ASCII)
            return Encoding.ASCII.GetBytes(ToString());

        else if (_encoding == DataEncoding.Binary)
        {
            var bytes = ByteHelper.GetBytesFromHexString(ToString());
            ByteHelper.CombineBytes(Encoding.ASCII.GetBytes(bytes.Length.ToString().PadLeft(3, '0')), bytes);
            return bytes;
        }


        return Encoding.ASCII.GetBytes(ToString());
    }

    public virtual string LogDump()
    {
        if (Mask)
        {
            if (Value?.Length > 4)
            {
                string.Format($"****-{Value.Substring(Value.Length - 4)}");
            }
            else
            {
                return "****";
            }
        }
        return Value;
    }

    public abstract override string ToString();
    public abstract int SetValueBytes(byte[] dataByte, int offset);

    public int MessageIndex { get { return _messageIndex; } }

    #endregion Public Methods
}
