using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using VkNet.Model.RequestParams.Polls;
using VkNet.Utils;

namespace VkNet.Categories;

/// <inheritdoc />
public partial class PollsCategory
{
    /// <inheritdoc />
    public Task<Poll> GetByIdAsync(PollsGetByIdParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => GetById(@params));
    }

    /// <inheritdoc />
    public Task<bool> EditAsync(PollsEditParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => Edit(@params));
    }

    /// <inheritdoc />
    public Task<bool> AddVoteAsync(PollsAddVoteParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => AddVote(@params));
    }

    /// <inheritdoc />
    public Task<bool> DeleteVoteAsync(PollsDeleteVoteParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => DeleteVote(@params));
    }

    /// <inheritdoc />
    public Task<VkCollection<PollAnswerVoters>> GetVotersAsync(PollsGetVotersParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => GetVoters(@params));
    }

    /// <inheritdoc />
    public Task<Poll> CreateAsync(PollsCreateParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => Create(@params));
    }

    /// <inheritdoc />
    public Task<ReadOnlyCollection<GetBackgroundsResult>> GetBackgroundsAsync()
    {
        return TypeHelper.TryInvokeMethodAsync(() => GetBackgrounds());
    }

    /// <inheritdoc />
    public Task<PhotoUploadServer> GetPhotoUploadServerAsync(long ownerId)
    {
        return TypeHelper.TryInvokeMethodAsync(() => GetPhotoUploadServer(ownerId));
    }

    /// <inheritdoc />
    public Task<SavePhotoResult> SavePhotoAsync(SavePhotoParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => SavePhoto(@params));
    }
}