namespace tvkm;

public class ConfigManager
{
    public static void ReadSettings()
    {
        try
        {
            var lines = File.ReadAllLines("config.txt").Select(x => x.Split(':').Select(y => y.Trim()).ToArray());
            Dictionary<string, string> config = new();
            foreach (var line in lines)
            {
                if(line.Length<2 || line[0].StartsWith("//") || line[0].StartsWith('#')) 
                    continue;
                
                config.Add(line[0], line[1]);
            }
            Settings.Apply(config);
        }
        catch (FileNotFoundException)
        {
            using StreamWriter sw = new("config.txt", false);
            sw.Write($"# Это пустой конфигурационный файл для TVKM. Загляните на GitHub для получения информации о его создании.\n\n" +
                     $"# BrowserPath: /usr/bin/w3m\n" +
                     $"# ImageViewerPath: /usr/bin/ristretto\n" +
                     $"# PlayerPath: /usr/bin/vlc" +
                     $"# SendReadEvent: true");
            sw.Flush();
        }
        catch (IOException)
        {

        }
        catch
        {
            
        }
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

    public static void WriteToken(long userId, string token)
    {
        using StreamWriter sw = new("session.txt", false);
        sw.Write($"{userId} {token}");
        sw.Flush();
    }
}