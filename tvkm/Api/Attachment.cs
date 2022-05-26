using tvkm.UIEngine;
using VkNet.Model.Attachments;

namespace tvkm.Api;

public struct Attachment
{
    public enum AttachmentAction : byte
    {
        DoNothing = 0,
        ViewImage,
        ViewMedia,
        JustDownload,
        OpenBrowser,
        RunDelegate,
    }

    public Attachment(string caption, AttachmentAction action, object? actionObject)
    {
        Caption = caption;
        Action = action;
        ActionObject = actionObject;
    }

    public string Caption;
    public AttachmentAction Action;
    public object? ActionObject;

    public static string PrintLen(int l)
    {
        if (l > 0)
            return
                (l / 60).ToString().PadLeft(2, '0') + ":" +
                (l % 60).ToString().PadLeft(2, '0');
        return "live";
    }

    public void View(ScreenStack<App> stack)
    {
        switch (Action)
        {
            case AttachmentAction.OpenBrowser:
                ExternalUtils.TryOpenBrowser(ActionObject.ToString(), stack);
                break;
            case AttachmentAction.ViewImage:
                ExternalUtils.TryViewPhoto(ActionObject.ToString(), stack);
                break;
            case AttachmentAction.ViewMedia:
                ExternalUtils.TryPlayMediaAsIs(ActionObject.ToString(), stack);
                break;
            case AttachmentAction.JustDownload:
                break;
            case AttachmentAction.RunDelegate:
                if (ActionObject is Action<ScreenStack<App>> del)
                    del.Invoke(stack);
                break;
            case AttachmentAction.DoNothing:
                break;
            default:
                break;
        }
    }

    public static Attachment[] Convert(IEnumerable<VkNet.Model.Attachments.Attachment>? atts)
    {
        if (atts == null) return Array.Empty<Attachment>();

        // list to operate
        List<Attachment> result = new();

        foreach (var att in atts)
        {
            switch (att.Instance)
            {
                case Photo x:
                    result.Add(new Attachment((Settings.AttachmentTypesAsEmoji ? "📷" : "Фотография"),
                        AttachmentAction.ViewImage,
                        x.Sizes.OrderBy(x => x.Width).Last().Url.AbsoluteUri));
                    break;
                case Video x:
                    result.Add(new Attachment(
                        $"{(Settings.AttachmentTypesAsEmoji ? "🎥" : "Видео")} \"{x.Title}\" ({PrintLen(x.Duration ?? 0)})",
                        AttachmentAction.RunDelegate,
                        (Action<ScreenStack<App>>) (st => st.Push(new VideoScreen(x, st)))));
                    break;
                case Audio x:
                    result.Add(new Attachment(
                        $"{(Settings.AttachmentTypesAsEmoji ? "🎜" : "Аудио")} \"{x.Artist} - {x.Title}\" ({PrintLen(x.Duration)})",
                        AttachmentAction.ViewMedia, x.Url.AbsoluteUri));
                    break;
                case Document x:
                    string size;
                    long s = x.Size ?? 0;
                    size = s switch
                    {
                        < 4096 => $"{s}B",
                        < 4096 * 1024 => $"{s / 1024}KB",
                        _ => $"{s / 1024 / 1024}MB"
                    };

                    result.Add(new Attachment(
                        $"{(Settings.AttachmentTypesAsEmoji ? "📄" : "Документ")} \"{x.Title}\" ({size})",
                        AttachmentAction.DoNothing, null));
                    break;
                case AudioMessage x:
                    string tr = x.Transcript == null ? "" : $" ({x.Transcript})";
                    result.Add(new Attachment(
                        $"{(Settings.AttachmentTypesAsEmoji ? "📢" : "Голосовое")} ({PrintLen((int) x.Duration)}){tr}",
                        AttachmentAction.ViewMedia, x.LinkMp3.AbsoluteUri));
                    break;
                case Link x:
                    result.Add(new Attachment(x.Uri.AbsoluteUri, AttachmentAction.OpenBrowser, x.Uri.AbsoluteUri));
                    break;
                case Sticker x:
                    result.Add(new Attachment("Стикер", AttachmentAction.ViewImage,
                        x.Images.Skip(1).First().Url.AbsoluteUri));
                    break;
                case Poll x:
                    result.Add(new Attachment("Опрос: " + x.Question, AttachmentAction.DoNothing, null));
                    break;
                case Wall x:
                    result.Add(new Attachment("Запись на стене", AttachmentAction.DoNothing, null));
                    break;
                default:
                    result.Add(new Attachment("Unsupported attachment" + att.Instance.ToString(),
                        AttachmentAction.DoNothing, null));
                    break;
            }
        }

        // return
        return result.ToArray();
    }
}