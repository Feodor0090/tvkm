using tvkm.UIEngine;
using tvkm.UIEngine.Controls;
using tvkm.UIEngine.Templates;
using VkNet.Model.Attachments;

namespace tvkm;

public class VideoScreen : ListScreen<App>
{
    public VideoScreen(Video video, ScreenStack<App> stack) : base(video.Title)
    {
        if (video.OwnerId != stack.MainScreen.Api.UserId && video.Added != true)
            Add(new Button<App>("Добавить к себе", () =>
            {
                stack.MainScreen.Api.Video.Add((long) video.Id, (long) video.OwnerId);
                this.RemoveAt(0);
            }));
        var f = video.Files;
        if (f == null) return;
        if (f.External != null)
            Add(new Button<App>(f.External.AbsoluteUri,
                () => ExternalUtils.TryOpenBrowser(f.External.AbsoluteUri, stack)));
        if (f.Hls != null)
            Add(new Button<App>("M3U8/mpeg2-hls", () => ExternalUtils.TryPlayMediaAsIs(f.Hls.AbsoluteUri, stack)));
        if (f.DashWebm != null)
            Add(new Button<App>("DASH/webm", () => ExternalUtils.TryPlayMediaAsIs(f.DashWebm.AbsoluteUri, stack)));
        if (f.Mp4_240 != null)
            Add(new Button<App>("MP4/h264 (240p)", () => ExternalUtils.TryPlayMediaAsIs(f.Mp4_240.AbsoluteUri, stack)));
        if (f.Mp4_360 != null)
            Add(new Button<App>("MP4/h264 (360p)", () => ExternalUtils.TryPlayMediaAsIs(f.Mp4_360.AbsoluteUri, stack)));
        if (f.Mp4_480 != null)
            Add(new Button<App>("MP4/h264 (480p)", () => ExternalUtils.TryPlayMediaAsIs(f.Mp4_480.AbsoluteUri, stack)));
        if (f.Mp4_720 != null)
            Add(new Button<App>("MP4/h264 (720p)", () => ExternalUtils.TryPlayMediaAsIs(f.Mp4_720.AbsoluteUri, stack)));
        if (f.Mp4_1080 != null)
            Add(new Button<App>("MP4/h264 (1080p)",
                () => ExternalUtils.TryPlayMediaAsIs(f.Mp4_1080.AbsoluteUri, stack)));
    }
}