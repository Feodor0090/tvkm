using System.Diagnostics;
using tvkm.UIEngine;
using VkNet.Model.Attachments;

namespace tvkm;

/// <summary>
/// Set of methods to do anything outside the client.
/// </summary>
public static class ExternalUtils
{
    public static void TryOpenBrowser(string url, ScreenStack<App> stack)
    {
        try
        {
            Process.Start(Settings.BrowserPath, url);
        }
        catch
        {
            if (Settings.BrowserPath.Contains("w3m"))
            {
                stack.Alert(
                    "Не удалось запустить W3M - он задан как браузер для открытия URL. Проверьте его установку или отредактируйте конфигурацию для использования другого браузера.");
            }
            else
            {
                stack.Alert("Не удалось запустить ваш браузер. Проверьте файл конфигурации.");
            }
        }
    }

    private static string CacheFile(string url)
    {
        using HttpClient httpClient = new HttpClient();
        var data = httpClient.GetByteArrayAsync(url).Result;
        var path = Path.GetTempFileName();
        File.WriteAllBytes(path, data);
        return path;
    }

    public static void TryPlayMediaAsIs(string url, ScreenStack<App> stack)
    {
        try
        {
            Console.SetCursorPosition(0, 0);
            Process.Start(Settings.PlayerPath, url);
        }
        catch
        {
            stack.Alert("Не удалось запустить ваш плеер.");
        }
    }

    public static void TryViewPhoto(Photo photo, ScreenStack<App> stack)
    {
        var size = photo.Sizes.OrderBy(x => x.Width).FirstOrDefault();
        TryViewPhoto(size.Url.AbsoluteUri, stack);
    }

    public static void TryViewPhoto(string photo, ScreenStack<App> stack)
    {
        var path = CacheFile(photo);
        try
        {
            Process.Start(Settings.ImageViewerPath, path);
        }
        catch
        {
            stack.Alert("Не удалось запустить ваш просмотрщик изображений. Проверьте файл конфигурации.");
        }
    }
}