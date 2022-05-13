namespace tvkm;

public static class Settings
{
    public static string BrowserPath = "/usr/bin/w3m";
    public static string ImageViewerPath = "/usr/bin/ristretto";
    public static int LongpollTimeout = 25;
    public static int LongpollErrorPause = 5;
    public static bool SendReadEvent = true;

    public static ConsoleColor DefaultColor = ConsoleColor.White;
    public static ConsoleColor SelectionColor = ConsoleColor.Yellow;
    public static ConsoleColor AccentColor = ConsoleColor.Green;

    public static void Apply(Dictionary<string, string> config)
    {
        var fields = typeof(Settings).GetFields();
        foreach (var pair in config)
        {
            var field = fields.FirstOrDefault(x => x.Name == pair.Key);
            if (field == null) continue;

            if (field.FieldType == typeof(string))
            {
                field.SetValue(null, pair.Value);
            }
            else if (field.FieldType == typeof(int))
            {
                field.SetValue(null, int.Parse(pair.Value));
            }
            else if (field.FieldType == typeof(bool))
            {
                field.SetValue(null, bool.Parse(pair.Value));
            }
            else if (field.FieldType == typeof(ConsoleColor))
            {
                field.SetValue(null, (ConsoleColor) int.Parse(pair.Value));
            }
        }
    }
}