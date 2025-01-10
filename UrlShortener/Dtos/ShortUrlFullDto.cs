namespace UrlShortener.Dtos
{
    public class ShortUrlFullDto
    {
        public Guid Id { get; set; }

        public string OriginalUrl { get; set; }

        public string ShortenedUrl { get; set; }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }
    }
}
