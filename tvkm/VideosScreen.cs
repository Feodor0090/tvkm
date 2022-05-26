using tvkm.Api;
using tvkm.UIEngine;
using tvkm.UIEngine.Templates;
using VkNet.Model.RequestParams;

namespace tvkm;

public class VideosScreen : LongLoadingListScreen<App>
{
    public VideosScreen() : base("Видео")
    {
    }

    protected override void Load(ScreenStack<App> stack)
    {
        var videos = stack.MainScreen.Api.Video.Get(new VideoGetParams() { });
        foreach (var video in videos)
        {
            Add(new LinkLabel($"{video.Title} ({Attachment.PrintLen(video.Duration ?? 0)})",
                st => { st.Push(new VideoScreen(video, st)); }));
        }
    }
}