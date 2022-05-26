using System.Collections.Generic;
using System.Threading.Tasks;
using VkNet.Enums;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace VkNet.Categories;

/// <inheritdoc />
public partial class WallCategory
{
    /// <inheritdoc />
    public Task<WallGetObject> GetAsync(WallGetParams @params, bool skipAuthorization = false)
    {
        return TypeHelper.TryInvokeMethodAsync(() => Get(@params, skipAuthorization));
    }

    /// <inheritdoc />
    public Task<WallGetCommentsResult> GetCommentsAsync(WallGetCommentsParams @params, bool skipAuthorization = false)
    {
        return TypeHelper.TryInvokeMethodAsync(() => GetComments(@params, skipAuthorization));
    }

    /// <inheritdoc />
    public Task<WallGetObject> GetByIdAsync(IEnumerable<string> posts
        , bool? extended = null
        , long? copyHistoryDepth = null
        , ProfileFields fields = null
        , bool skipAuthorization = false)
    {
        return TypeHelper.TryInvokeMethodAsync(() =>
            GetById(posts,
                extended,
                copyHistoryDepth,
                fields,
                skipAuthorization));
    }

    /// <inheritdoc />
    public Task<long> PostAsync(WallPostParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => Post(@params));
    }

    /// <inheritdoc />
    public Task<RepostResult> RepostAsync(string @object, string message, long? groupId, bool markAsAds)
    {
        return TypeHelper.TryInvokeMethodAsync(() =>
            Repost(@object, message, groupId, markAsAds));
    }

    /// <inheritdoc />
    public Task<long> EditAsync(WallEditParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => Edit(@params));
    }

    /// <inheritdoc />
    public Task<bool> DeleteAsync(long? ownerId = null, long? postId = null)
    {
        return TypeHelper.TryInvokeMethodAsync(() => Delete(ownerId, postId));
    }

    /// <inheritdoc />
    public Task<bool> RestoreAsync(long? ownerId = null, long? postId = null)
    {
        return TypeHelper.TryInvokeMethodAsync(() => Restore(ownerId, postId));
    }

    /// <inheritdoc />
    public Task<long> CreateCommentAsync(WallCreateCommentParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => CreateComment(@params));
    }

    /// <inheritdoc />
    public Task<bool> DeleteCommentAsync(long? ownerId, long commentId)
    {
        return TypeHelper.TryInvokeMethodAsync(() => DeleteComment(ownerId, commentId));
    }

    /// <inheritdoc />
    public Task<bool> RestoreCommentAsync(long commentId, long? ownerId)
    {
        return TypeHelper.TryInvokeMethodAsync(() => RestoreComment(commentId, ownerId));
    }

    /// <inheritdoc />
    public Task<WallGetObject> SearchAsync(WallSearchParams @params, bool skipAuthorization = false)
    {
        return TypeHelper.TryInvokeMethodAsync(() =>
            Search(@params, skipAuthorization));
    }

    /// <inheritdoc />
    public Task<WallGetObject> GetRepostsAsync(long? ownerId
        , long? postId
        , long? offset
        , long? count
        , bool skipAuthorization = false)
    {
        return TypeHelper.TryInvokeMethodAsync(() =>
            GetReposts(ownerId, postId, offset, count, skipAuthorization));
    }

    /// <inheritdoc />
    public Task<bool> PinAsync(long postId, long? ownerId = null)
    {
        return TypeHelper.TryInvokeMethodAsync(() => Pin(postId, ownerId));
    }

    /// <inheritdoc />
    public Task<bool> UnpinAsync(long postId, long? ownerId = null)
    {
        return TypeHelper.TryInvokeMethodAsync(() => Unpin(postId, ownerId));
    }

    /// <inheritdoc />
    public Task<bool> EditCommentAsync(long commentId
        , string message
        , long? ownerId = null
        , IEnumerable<MediaAttachment> attachments = null)
    {
        return TypeHelper.TryInvokeMethodAsync(() =>
            EditComment(commentId, message, ownerId, attachments));
    }

    /// <inheritdoc />
    public Task<bool> ReportPostAsync(long ownerId, long postId, ReportReason? reason = null)
    {
        return TypeHelper.TryInvokeMethodAsync(() => ReportPost(ownerId, postId, reason));
    }

    /// <inheritdoc />
    public Task<bool> ReportCommentAsync(long ownerId, long commentId, ReportReason? reason)
    {
        return TypeHelper.TryInvokeMethodAsync(() =>
            ReportComment(ownerId, commentId, reason));
    }

    /// <inheritdoc />
    public Task<bool> EditAdsStealthAsync(EditAdsStealthParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => EditAdsStealth(@params));
    }

    /// <inheritdoc />
    public Task<long> PostAdsStealthAsync(PostAdsStealthParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => PostAdsStealth(@params));
    }

    /// <inheritdoc />
    public Task<bool> OpenCommentsAsync(long ownerId, long postId)
    {
        return TypeHelper.TryInvokeMethodAsync(() => OpenComments(ownerId, postId));
    }

    /// <inheritdoc />
    public Task<bool> CloseCommentsAsync(long ownerId, long postId)
    {
        return TypeHelper.TryInvokeMethodAsync(() => CloseComments(ownerId, postId));
    }

    /// <inheritdoc />
    public Task<bool> CheckCopyrightLinkAsync(string link)
    {
        return TypeHelper.TryInvokeMethodAsync(() => CheckCopyrightLink(link));
    }

    /// <inheritdoc />
    public Task<WallGetCommentResult> GetCommentAsync(int ownerId, int commentId, bool? extended = null,
        string fields = null, bool skipAuthorization = false)
    {
        return TypeHelper.TryInvokeMethodAsync(
            () => GetComment(ownerId, commentId, extended, fields, skipAuthorization));
    }
}