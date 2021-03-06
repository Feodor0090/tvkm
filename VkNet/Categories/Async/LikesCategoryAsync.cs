using System;
using System.Threading.Tasks;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace VkNet.Categories;

/// <inheritdoc />
public partial class LikesCategory
{
    /// <inheritdoc />
    public Task<VkCollection<long>> GetListAsync(LikesGetListParams @params, bool skipAuthorization = false)
    {
        return TypeHelper.TryInvokeMethodAsync(() =>
            GetList(@params, skipAuthorization));
    }

    /// <inheritdoc />
    public Task<UserOrGroup> GetListExAsync(LikesGetListParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => GetListEx(@params));
    }

    /// <inheritdoc />
    public Task<long> AddAsync(LikesAddParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => Add(@params));
    }

    /// <inheritdoc />
    public Task<long> DeleteAsync(LikeObjectType type, long itemId, long? ownerId = null)
    {
        return TypeHelper.TryInvokeMethodAsync(() =>
            Delete(type, itemId, ownerId));
    }

    /// <inheritdoc />
    [Obsolete(ObsoleteText.CaptchaNeeded, true)]
    public Task<long> DeleteAsync(LikeObjectType type
        , long itemId
        , long? ownerId = null
        , long? captchaSid = null
        , string captchaKey = null)
    {
        return TypeHelper.TryInvokeMethodAsync(() =>
            Delete(type, itemId, ownerId, captchaSid, captchaKey));
    }

    /// <inheritdoc />
    public Task<bool> IsLikedAsync(LikeObjectType type, long itemId, long? userId = null, long? ownerId = null)
    {
        return TypeHelper.TryInvokeMethodAsync(() =>
            IsLiked(out var _, type, itemId, userId, ownerId));
    }
}