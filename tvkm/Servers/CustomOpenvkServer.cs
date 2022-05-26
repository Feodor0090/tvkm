namespace tvkm.Servers;

public class CustomOpenvkServer : OpenvkSuServer
{
    public override string BaseApiUrl => Settings.ApiServer;
    public override string BaseAuthUrl => Settings.AuthServer;
}