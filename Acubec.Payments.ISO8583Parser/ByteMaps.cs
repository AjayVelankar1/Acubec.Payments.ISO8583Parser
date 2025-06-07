using Acubec.Payments.ISO8583Parser.Helpers;
using Acubec.Payments.ISO8583Parser.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
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
    public void SetBitMap(Span<byte> bitMap)
    {
        _bitMap = bitMap.ToArray();
    }

    public Span<byte> BitMap => _bitMap;

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
        if (string.IsNullOrEmpty(value.Value)) return;

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

    internal Span<byte> GetDataByte(DataEncoding format, IMTIParser mtiParser, IServiceProvider serviceProvider)
    {
        for (int i = 0; i < _byteMaps.Length; i++)
        {
            _byteMaps[i].BitMap.Fill(0);
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
        var encoder = serviceProvider.GetKeyedService<IEncoderFormator>(format.ToString());
        byte[] dataBytes = encoder.Decode(str.ToString());
        
        var biteMapBytes = GetHeaderBytes(format);
        var header = mtiParser.WriteMTI(Encoding.ASCII.GetString(_headerMAP), biteMapBytes.Length + dataBytes?.Length??0).ToArray();
        return ByteHelper.Combine(header, biteMapBytes.ToArray(),dataBytes);
    }

    public Span<byte> GetHeaderBytes(DataEncoding format)
    {
        Span<byte> bytes = new byte[0];

        for (int i = 0; i < _byteMaps.Length; i++)
        {
            bytes = ByteHelper.Combine(bytes, _byteMaps[i].BitMap);
        }
        return bytes;
    }

    public void SetBitMap(int index, Span<byte> bitMap)
    {
        _byteMaps[index].SetBitMap(bitMap);
    }

    public Span<byte> getBitMap(int index)
    {
        return _byteMaps[index].BitMap;
    }
}
