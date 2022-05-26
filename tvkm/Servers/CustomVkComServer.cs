namespace tvkm.Servers;

public class CustomVkComServer : VkComServer
{
    public virtual string ReadableName => "vk.com (custom)";
    public override string BaseApiUrl => Settings.CustomApiServer;
    public override string BaseAuthUrl => Settings.CustomAuthServer;
}