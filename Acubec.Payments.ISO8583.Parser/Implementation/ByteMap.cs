using Acubec.Payments.ISO8583.Parser.Interfaces;
using Acubec.Payments.ISO8583.Parser.Types;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583.Parser.Implementation;


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
            _byteMaps[index].BitMap[(reminder - 1) / 8] |= (byte)(0x80 >> ((reminder - 1) % 8));
            _byteMaps[index].SetBitMap(_byteMaps[index].BitMap);
        }
        else
        {
            
            _byteMaps[0].BitMap[(position - 1) / 8] |= (byte)(0x80 >> ((position - 1) % 8));
        }

    }

    internal byte[] GetDataByte(DataEncoding format, IHeaderParser mtiParser, IIsoMessage message, SchemaConfiguration schemaConfiguration, IServiceProvider serviceProvider)
    {
        ReadOnlySpan<byte> str = Span<byte>.Empty;
        _byteMaps[0].IsSet = true;
        

        foreach (var field in _dictionary)
        {
            int index = 0;
            if (field.Key > 64 && field.Value.IsSet)
            {
                index = field.Key / 64;
                _byteMaps[index - 1].BitMap[0] |= 0x80;
            }

            if (field.Value.IsSet)
            {
                _byteMaps[index].IsSet = true;
                str = ByteHelper.Combine(str, field.Value.ValueSpans);
            }
        }
        
        var biteMapBytes = GetHeaderBytes(format).ToArray();
        
        var header = mtiParser.WriteMTI(Encoding.ASCII.GetString(_headerMAP), biteMapBytes.Length + str.Length).ToArray();
        // Fix: Actually return the combined bytes, not Span<byte>.Empty
        return ByteHelper.Combine(header, biteMapBytes, str.ToArray()).ToArray();
    }

    public byte[] GetHeaderBytes(DataEncoding format)
    {
        byte[] bytes = new byte[0];

        for (int i = 0; i < _byteMaps.Length; i++)
        {
            bytes = ByteHelper.Combine(bytes, _byteMaps[i].BitMap).ToArray();
        }
        return bytes;
    }

    public void SetBitMap(int index, Span<byte> bitMap)
    {
        _byteMaps[index].SetBitMap(bitMap);
    }

    public byte[] getBitMap(int index)
    {
        return _byteMaps[index].BitMap;
    }

    public int ByteMapLength => _byteMapLength;
}


