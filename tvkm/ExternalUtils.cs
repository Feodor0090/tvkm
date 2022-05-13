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
        throw new NotImplementedException();
    }

    public static void TryViewPhoto(Photo photo, ScreenStack stack)
    {
        var size = photo.Sizes.OrderBy(x => x.Width).FirstOrDefault();
        var path = CacheFile(size.Url.AbsoluteUri);
        try
        {
            Process.Start(Settings.ImageViewerPath, path);
        }
        catch
        {
            stack.Push(new AlertPopup("Не удалось запустить ваш просмотрщик изображений. Проверьте файл конфигурации.", stack));
        }
    }
}