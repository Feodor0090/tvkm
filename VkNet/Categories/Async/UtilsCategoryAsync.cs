using System;
using System.Threading.Tasks;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace VkNet.Categories;

/// <inheritdoc />
public partial class UtilsCategory
{
    /// <inheritdoc />
    public Task<LinkAccessType> CheckLinkAsync(string url)
    {
        return TypeHelper.TryInvokeMethodAsync(() => CheckLink(url));
    }

    /// <inheritdoc />
    public Task<LinkAccessType> CheckLinkAsync(Uri url)
    {
        return TypeHelper.TryInvokeMethodAsync(() => CheckLink(url));
    }

    /// <inheritdoc />
    public Task<VkObject> ResolveScreenNameAsync(string screenName)
    {
        return TypeHelper.TryInvokeMethodAsync(() => ResolveScreenName(screenName));
    }

    /// <inheritdoc />
    public Task<DateTime> GetServerTimeAsync()
    {
        return TypeHelper.TryInvokeMethodAsync(() => GetServerTime());
    }

    /// <inheritdoc />
    public Task<ShortLink> GetShortLinkAsync(Uri url, bool isPrivate)
    {
        return TypeHelper.TryInvokeMethodAsync(() => GetShortLink(url, isPrivate));
    }

    /// <inheritdoc />
    public Task<bool> DeleteFromLastShortenedAsync(string key)
    {
        return TypeHelper.TryInvokeMethodAsync(() => DeleteFromLastShortened(key));
    }

    /// <inheritdoc />
    public Task<VkCollection<ShortLink>> GetLastShortenedLinksAsync(ulong count = 10, ulong offset = 0)
    {
        return TypeHelper.TryInvokeMethodAsync(() => GetLastShortenedLinks(count, offset));
    }

    /// <inheritdoc />
    public Task<LinkStatsResult> GetLinkStatsAsync(LinkStatsParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => GetLinkStats(@params));
    }
}