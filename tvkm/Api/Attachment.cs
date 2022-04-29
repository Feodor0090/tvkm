using VkNet.Model.Attachments;

namespace tvkm.Api;

public struct Attachment
{
    public Attachment(string caption)
    {
        Caption = caption;
        ActionUrl = null;
    }

    public string Caption;
    public string? ActionUrl;

    private static string PrintLen(int l)
    {
        if (l > 0)
            return
                (l / 60).ToString().PadLeft(2, '0') + ":" +
                (l % 60).ToString().PadLeft(2, '0');
        return "live";
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
                    result.Add(new Attachment("Фотография"));
                    break;
                case Video x:
                    result.Add(new Attachment($"Видео \"{x.Title}\" ({PrintLen(x.Duration ?? 0)})"));
                    break;
                case Audio x:
                    result.Add(new Attachment
                        { Caption = $"Аудио \"{x.Artist} - {x.Title}\" ({PrintLen(x.Duration)})" });
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

                    result.Add(new Attachment($"Документ \"{x.Title}\" ({size})"));
                    break;
                case AudioMessage x:
                    string tr = x.Transcript == null ? "" : $" ({x.Transcript})";
                    result.Add(new Attachment($"Голосовое ({PrintLen((int)x.Duration)}){tr}")
                    {
                        ActionUrl = x.LinkMp3.AbsoluteUri
                    });
                    break;
                case Link x:
                    result.Add(new Attachment(x.Uri.AbsoluteUri)
                    {
                        ActionUrl = x.Uri.AbsoluteUri
                    });
                    break;
                case Sticker x:
                    result.Add(new Attachment
                    {
                        Caption = "Стикер",
                        ActionUrl = x.Images.Skip(1).First().Url.AbsoluteUri
                    });
                    break;
                case Poll x:
                    result.Add(new Attachment("Опрос: " + x.Question));
                    break;
                case Wall x:
                    result.Add(new Attachment("Запись на стене"));
                    break;
                default:
                    result.Add(new Attachment("Unsupported attachment" + att.Instance.ToString()));
                    break;
            }
        }

        // return
        return result.ToArray();
    }
}