using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models
{
    public class ShortUrl
    {
        [Key]
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string OriginalUrl { get; set; }

        [Required]
        public string ShortenedUrl { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        public User User { get; set; }
    }
}
