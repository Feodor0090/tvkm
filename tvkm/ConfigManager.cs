namespace tvkm;

public class ConfigManager
{
    public static void ReadSettings()
    {
        // do nothing for now
    }

    /// <summary>
    /// Reads a file with saved user ID a token to restore a session.
    /// </summary>
    /// <returns>User ID and token.</returns>
    public static (int, string) ReadSavedToken()
    {
        var s = File.ReadAllText("session.txt").Split(' ');
        return (int.Parse(s[0]), s[1]);
    }

    public static void WriteToken(long userId, string token)
    {
        using StreamWriter sw = new("session.txt", false);
        sw.Write($"{userId} {token}");
        sw.Flush();
    }
}