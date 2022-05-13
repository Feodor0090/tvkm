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

    public static void TryOpenBrowser(string url, ScreenStack stack)
    {
        try
        {
            Process.Start(BrowserPath, url);
        }
        catch
        {
            if (BrowserPath.Contains("w3m"))
            {
                stack.Push(new AlertPopup("Не удалось запустить W3M - он задан как браузер для открытия URL. Проверьте его установку или отредактируйте конфигурацию для использования другого браузера.", stack));
            }
            else
            {
                stack.Push(new AlertPopup("Не удалось запустить ваш браузер. Проверьте файл конфигурации.", stack));
            }
        }
    }
}