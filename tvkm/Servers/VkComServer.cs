namespace tvkm.Servers;

public class VkComServer : IServerProvider
{
    public virtual string ReadableName => "vk.com";
    public virtual string BaseApiUrl => "https://api.vk.com";
    public virtual string BaseAuthUrl => "https://oauth.vk.com";

    private const int VkmId = 2685278;
    private const string VkmSecret = "lxhD8OD7dMsqtXIm5IUY";

    public string GetAuthUrl(string login, string password, string? code)
    {
        return "/token?grant_type=password" +
               "&client_id=" + VkmId + "&client_secret=" + VkmSecret + "&username=" + login +
               "&password=" + password +
               "&2fa_supported=1&scope=notify,friends,photos,audio,video,docs,notes,pages,status,offers,questions,wall,groups,messages,notifications,stats,ads" +
               (code == null ? "" : $"&code={code}");
    }
}