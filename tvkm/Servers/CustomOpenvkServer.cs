namespace tvkm.Servers;

public class CustomOpenvkServer : OpenvkSuServer
{
    public virtual string ReadableName => "openvk (custom)";
    public override string BaseApiUrl => Settings.CustomApiServer;
    public override string BaseAuthUrl => Settings.CustomAuthServer;
}