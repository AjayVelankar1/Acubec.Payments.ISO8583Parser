namespace Acubec.Payments.ISO8583Parser.Helpers;

public static class ParserHelper
{
    public static T? TryParse<T>(this string str, T result, T defaultValue = default) where T : IParsable<T>
    {
        if (str == null)
        {
            return defaultValue;
        }

        return T.TryParse(str, null, result: out result) ? result : defaultValue;
    }

    public static string TryRightPadding(this string str, int totalWidth, char paddingChar)
    {
        if (str == null)
        {
            str = string.Empty;
        }

        return str.PadRight(totalWidth, paddingChar);
    }
}
