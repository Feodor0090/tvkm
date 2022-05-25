namespace tvkm;

/// <summary>
/// Set of methods to manage config and session files.
/// </summary>
public class ConfigManager
{
    /// <summary>
    /// Reads configuration. If there is no file, creates it. If it is correct, automatically applies it.
    /// </summary>
    public static void ReadConfig()
    {
        try
        {
            var lines = File.ReadAllLines("config.txt").Select(x => x.Split(':').Select(y => y.Trim()).ToArray());
            Dictionary<string, string> config = new();
            foreach (var line in lines)
            {
                if (line.Length < 2 || line[0].StartsWith("//") || line[0].StartsWith('#'))
                    continue;

                config.Add(line[0], line[1]);
            }

            Settings.Apply(config);
        }
        catch (FileNotFoundException)
        {
            CreatePlaceholderConfig();
        }
        catch (IOException)
        {
        }
        catch
        {
        }
    }

    /// <summary>
    /// Creates empty config with paths to external apps and SendReadEvent switch. Uses W3M, Ristretto and VLC.
    /// </summary>
    public static void CreatePlaceholderConfig()
    {
        using StreamWriter sw = new("config.txt", false);
        sw.Write(
            $"# Это пустой конфигурационный файл для TVKM. Вручную допишите здесь нужные параметры или выберите сохранение всех в меню настроек программы.\n\n" +
            $"# BrowserPath: /usr/bin/w3m\n" +
            $"# ImageViewerPath: /usr/bin/ristretto\n" +
            $"# PlayerPath: /usr/bin/vlc" +
            $"# SendReadEvent: true");
        sw.Flush();
    }

    public static void SaveConfig()
    {
        using StreamWriter sw = new("config.txt", false);
        var config = Settings.Export();
        sw.WriteLine("# Конфигурация TVKM - сохранено " + DateTime.Now.ToString("HH:mm:ss dd.MM.yyyy"));
        sw.WriteLine();
        foreach (var pair in config)
        {
            sw.WriteLine(pair.Key + ": " + pair.Value);
        }

        sw.Flush();
    }

    /// <summary>
    /// Reads a file with saved user ID a token to restore a session.
    /// </summary>
    /// <returns>User ID and token.</returns>
    public static (long, string) ReadSavedToken()
    {
        var s = File.ReadAllText("session.txt").Split(' ');
        return (long.Parse(s[0]), s[1]);
    }

    /// <summary>
    /// Saves user's session.
    /// </summary>
    /// <param name="userId">User ID.</param>
    /// <param name="token">Access token.</param>
    public static void WriteToken(long userId, string token)
    {
        using StreamWriter sw = new("session.txt", false);
        sw.Write($"{userId} {token}");
        sw.Flush();
    }

    /// <summary>
    /// Deletes file with user's ID and token.
    /// </summary>
    public static void DeleteSavedToken()
    {
        File.Delete("session.txt");
    }
}