namespace tvkm.Servers;

public interface IServerProvider
{
    string ReadableName { get; }
    string BaseApiUrl { get; }
    string BaseAuthUrl { get; }
    string GetAuthUrl(string login, string password, string? code);
}