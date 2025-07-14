using Acubec.Payments.ISO8583.Parser.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583.Parser.Implementation;

internal class AsciiEncoder : IEncoder
{
    public ReadOnlySpan<byte> Decode(ReadOnlySpan<char> value)
    {
        byte[] bytes = new byte[Encoding.ASCII.GetByteCount(value)];
        Encoding.ASCII.GetBytes(value, bytes);
        return bytes;
    }

    public ReadOnlySpan<byte> DecodeFromASCIIValue(ReadOnlySpan<char> value)
    {
        var asciiBytes = Encoding.ASCII.GetBytes(value.ToString());
        byte[] hexBytes = new byte[asciiBytes.Length * 2];
        for (int i = 0; i < asciiBytes.Length; i++)
        {
            string hex = asciiBytes[i].ToString("X2");
            hexBytes[i * 2] = (byte)hex[0];
            hexBytes[i * 2 + 1] = (byte)hex[1];
        }
        return hexBytes;
    }

    public ReadOnlySpan<char> Encode(ReadOnlySpan<byte> value)
    {
        return Encoding.ASCII.GetString(value).AsSpan();
    }
}

internal class UTF8Encoder : IEncoder
{
    public ReadOnlySpan<byte> Decode(ReadOnlySpan<char> value)
    {
        byte[] bytes = new byte[Encoding.UTF8.GetByteCount(value)];
        Encoding.UTF8.GetBytes(value, bytes);
        return bytes;
    }

    public ReadOnlySpan<byte> DecodeFromASCIIValue(ReadOnlySpan<char> value)
    {
        throw new NotImplementedException();
    }

    public ReadOnlySpan<char> Encode(ReadOnlySpan<byte> value)
    {
        return Encoding.UTF8.GetString(value).AsSpan();
    }
}

internal class UTF32Encoder : IEncoder
{
    public ReadOnlySpan<byte> Decode(ReadOnlySpan<char> value)
    {
        byte[] bytes = new byte[Encoding.UTF32.GetByteCount(value)];
        Encoding.UTF32.GetBytes(value, bytes);
        return bytes;
    }

    public ReadOnlySpan<byte> DecodeFromASCIIValue(ReadOnlySpan<char> value)
    {
        throw new NotImplementedException();
    }

    public ReadOnlySpan<char> Encode(ReadOnlySpan<byte> value)
    {
        return Encoding.UTF32.GetString(value).AsSpan();
    }
}

internal class BinaryEncoder : IEncoder
{
    // Decodes a hex string (e.g., "00D4") to a byte array
    public ReadOnlySpan<byte> Decode(ReadOnlySpan<char> value)
    {
        if (value.IsEmpty || value.Length % 2 != 0)
            value =( "0" +  value.ToString()) .AsSpan(); // Default to empty byte array if input is invalid

        byte[] bytes = new byte[value.Length / 2];
        for (int i = 0; i < bytes.Length; i++)
        {
            string hexPair = new string(new char[] { value[i * 2], value[i * 2 + 1] });
            bytes[i] = Convert.ToByte(hexPair, 16);
        }
        return bytes;
    }

    public ReadOnlySpan<byte> DecodeFromASCIIValue(ReadOnlySpan<char> value)
    {
        throw new NotImplementedException();
    }

    // Encodes a byte array to a hex string (e.g., {0x00, 0xD4} => "00D4")
    public ReadOnlySpan<char> Encode(ReadOnlySpan<byte> value)
    {
        if (value.IsEmpty)
            throw new ArgumentNullException(nameof(value));
        char[] chars = new char[value.Length * 2];
        for (int i = 0; i < value.Length; i++)
        {
            string hex = value[i].ToString("X2");
            chars[i * 2] = hex[0];
            chars[i * 2 + 1] = hex[1];
        }
        return chars;
    }
}

internal class BinaryUnPackedEncoder : IEncoder
{
    // Decodes a string of decimal digits (e.g., "1234") to a byte array {0x01, 0x02, 0x03, 0x04}
    public ReadOnlySpan<byte> Decode(ReadOnlySpan<char> value)
    {
        if (value.IsEmpty)
            throw new ArgumentException("Input must be a non-null string of decimal digits.");

        byte[] bytes = new byte[value.Length];
        for (int i = 0; i < value.Length; i++)
        {
            char c = value[i];
            if (c < '0' || c > '9')
                throw new ArgumentException($"Invalid character '{c}' at position {i}. Only decimal digits are allowed.");
            bytes[i] = (byte)(c - '0');
        }
        return bytes;
    }

    // Encodes a byte array {0x01, 0x02, 0x03, 0x04} to a string of decimal digits ("1234")
    public ReadOnlySpan<char> Encode(ReadOnlySpan<byte> value)
    {
        if (value.IsEmpty)
            throw new ArgumentNullException(nameof(value));
        char[] chars = new char[value.Length];
        for (int i = 0; i < value.Length; i++)
        {
            if (value[i] > 9)
                throw new ArgumentException($"Invalid BCD un-packed value {value[i]} at position {i}. Only 0-9 allowed.");
            chars[i] = (char)('0' + value[i]);
        }
        return chars;
    }
}

internal class BinaryPlusEncoder : IEncoder
{
    // Decodes a hex string (e.g., "00D4") to a byte array, but supports odd-length by prepending '0'

    //ToDO: Need to look this FUnction
    public ReadOnlySpan<byte> Decode(ReadOnlySpan<char> value)
    {
        if (value.Length % 2 != 0)
        {
            char[] padded = new char[value.Length + 1];
            padded[0] = '0';
            value.CopyTo(padded.AsSpan(1));
            value = padded;
        }

        byte[] bytes = new byte[value.Length / 2];

        for (int i = 0; i < value.Length/2; i++)
        {
            bytes[i] = Convert.ToByte(value.Slice(i,2).ToString(), 10);
            i++;
        }
        return bytes;
    }

    public ReadOnlySpan<byte> DecodeFromASCIIValue(ReadOnlySpan<char> value)
    {
        throw new NotImplementedException();
    }

    // Encodes a byte array to a hex string (e.g., {0x0, 0xD4} => "00D4")
    public ReadOnlySpan<char> Encode(ReadOnlySpan<byte> value)
    {
        var sb = new StringBuilder();
        for (int i = 0; i < value.Length; i++)
        {
            sb.Append(value[i].ToString());
        }
        return sb.ToString().AsSpan();
    }
}

internal class HexEncoder : IEncoder
{
    // Decodes a hex string (e.g., "00D4") to a byte array
    public ReadOnlySpan<byte> Decode(ReadOnlySpan<char> value)
    {
        // Remove all whitespace from the input
        Span<char> buffer = stackalloc char[value.Length];
        int len = 0;
        foreach (var c in value)
        {
            if (!char.IsWhiteSpace(c))
                buffer[len++] = c;
        }
        var cleaned = buffer.Slice(0, len);

        if (cleaned.IsEmpty || cleaned.Length % 2 != 0)
            throw new ArgumentException("Input must be a non-null hex string with even length.");

        byte[] bytes = new byte[cleaned.Length / 2];
        for (int i = 0; i < bytes.Length; i++)
        {
            int high = GetHexValue(cleaned[i * 2]);
            int low = GetHexValue(cleaned[i * 2 + 1]);
            if (high == -1 || low == -1)
                throw new ArgumentException($"Invalid hex character at position {i * 2} or {i * 2 + 1}.");
            bytes[i] = (byte)((high << 4) | low);
        }
        return bytes;
    }

    private static int GetHexValue(char c)
    {
        if (c >= '0' && c <= '9') return c - '0';
        if (c >= 'A' && c <= 'F') return c - 'A' + 10;
        if (c >= 'a' && c <= 'f') return c - 'a' + 10;
        return -1;
    }

    // Encodes a byte array to a hex string (e.g., {0x00, 0xD4} => "00D4")
    public ReadOnlySpan<char> Encode(ReadOnlySpan<byte> value)
    {
        if (value.Length % 2 != 0)
            value = ByteHelper.Combine([(byte)0], value);

        if (value.IsEmpty)
            throw new ArgumentNullException(nameof(value));
        char[] chars = new char[value.Length * 2];
        for (int i = 0; i < value.Length; i++)
        {
            string hex = value[i].ToString("X2");
            chars[i * 2] = hex[0];
            chars[i * 2 + 1] = hex[1];
        }
        return chars;
    }

    public ReadOnlySpan<byte> DecodeFromASCIIValue(ReadOnlySpan<char> value)
    {
        var asciiBytes = Encoding.ASCII.GetBytes(value.ToString());
        byte[] hexBytes = new byte[asciiBytes.Length * 2];
        for (int i = 0; i < asciiBytes.Length; i++)
        {
            string hex = asciiBytes[i].ToString("X2");
            hexBytes[i * 2] = (byte)hex[0];
            hexBytes[i * 2 + 1] = (byte)hex[1];
        }
        return hexBytes;
    }
}

internal class EBCDICFEncoder : IEncoder
{
    public ReadOnlySpan<byte> Decode(ReadOnlySpan<char> value)
    {
        Encoding ebcdicEncoding = Encoding.GetEncoding("IBM037");
        // Calculate the maximum byte count needed
        int maxByteCount = ebcdicEncoding.GetMaxByteCount(value.Length);
        byte[] encodedBytes = new byte[maxByteCount];
        int bytesWritten = ebcdicEncoding.GetBytes(value, encodedBytes);
        return encodedBytes.AsSpan(0, bytesWritten);
    }

    public ReadOnlySpan<byte> DecodeFromASCIIValue(ReadOnlySpan<char> value)
    {
        throw new NotImplementedException();
    }

    public ReadOnlySpan<char> Encode(ReadOnlySpan<byte> value)
    {
        Encoding ebcdicEncoding = Encoding.GetEncoding("IBM037");
        return ebcdicEncoding.GetString(value).AsSpan();
    }
}