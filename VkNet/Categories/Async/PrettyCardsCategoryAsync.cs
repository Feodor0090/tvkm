using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace VkNet.Categories;

/// <inheritdoc />
public partial class PrettyCardsCategory
{
    /// <inheritdoc />
    public Task<PrettyCardsCreateResult> CreateAsync(PrettyCardsCreateParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => Create(@params));
    }

    /// <inheritdoc />
    public Task<PrettyCardsDeleteResult> DeleteAsync(PrettyCardsDeleteParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => Delete(@params));
    }

    /// <inheritdoc />
    public Task<PrettyCardsEditResult> EditAsync(PrettyCardsEditParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => Edit(@params));
    }

    /// <inheritdoc />
    public Task<VkCollection<PrettyCardsGetByIdResult>> GetAsync(PrettyCardsGetParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => Get(@params));
    }

    /// <inheritdoc />
    public Task<ReadOnlyCollection<PrettyCardsGetByIdResult>> GetByIdAsync(PrettyCardsGetByIdParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => GetById(@params));
    }

    /// <inheritdoc />
    public Task<Uri> GetUploadUrlAsync()
    {
        return TypeHelper.TryInvokeMethodAsync(GetUploadUrl);
    }
}