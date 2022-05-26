using System.Threading.Tasks;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace VkNet.Categories;

/// <inheritdoc />
public partial class BoardCategory
{
    /// <inheritdoc />
    public Task<VkCollection<Topic>> GetTopicsAsync(BoardGetTopicsParams @params, bool skipAuthorization = false)
    {
        return TypeHelper.TryInvokeMethodAsync(() =>
            GetTopics(@params, skipAuthorization));
    }

    /// <inheritdoc />
    public Task<TopicsFeed> GetCommentsAsync(BoardGetCommentsParams @params, bool skipAuthorization = false)
    {
        return TypeHelper.TryInvokeMethodAsync(() =>
            GetComments(@params, skipAuthorization));
    }

    /// <inheritdoc />
    public Task<long> AddTopicAsync(BoardAddTopicParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => AddTopic(@params));
    }

    /// <inheritdoc />
    public Task<bool> DeleteTopicAsync(BoardTopicParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => DeleteTopic(@params));
    }

    /// <inheritdoc />
    public Task<bool> CloseTopicAsync(BoardTopicParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => CloseTopic(@params));
    }

    /// <inheritdoc />
    public Task<bool> OpenTopicAsync(BoardTopicParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => OpenTopic(@params));
    }

    /// <inheritdoc />
    public Task<bool> FixTopicAsync(BoardTopicParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => FixTopic(@params));
    }

    /// <inheritdoc />
    public Task<bool> UnFixTopicAsync(BoardTopicParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => UnFixTopic(@params));
    }

    /// <inheritdoc />
    public Task<bool> EditTopicAsync(BoardEditTopicParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => EditTopic(@params));
    }

    /// <inheritdoc />
    public Task<long> CreateCommentAsync(BoardCreateCommentParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => CreateComment(@params));
    }

    /// <inheritdoc />
    public Task<bool> DeleteCommentAsync(BoardCommentParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => DeleteComment(@params));
    }

    /// <inheritdoc />
    public Task<bool> EditCommentAsync(BoardEditCommentParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => EditComment(@params));
    }

    /// <inheritdoc />
    public Task<bool> RestoreCommentAsync(BoardCommentParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => RestoreComment(@params));
    }
}