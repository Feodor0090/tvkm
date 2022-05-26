using System.Diagnostics;
using tvkm.UIEngine;
using tvkm.UIEngine.Templates;

namespace tvkm;

public static class Settings
{
    public static string BrowserPath = "/usr/bin/w3m";
    public static string ImageViewerPath = "/usr/bin/ristretto";
    public static string PlayerPath = "/usr/bin/vlc";
    public static int LongpollTimeout = 25;
    public static int LongpollErrorPause = 5;
    public static int ChatsListWidth = 50;
    public static bool AttachmentTypesAsEmoji = false;
    public static bool SendReadEvent = true;

    public static string CustomApiServer = "https://api.vk.com";
    public static string CustomAuthServer = "https://oauth.vk.com";

    /// <summary>
    /// Color to draw generic elements on screen.
    /// </summary>
    public static ConsoleColor DefaultColor = ConsoleColor.White;

    /// <summary>
    /// Color to draw elements that currently are in focus.
    /// </summary>
    public static ConsoleColor SelectionColor = ConsoleColor.Yellow;

    /// <summary>
    /// Color to draw elements that are different from the others.
    /// </summary>
    public static ConsoleColor SpecialColor = ConsoleColor.Green;

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
                if (int.TryParse(pair.Value, out int i))
                    field.SetValue(null, i);
            }
            else if (field.FieldType == typeof(bool))
            {
                var val = pair.Value;
                if (val == "1" || val.ToLower() == "on")
                    val = "True";
                if (val == "0" || val.ToLower() == "off")
                    val = "False";
                if (bool.TryParse(val, out bool b))
                    field.SetValue(null, b);
            }
            else if (field.FieldType == typeof(ConsoleColor))
            {
                if (Enum.TryParse(pair.Value, true, out ConsoleColor color))
                    field.SetValue(null, color);
            }
        }
    }

    public static Dictionary<string, string> Export()
    {
        var fields = typeof(Settings).GetFields();
        Dictionary<string, string> config = new();
        foreach (var field in fields)
        {
            var val = field.GetValue(null)?.ToString() ?? "null";
            config.Add(field.Name, val);
        }

        return config;
    }
}