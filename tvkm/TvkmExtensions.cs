namespace tvkm;

public static class TvkmExtensions
{
    public static string PadRightOrTrim(this string s, int targetLen)
    {
        return s.Length <= targetLen ? s.PadRight(targetLen) : string.Concat(s.AsSpan(0, targetLen - 3), "...");
    }
}