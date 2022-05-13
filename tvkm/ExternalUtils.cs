using System.Diagnostics;
using tvkm.UIEngine;
using tvkm.UIEngine.Templates;
using VkNet.Model.Attachments;

namespace tvkm;

/// <summary>
/// Set of methods to do anything outside the client.
/// </summary>
public static class ExternalUtils
{
    public static void TryOpenBrowser(string url, ScreenStack stack)
    {
        try
        {
            Process.Start(Settings.BrowserPath, url);
        }
        catch
        {
            if (Settings.BrowserPath.Contains("w3m"))
            {
                stack.Push(new AlertPopup(
                    "Не удалось запустить W3M - он задан как браузер для открытия URL. Проверьте его установку или отредактируйте конфигурацию для использования другого браузера.",
                    stack));
            }
            else
            {
                stack.Push(new AlertPopup("Не удалось запустить ваш браузер. Проверьте файл конфигурации.", stack));
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

    public static void TryViewPhoto(Photo photo, ScreenStack stack)
    {
        var size = photo.Sizes.OrderBy(x => x.Width).FirstOrDefault();
        TryViewPhoto(size.Url.AbsoluteUri, stack);
    }

    public static void TryViewPhoto(string photo, ScreenStack stack)
    {
        var path = CacheFile(photo);
        try
        {
            Process.Start(Settings.ImageViewerPath, path);
        }
        catch
        {
            stack.Push(new AlertPopup("Не удалось запустить ваш просмотрщик изображений. Проверьте файл конфигурации.",
                stack));
        }
    }
}