using System.Text;

namespace Acubec.Payments.ISO8583Parser.Helpers;
public static class ByteHelper
{
    public static byte[] CombineBytes(params byte[][] arrays)
    {
        byte[] array = new byte[arrays.Sum((a) => a.Length)];
        int num = 0;
        foreach (byte[] array2 in arrays)
        {
            Buffer.BlockCopy(array2, 0, array, num, array2.Length);
            num += array2.Length;
        }

        return array;
    }

    public static bool ArrayNotZero(byte[] b)
    {
        checked
        {
            int num = b.Length - 1;
            int num2 = 0;
            while (true)
            {
                if (num2 > num)
                {
                    return false;
                }

                if (b[num2] != 0)
                {
                    break;
                }

                num2++;
            }

            return true;
        }
    }

    public static string ByteArrayToHexString(this byte[] bData)
    {
        StringBuilder stringBuilder = new StringBuilder();
        int upperBound = bData.GetUpperBound(0);
        for (int i = 0; i <= upperBound; i = checked(i + 1))
        {
            stringBuilder.AppendFormat("{0:X2}", bData[i]);
        }

        string result = stringBuilder.ToString();
        stringBuilder.Clear();
        return result;
    }

    public static byte[] Combine(params byte[][] arrays)
    {
        byte[] array = new byte[arrays.Sum((x) => x.Length)];
        int num = 0;
        foreach (byte[] array2 in arrays)
        {
            Buffer.BlockCopy(array2, 0, array, num, array2.Length);
            num += array2.Length;
        }

        return array;
    }

    public static byte[] GetByteSlice(this byte[] dataByte, int length, int startIndex)
    {
        byte[] array = new byte[length];
        Array.Copy(dataByte, startIndex, array, 0, length);
        return array;
    }

    public static string ToString(this byte[] array)
    {
        return Encoding.ASCII.GetString(array);
    }

    public static byte[] ToBytes(this string str)
    {
        return Encoding.ASCII.GetBytes(str);
    }

    public static byte[] GetBytesFromHexString(this string hexString)
    {
        return hexString.AsSpan().GetBytesFromHexString();
    }

    public static byte[] GetBytesFromHexString(this ReadOnlySpan<char> hexString)
    {
        byte[] array = new byte[hexString.Length / 2];
        for (int i = 0; i < hexString.Length / 2; i++)
        {
            array[i] = Convert.ToByte(hexString.Slice(2 * i, 2).ToString(), 16);
        }

        return array;
    }

    public static string GetHexRepresentation(this byte[] bytes)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (byte b in bytes)
        {
            stringBuilder.AppendFormat("{0:X2}", b);
        }

        return stringBuilder.ToString();
    }

    public static byte[] GetBitmapFromHexString(string bitmapString)
    {
        byte[] array = new byte[8];
        int num = 0;
        for (int i = 0; i < bitmapString.Length; i += 2)
        {
            array[num++] = Convert.ToByte(bitmapString.Substring(i, 2), 16);
        }

        return array;
    }

    public static string GetHexRepresentationOfBitmap(byte[] bytes)
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (byte b in bytes)
        {
            stringBuilder.AppendFormat("{0:X2}", b);
        }

        return stringBuilder.ToString();
    }

    public static byte[] ParseHex(this string hex)
    {
        return hex.AsSpan().ParseHex();
    }

    public static byte[] ParseHex(this ReadOnlySpan<char> hex)
    {
        int num = hex.StartsWith("0x") ? 2 : 0;
        if (hex.Length % 2 != 0)
        {
            throw new ArgumentException("Invalid length: " + hex.Length);
        }

        byte[] array = new byte[(hex.Length - num) / 2];
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = (byte)(ParseNybble(hex[num]) << 4 | ParseNybble(hex[num + 1]));
            num += 2;
        }

        return array;
    }

    private static int ParseNybble(char c)
    {
        if (c >= '0' && c <= '9')
        {
            return c - 48;
        }

        if (c >= 'A' && c <= 'F')
        {
            return c - 65 + 10;
        }

        if (c >= 'a' && c <= 'f')
        {
            return c - 97 + 10;
        }

        throw new ArgumentException("Invalid hex digit: " + c);
    }

    public static string HexDump(byte[] bytes, int bytesPerLine = 16)
    {
        if (bytes == null)
        {
            return "<null>";
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

        return stringBuilder.ToString();
    }

    public static byte[] ASCIIToHex(this byte[] bytes)
    {
        byte[] hexArray = new byte[bytes.Length * 2];

        for (int i = 0; i < bytes.Length; i++)
        {
            // Convert each byte to its hex representation and store in the array
            hexArray[i * 2] = GetHexValue(bytes[i] >> 4 & 0x0F);       // High nibble
            hexArray[i * 2 + 1] = GetHexValue(bytes[i] & 0x0F); // Low nibble
        }

        return hexArray;
    }

    static byte GetHexValue(int nibble)
    {
        // Convert a nibble (4 bits) to its hex ASCII value
        return (byte)(nibble < 10 ? nibble + '0' : nibble - 10 + 'A');
    }

    internal static byte[] convertHexToBinaryUsingConvert(string hexString)
    {
        // Convert hex to binary using Convert class
        byte[] hexBytes = new byte[hexString.Length / 2];
        for (int i = 0; i < hexBytes.Length; i++)
        {
            hexBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
        }

        return hexBytes;
    }

    public static byte[] HexStringToByteArray(string hexString)
    {
        // Convert hex to binary using Convert class
        byte[] hexBytes = new byte[hexString.Length / 2];
        for (int i = 0; i < hexBytes.Length; i++)
        {
            hexBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
        }

        return hexBytes;
    }

    internal static byte[] convertBinaryToHexUsingConvert(byte[] ba)
    {
        StringBuilder hex = new StringBuilder(ba.Length * 2);
        foreach (byte b in ba)
        {
            hex.AppendFormat("{0:x2}", b);
        }

        return Encoding.ASCII.GetBytes(hex.ToString());
    }
}
