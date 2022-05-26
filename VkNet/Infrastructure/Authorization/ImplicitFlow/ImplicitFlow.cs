using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using VkNet.Abstractions.Authorization;
using VkNet.Abstractions.Core;
using VkNet.Enums.SafetyEnums;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Utils;

namespace VkNet.Infrastructure.Authorization.ImplicitFlow;

/// <inheritdoc />
public class ImplicitFlow : IImplicitFlow
{
    [NotNull] private readonly IAuthorizationFormFactory _authorizationFormsFactory;

    /// <summary>
    ///     Менеджер версий VkApi
    /// </summary>
    private readonly IVkApiVersionManager _versionManager;

    private readonly IVkAuthorization<ImplicitFlowPageType> _vkAuthorization;

    private IApiAuthParams _authorizationParameters;

    /// <inheritdoc cref="ImplicitFlow" />
    public ImplicitFlow(
        IVkApiVersionManager versionManager,
        IAuthorizationFormFactory authorizationFormsFactory,
        IVkAuthorization<ImplicitFlowPageType> vkAuthorization)
    {
        _versionManager = versionManager;
        _authorizationFormsFactory = authorizationFormsFactory;
        _vkAuthorization = vkAuthorization;
    }

    /// <inheritdoc />
    public async Task<AuthorizationResult> AuthorizeAsync()
    {
        ValidateAuthorizationParameters();

        var authorizeUrlResult = CreateAuthorizeUrl();

        var loginFormResult = await _authorizationFormsFactory.Create(ImplicitFlowPageType.LoginPassword)
            .ExecuteAsync(authorizeUrlResult, _authorizationParameters)
            .ConfigureAwait(false);

        return await NextStepAsync(loginFormResult).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public void SetAuthorizationParams(IApiAuthParams authorizationParams)
    {
        _authorizationParameters = authorizationParams;
    }

    /// <inheritdoc />
    [Obsolete(
        "Используйте перегрузку Url CreateAuthorizeUrl();\nПараметры авторизации должны быть уставленны вызовом void SetAuthorizationParams(IApiAuthParams authorizationParams);")]
    public Uri CreateAuthorizeUrl(ulong clientId, ulong scope, Display display, string state)
    {
        _authorizationParameters.ApplicationId = clientId;
        _authorizationParameters.Display = display;
        _authorizationParameters.State = state;

        return CreateAuthorizeUrl();
    }

    /// <inheritdoc />
    public Uri CreateAuthorizeUrl()
    {
        const string url = "https://oauth.vk.com/authorize?";

        var vkAuthParams = new VkParameters
        {
            {"client_id", _authorizationParameters.ApplicationId},
            {
                "redirect_uri", _authorizationParameters.RedirectUri != null
                    ? _authorizationParameters.RedirectUri.ToString()
                    : Constants.DefaultRedirectUri
            },
            {"display", Display.Mobile},
            {"scope", _authorizationParameters.Settings?.ToUInt64()},
            {"response_type", ResponseType.Token},
            {"v", _versionManager.Version},
            {"state", _authorizationParameters.State},
            {"revoke", _authorizationParameters.Revoke}
        };

        var resultUrl = Url.Combine(url, Url.QueryFrom(vkAuthParams.ToArray()));

        return new Uri(resultUrl);
    }

    private async Task<AuthorizationResult> NextStepAsync(AuthorizationFormResult formResult)
    {
        var responseUrl = formResult.ResponseUrl;

        if (responseUrl.OriginalString.StartsWith("https://oauth.vk.com/auth_redirect"))
            responseUrl = GetRedirectUrl(responseUrl);

        var pageType = _vkAuthorization.GetPageType(responseUrl);

        switch (pageType)
        {
            case ImplicitFlowPageType.Error:

            {
                throw new VkAuthorizationException("При авторизации произошла ошибка.");
            }

            case ImplicitFlowPageType.LoginPassword:

            {
                throw new VkAuthorizationException("Неверный логин или пароль.");
            }

            case ImplicitFlowPageType.Captcha:

            {
                break;
            }

            case ImplicitFlowPageType.TwoFactor:

            {
                break;
            }

            case ImplicitFlowPageType.Consent:

            {
                break;
            }

            case ImplicitFlowPageType.Result:

            {
                return _vkAuthorization.GetAuthorizationResult(responseUrl);
            }
        }

        var resultForm = await _authorizationFormsFactory.Create(pageType)
            .ExecuteAsync(responseUrl, _authorizationParameters)
            .ConfigureAwait(false);

        return await NextStepAsync(resultForm).ConfigureAwait(false);
    }

    private static Uri GetRedirectUrl(Uri originalUrl)
    {
        var originalString = originalUrl.OriginalString;
        var query = Url.ParseQueryString(originalString);

        if (!query.ContainsKey("authorize_url")) return originalUrl;

        var escapedUrl = query["authorize_url"];
        var unEscapedUrl = Uri.UnescapeDataString(escapedUrl);

        return new Uri(unEscapedUrl);
    }

    /// <summary>
    ///     Валидация параметров авторизации
    /// </summary>
    /// <exception cref="VkApiException"> Список неверно заданных параметров </exception>
    private void ValidateAuthorizationParameters()
    {
        var errorsBuilder = new StringBuilder();

        if (_authorizationParameters == null)
        {
            errorsBuilder.AppendLine("Параметры авторизации не установленны");
        }
        else
        {
            if (_authorizationParameters.ApplicationId == 0)
                errorsBuilder.AppendLine($"{nameof(_authorizationParameters.ApplicationId)} обязательный параметр");

            if (string.IsNullOrWhiteSpace(_authorizationParameters.Login))
                errorsBuilder.AppendLine($"{nameof(_authorizationParameters.Login)} обязательный параметр");

            if (string.IsNullOrWhiteSpace(_authorizationParameters.Password))
                errorsBuilder.AppendLine($"{nameof(_authorizationParameters.Password)} обязательный параметр");
        }

        var errors = errorsBuilder.ToString();

        if (!string.IsNullOrWhiteSpace(errors)) throw new VkAuthorizationException(errors);
    }
}