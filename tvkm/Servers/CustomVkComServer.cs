namespace tvkm.Servers;

public class CustomVkComServer : VkComServer
{
    public override string BaseApiUrl => Settings.ApiServer;
    public override string BaseAuthUrl => Settings.AuthServer;
}