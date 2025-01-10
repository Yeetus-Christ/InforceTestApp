using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Text;
using UrlShortener.Data;
using UrlShortener.Dtos;
using UrlShortener.Models;

namespace UrlShortener.Services
{
    public class ShortUrlService : IShortUrlService
    {
        private readonly AppDbContext _context;

        public ShortUrlService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ShortUrlDto>> GetAllShortUrlsAsync()
        {
            var urls = await _context.ShortUrls.ToListAsync();

            var result = new List<ShortUrlDto>();

            foreach (var url in urls) 
            {
                result.Add(new ShortUrlDto { 
                    Id = url.Id,
                    OriginalUrl = url.OriginalUrl,
                    ShortenedUrl = url.ShortenedUrl
                });
            }

            return result;
        }

        public async Task<ShortUrlFullDto> GetShortUrlByIdAsync(Guid id)
        {
            var result = await _context.ShortUrls.Include(u => u.User).FirstOrDefaultAsync(u => u.Id == id);

            ArgumentNullException.ThrowIfNull(result);

            return new ShortUrlFullDto { 
                Id = result.Id,
                OriginalUrl = result.OriginalUrl,
                ShortenedUrl = result.ShortenedUrl,
                CreatedDate = result.CreatedDate,
                CreatedBy = result.User.Username
            };
        }

        public async Task<string> CreateShortUrlAsync(string originalUrl, Guid createdBy)
        {
            if (_context.ShortUrls.Any(u => u.OriginalUrl == originalUrl))
                throw new Exception("URL already exists.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == createdBy);

            ArgumentNullException.ThrowIfNull(nameof(user));

            var shortUrl = ConvertToShortUrl(originalUrl);
            var newShortUrl = new ShortUrl
            {
                OriginalUrl = originalUrl,
                ShortenedUrl = shortUrl,
                CreatedDate = DateTime.UtcNow,
                User = user!
            };

            _context.ShortUrls.Add(newShortUrl);
            await _context.SaveChangesAsync();

            return shortUrl;
        }

        public async Task<bool> DeleteShortUrlAsync(Guid id, Guid requestingUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == requestingUser);

            ArgumentNullException.ThrowIfNull(nameof(user));

            var shortUrl = await _context.ShortUrls.Include(u => u.User).FirstOrDefaultAsync(u => u.Id == id);
            if (shortUrl == null) return false;

            if (shortUrl.User.Id != requestingUser && user!.Role != "Admin")
                throw new UnauthorizedAccessException("You cannot delete this URL.");

            _context.ShortUrls.Remove(shortUrl);
            await _context.SaveChangesAsync();

            return true;
        }

        private string ConvertToShortUrl(string originalUrl)
        {
            var base62Chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var sb = new StringBuilder();
            var hash = originalUrl.GetHashCode();

            while (hash > 0)
            {
                sb.Append(base62Chars[hash % 62]);
                hash /= 62;
            }

            return $"https://short.ly/{sb}";
        }
    }
}
