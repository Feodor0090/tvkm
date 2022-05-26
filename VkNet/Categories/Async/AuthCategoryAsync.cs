using System.Threading.Tasks;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace VkNet.Categories;

/// <inheritdoc />
public partial class AuthCategory
{
    /// <inheritdoc />
    public Task<bool> CheckPhoneAsync(string phone, string clientSecret, long? clientId = null,
        bool? authByPhone = null)
    {
        return TypeHelper.TryInvokeMethodAsync(() =>
            CheckPhone(phone, clientSecret, clientId, authByPhone));
    }

    /// <inheritdoc />
    public Task<string> SignupAsync(AuthSignupParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => Signup(@params));
    }

    /// <inheritdoc />
    public Task<AuthConfirmResult> ConfirmAsync(AuthConfirmParams @params)
    {
        return TypeHelper.TryInvokeMethodAsync(() => Confirm(@params));
    }

    /// <inheritdoc />
    public Task<string> RestoreAsync(string phone, string lastName)
    {
        return TypeHelper.TryInvokeMethodAsync(() => Restore(phone, lastName));
    }
}