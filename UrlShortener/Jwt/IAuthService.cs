namespace UrlShortener.Jwt
{
    public interface IAuthService
    {
        string? Authenticate(string username, string password);
    }
}
