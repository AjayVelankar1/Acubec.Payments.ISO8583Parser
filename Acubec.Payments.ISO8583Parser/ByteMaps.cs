using Acubec.Payments.ISO8583Parser.Helpers;
using Acubec.Payments.ISO8583Parser.Interfaces;
using System.Text;

namespace Acubec.Payments.ISO8583Parser;


internal class ByteMap
{
    byte[] _bitMap;
    bool _isSet;
    string _value;
    public bool IsSet
    {
        get
        {
            return _isSet;
        }
        set
        {
            _isSet = value;
        }
    }

    public ByteMap()
    {
        _bitMap = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
    }
    public void SetBitMap(byte[] bitMap)
    {
        _bitMap = bitMap;
    }

    public byte[] BitMap => _bitMap;

    public void SetValue(int position, IIsoField value, SortedDictionary<int, IIsoField> _dictionary)
    {
        if (value.Value == "" || value.Value == null) return;
        if (!_dictionary.ContainsKey(position))
        {
            _dictionary.Add(position, value);
        }
        else
        {
            _dictionary[position] = value;
        }

        _bitMap[0] |= 0x80;
    }
}


public sealed class ByteMaps
{
    public ByteMaps(string messageType, int byteMapLength = 2)
    {
        _dictionary = new SortedDictionary<int, IIsoField>();
        _headerMAP = Encoding.ASCII.GetBytes(messageType);
        _byteMapLength = byteMapLength;
        _byteMaps = new ByteMap[_byteMapLength];

        for (int i = 0; i < _byteMapLength; i++)
        {
            _byteMaps[i] = new ByteMap();
        }
    }


    private readonly ByteMap[] _byteMaps;
    private readonly int _byteMapLength;

    private readonly byte[] _headerMAP;
    private readonly SortedDictionary<int, IIsoField> _dictionary;

    public void resetBitMap()
    {
        for (int i = 0; i < _byteMapLength; i++)
        {
            _byteMaps[i] = new ByteMap();
        }
    }

    public void SetValue(int position, IIsoField value)
    {
        if (value.Value == "" || value.Value == null) return;

        int index = position / 64;
        int reminder = position % 64;

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
            _byteMaps[0].BitMap[index - 1] |= 0x80;
            _byteMaps[index].BitMap[(reminder - 1) / 8] |= (byte)(0x80 >> (reminder - 1) % 8);
            _byteMaps[index].SetBitMap(_byteMaps[index].BitMap);
        }
        else
        {
            _byteMaps[0].BitMap[(position - 1) / 8] |= (byte)(0x80 >> (position - 1) % 8);
        }

    }

    internal byte[] GetDataByte(DataEncoding format, IMTIParser mtiParser)
    {
        for (int i = 0; i < _byteMaps.Length; i++)
        {
            Array.Fill(_byteMaps[i].BitMap, (byte)0);
        }

        StringBuilder str = new();
        _byteMaps[0].IsSet = true;

        foreach (var field in _dictionary)
        {
            if (field.Key > 64 && field.Value.IsSet)
            {
                int index = field.Key / 64;
                _byteMaps[index - 1].BitMap[0] |= 0x80;
                _byteMaps[index].IsSet = true;
            }

            str.Append(field.Value.ToString());
        }

        byte[] dataBytes = GetHeaderBytes(format);

        if (format == DataEncoding.ASCII)
        {
            dataBytes = ByteHelper.CombineBytes(_headerMAP, dataBytes, Encoding.ASCII.GetBytes(str.ToString()));
        }
        else
        {
            dataBytes = ByteHelper.CombineBytes(_headerMAP, dataBytes);
            dataBytes = ByteHelper.CombineBytes(dataBytes, Encoding.UTF8.GetBytes(str.ToString()));
        }
        return dataBytes;
    }

    public byte[] GetHeaderBytes(DataEncoding format)
    {
        byte[] bytes = new byte[0];

        for (int i = 0; i < _byteMaps.Length; i++)
        {
            if (_byteMaps[i].IsSet)
            {
                if (format == DataEncoding.ASCII)
                {
                    bytes = ByteHelper.convertBinaryToHexUsingConvert(bytes);
                }
                else
                {
                    bytes = ByteHelper.Combine(bytes, _byteMaps[i].BitMap);
                }

            }
        }
        return bytes;
    }

    public void SetBitMap(int index, byte[] bitMap)
    {
        _byteMaps[index].SetBitMap(bitMap);
    }

    public byte[] getBitMap(int index)
    {
        return _byteMaps[index].BitMap;
    }
}
