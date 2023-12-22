using System.Text;
using Acubec.Payments.ISO8583Parser.Helpers;
using Acubec.Payments.ISO8583Parser.Interfaces;

namespace Acubec.Payments.ISO8583Parser;

public sealed class ByteMap
{
    public ByteMap(string messageType)
    {
        _primaryBitMap = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
        _secondaryBitMap = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };

        _dictionary = new SortedDictionary<int, IIsoField>();
        _headerMAP = Encoding.ASCII.GetBytes(messageType);


    }

    private byte[] _primaryBitMap;
    private byte[] _secondaryBitMap;

    private readonly byte[] _headerMAP;
    private bool _isSecondaryBitmapOn;
    private readonly SortedDictionary<int, IIsoField> _dictionary;
    public byte[] PrimaryBitMap { get { return _primaryBitMap; } }
    public byte[] SecondaryBitMap { get { return _secondaryBitMap; } }

    public void resetBitMap()
    {
        _primaryBitMap = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
        _secondaryBitMap = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
    }

    public void SetValue(int position, IIsoField value)
    {
        if (value.Value == "" || value.Value == null) return;
        if (value.IsSet)
        {
            if (!_dictionary.ContainsKey(position))
            {
                _dictionary.Add(position, value);
            }
            else
            {
                _dictionary[position] = value;
            }

            if (position > 64)
            {
                _primaryBitMap[0] |= 0x80;
                _isSecondaryBitmapOn = true;
                position -= 64;
                _secondaryBitMap[(position - 1) / 8] |= (byte)(0x80 >> (position - 1) % 8);

            }
            else
            {
                _primaryBitMap[(position - 1) / 8] |= (byte)(0x80 >> (position - 1) % 8);
            }
        }
        else
        {
            if (position > 64)
            {
                position -= 64;
                _secondaryBitMap[(position - 1) / 8] *= (byte)(0x80 << (position - 1) % 8);

            }
            else
            {
                _primaryBitMap[(position - 1) / 8] *= (byte)(0x80 << (position - 1) % 8);
            }
        }

    }

    public byte[] GetDataByte(string format)
    {
        StringBuilder str = new();
        bool isSeconderybitOn = false;
        foreach (var field in _dictionary)
        {
            if (field.Key > 64 && field.Value.IsSet)
            {
                isSeconderybitOn = true;
            }

            str.Append(field.Value.ToString());
        }

        _isSecondaryBitmapOn = isSeconderybitOn;

        byte[] databytes, primaryBitMap, secondaryBitMap;

        if (string.Equals(format, "ASCII", StringComparison.OrdinalIgnoreCase))
        {
            primaryBitMap = ByteHelper.convertBinaryToHexUsingConvert(_primaryBitMap);
        }
        else
        {
            primaryBitMap = _primaryBitMap;
        }

        if (_isSecondaryBitmapOn)
        {
            if (string.Equals(format, "ASCII", StringComparison.OrdinalIgnoreCase))
            {
                secondaryBitMap = ByteHelper.convertBinaryToHexUsingConvert(_secondaryBitMap);
            }
            else
            {
                secondaryBitMap = _secondaryBitMap;
            }

            databytes = ByteHelper.CombineBytes(_headerMAP, primaryBitMap, secondaryBitMap, Encoding.ASCII.GetBytes(str.ToString()));
        }
        else
            databytes = ByteHelper.CombineBytes(_headerMAP, primaryBitMap, Encoding.ASCII.GetBytes(str.ToString()));

        return databytes;
    }

    public byte[] GetHeaderBytes()
    {
        return _primaryBitMap;
    }

    public void SetPrimaryBitMap(byte[] primaryBitMap)
    {
        _primaryBitMap = primaryBitMap;
    }

    public void SetSecondaryBitMap(byte[] secondaryBitMap)
    {
        _secondaryBitMap = secondaryBitMap;
    }
}
