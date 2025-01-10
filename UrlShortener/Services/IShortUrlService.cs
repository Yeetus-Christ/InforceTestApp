using UrlShortener.Dtos;
using UrlShortener.Models;

namespace UrlShortener.Services
{
    public interface IShortUrlService
    {
        Task<IEnumerable<ShortUrlDto>> GetAllShortUrlsAsync();
        Task<ShortUrlFullDto> GetShortUrlByIdAsync(Guid id);
        Task<string> CreateShortUrlAsync(string originalUrl, Guid createdBy);
        Task<bool> DeleteShortUrlAsync(Guid id, Guid requestingUser);
    }
}
