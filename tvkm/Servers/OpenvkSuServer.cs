namespace tvkm.Servers;

public class OpenvkSuServer : IServerProvider
{
    public string BaseApiUrl => "https://openvk.su";
    public string BaseAuthUrl => "https://openvk.su";

    public string GetAuthUrl(string login, string password, string? code)
    {
        return
            $"/token?username={login}&password={password}&grant_type=password{(code == null ? "" : $"&code={code}")}";
    }
}