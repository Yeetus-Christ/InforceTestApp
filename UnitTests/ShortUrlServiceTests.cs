using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using UrlShortener.Data;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UnitTests
{
    public class ShortUrlServiceTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly ShortUrlService _service;

        public ShortUrlServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new AppDbContext(options);
            _service = new ShortUrlService(_context);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetAllShortUrlsAsync_ShouldReturnListOfShortUrls()
        {
            // Arrange
            var shortUrls = new List<ShortUrl>
        {
            new ShortUrl { Id = Guid.NewGuid(), OriginalUrl = "https://example.com", ShortenedUrl = "https://short.ly/abc123" },
            new ShortUrl { Id = Guid.NewGuid(), OriginalUrl = "https://google.com", ShortenedUrl = "https://short.ly/xyz456" }
        };

            _context.ShortUrls.AddRange(shortUrls);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetAllShortUrlsAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetShortUrlByIdAsync_ShouldReturnShortUrl()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Username = "testuser", PasswordHash = "hash", Role = "User" };
            var shortUrl = new ShortUrl { Id = Guid.NewGuid(), OriginalUrl = "https://example.com", ShortenedUrl = "https://short.ly/abc123", User = user };

            _context.Users.Add(user);
            _context.ShortUrls.Add(shortUrl);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetShortUrlByIdAsync(shortUrl.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(shortUrl.OriginalUrl, result.OriginalUrl);
            Assert.Equal(shortUrl.ShortenedUrl, result.ShortenedUrl);
        }

        [Fact]
        public async Task CreateShortUrlAsync_ShouldCreateShortUrl()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Username = "testuser", PasswordHash = "hash", Role = "User" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            string originalUrl = "https://newurl.com";

            // Act
            var result = await _service.CreateShortUrlAsync(originalUrl, user.Id);

            // Assert
            Assert.NotNull(result);
            var createdShortUrl = await _context.ShortUrls.FirstOrDefaultAsync(u => u.OriginalUrl == originalUrl);
            Assert.NotNull(createdShortUrl);
            Assert.Equal(originalUrl, createdShortUrl.OriginalUrl);
        }

        [Fact]
        public async Task CreateShortUrlAsync_ShouldThrowExceptionWhenUrlExists()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Username = "testuser", PasswordHash = "hash", Role = "User" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            string originalUrl = "https://existingurl.com";
            await _service.CreateShortUrlAsync(originalUrl, user.Id);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _service.CreateShortUrlAsync(originalUrl, user.Id));
        }

        [Fact]
        public async Task DeleteShortUrlAsync_ShouldDeleteShortUrl()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Username = "testuser", PasswordHash = "hash", Role = "User" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var shortUrl = new ShortUrl { Id = Guid.NewGuid(), OriginalUrl = "https://deleteurl.com", ShortenedUrl = "https://short.ly/def789", User = user };
            _context.ShortUrls.Add(shortUrl);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.DeleteShortUrlAsync(shortUrl.Id, user.Id);

            // Assert
            Assert.True(result);
            var deletedShortUrl = await _context.ShortUrls.FindAsync(shortUrl.Id);
            Assert.Null(deletedShortUrl);
        }

        [Fact]
        public async Task DeleteShortUrlAsync_ShouldThrowUnauthorizedAccessExceptionWhenUserIsNotOwner()
        {
            // Arrange
            var user1 = new User { Id = Guid.NewGuid(), Username = "testuser1", PasswordHash = "hash", Role = "User" };
            var user2 = new User { Id = Guid.NewGuid(), Username = "testuser2", PasswordHash = "hash", Role = "User" };

            _context.Users.AddRange(user1, user2);
            await _context.SaveChangesAsync();

            var shortUrl = new ShortUrl { Id = Guid.NewGuid(), OriginalUrl = "https://deleteurl.com", ShortenedUrl = "https://short.ly/def789", User = user1 };
            _context.ShortUrls.Add(shortUrl);
            await _context.SaveChangesAsync();

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _service.DeleteShortUrlAsync(shortUrl.Id, user2.Id));
        }

        [Fact]
        public async Task DeleteShortUrlAsync_ShouldReturnFalseWhenUrlDoesNotExist()
        {
            // Arrange
            var user = new User { Id = Guid.NewGuid(), Username = "testuser", PasswordHash = "hash", Role = "User" };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.DeleteShortUrlAsync(Guid.NewGuid(), user.Id);

            // Assert
            Assert.False(result);
        }
    }
}