namespace UrlShortener.Dtos
{
    public class ShortUrlDto
    {
        public Guid Id { get; set; }

        public string OriginalUrl { get; set; }

        public string ShortenedUrl { get; set; }
    }
}
