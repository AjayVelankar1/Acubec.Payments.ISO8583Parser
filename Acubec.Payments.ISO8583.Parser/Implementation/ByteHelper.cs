using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acubec.Payments.ISO8583.Parser.Implementation;

internal static class ByteHelper
{
    internal static ReadOnlySpan<byte> Combine(ReadOnlySpan<byte> header, ReadOnlySpan<byte> biteMapBytes, ReadOnlySpan<byte> dataBytes)
    {
        int totalLength = header.Length + biteMapBytes.Length + dataBytes.Length;
        byte[] result = new byte[totalLength];
        int offset = 0;
        header.CopyTo(result.AsSpan(offset, header.Length));
        offset += header.Length;
        biteMapBytes.CopyTo(result.AsSpan(offset, biteMapBytes.Length));
        offset += biteMapBytes.Length;
        dataBytes.CopyTo(result.AsSpan(offset, dataBytes.Length));
        return result;
    }

    internal static Span<byte> Combine(ReadOnlySpan<byte> bytes, ReadOnlySpan<byte> bitMap)
    {
        byte[] result = new byte[bytes.Length + bitMap.Length];
        bytes.CopyTo(result.AsSpan(0, bytes.Length));
        bitMap.CopyTo(result.AsSpan(bytes.Length, bitMap.Length));
        return result;
    }

    //internal static Span<byte> Combine(this param Span<byte> bytes, Span<byte> bitMap)
    //{
    //    byte[] result = new byte[bytes.Length + bitMap.Length];
    //    bytes.CopyTo(result.AsSpan(0, bytes.Length));
    //    bitMap.CopyTo(result.AsSpan(bytes.Length, bitMap.Length));
    //    return result;
    //}

    internal static ReadOnlySpan<byte> GetByteSlice(this ReadOnlySpan<byte> dataByte, int length, int offset)
    {
        if (offset < 0 || length < 0 || offset + length > dataByte.Length)
            throw new ArgumentOutOfRangeException();

        return dataByte.Slice(offset, length);
    }


    public static string ByteSpanToHexString(this ReadOnlySpan<byte> bData)
    {
        StringBuilder stringBuilder = new StringBuilder(bData.Length * 2);
        for (int i = 0; i < bData.Length; i++)
        {
            stringBuilder.AppendFormat("{0:X2}", bData[i]);
        }

        return stringBuilder.ToString();
    }

    internal static StringBuilder HexDump(Span<byte> bytes, int bytesPerLine = 16)
    {
        if (bytes == null)
        {
            return new StringBuilder("<null>");
        }

        int num = bytes.Length;
        char[] array = "0123456789ABCDEF".ToCharArray();
        int num2 = 11;
        int num3 = num2 + bytesPerLine * 3 + (bytesPerLine - 1) / 8 + 2;
        int num4 = num3 + bytesPerLine + Environment.NewLine.Length;
        char[] array2 = (new string(' ', num4 - Environment.NewLine.Length) + Environment.NewLine).ToCharArray();
        int num5 = (num + bytesPerLine - 1) / bytesPerLine;
        StringBuilder stringBuilder = new StringBuilder(num5 * num4);
        for (int i = 0; i < num; i += bytesPerLine)
        {
            array2[0] = array[i >> 28 & 0xF];
            array2[1] = array[i >> 24 & 0xF];
            array2[2] = array[i >> 20 & 0xF];
            array2[3] = array[i >> 16 & 0xF];
            array2[4] = array[i >> 12 & 0xF];
            array2[5] = array[i >> 8 & 0xF];
            array2[6] = array[i >> 4 & 0xF];
            array2[7] = array[i & 0xF];
            int num6 = num2;
            int num7 = num3;
            for (int j = 0; j < bytesPerLine; j++)
            {
                if (j > 0 && (j & 7) == 0)
                {
                    num6++;
                }

                if (i + j >= num)
                {
                    array2[num6] = ' ';
                    array2[num6 + 1] = ' ';
                    array2[num7] = ' ';
                }
                else
                {
                    byte b = bytes[i + j];
                    array2[num6] = array[b >> 4 & 0xF];
                    array2[num6 + 1] = array[b & 0xF];
                    array2[num7] = (char)(b < 32 ? 183 : b);
                }

                num6 += 3;
                num7++;
            }

            stringBuilder.Append(array2);
        }

        return stringBuilder;
    }

    internal static ReadOnlySpan<T> ToReadOnlySpan<T>(this Span<T> span)
    {
        ReadOnlySpan<T> result = span;
        return result;
    }

    public static Span<byte> GetByteSlice(this Span<byte> dataByte, int length, int startIndex)
    {
        if (startIndex < 0 || length < 0 || startIndex + length > dataByte.Length)
            throw new ArgumentOutOfRangeException();

        return dataByte.Slice(startIndex, length);
    }

    internal static Span<byte> convertHexToBinaryUsingConvert(ReadOnlySpan<char> hexString)
    {
        if (hexString.Length % 2 != 0)
            throw new ArgumentException("Hex string must have an even length.", nameof(hexString));

        int byteCount = hexString.Length / 2;
        byte[] result = new byte[byteCount];

        for (int i = 0; i < byteCount; i++)
        {
            string byteValue = new string(hexString.Slice(i * 2, 2));
            result[i] = Convert.ToByte(byteValue, 16);
        }

        return result;
    }
}
