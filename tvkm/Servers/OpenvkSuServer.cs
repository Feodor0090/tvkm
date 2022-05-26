namespace tvkm.Servers;

public class OpenvkSuServer : IServerProvider
{
    public virtual string ReadableName => "openvk";
    public virtual string BaseApiUrl => "https://openvk.su";
    public virtual string BaseAuthUrl => "https://openvk.su";

    public string GetAuthUrl(string login, string password, string? code)
    {
        return
            $"/token?username={login}&password={password}&grant_type=password{(code == null ? "" : $"&code={code}")}";
    }
}