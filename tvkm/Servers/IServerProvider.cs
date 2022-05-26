namespace tvkm.Servers;

public interface IServerProvider
{
    string BaseApiUrl { get; }
    string BaseAuthUrl { get; }
    string GetAuthUrl(string login, string password, string? code);
}