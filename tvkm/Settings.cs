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
    public static bool SendReadEvent = true;

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