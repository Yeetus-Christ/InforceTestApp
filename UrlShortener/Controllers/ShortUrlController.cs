using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShortUrlController : ControllerBase
    {
        private readonly IShortUrlService _shortUrlService;

        public ShortUrlController(IShortUrlService shortUrlService)
        {
            _shortUrlService = shortUrlService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var urls = await _shortUrlService.GetAllShortUrlsAsync();
            return Ok(urls);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(Guid id)
        {
            var url = await _shortUrlService.GetShortUrlByIdAsync(id);
            if (url == null) return NotFound("URL not found.");

            return Ok(url);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] string originalUrl)
        {
            try
            {
                var createdBy = GetUserIdFromJwt();
                var shortUrl = await _shortUrlService.CreateShortUrlAsync(originalUrl, createdBy);
                return Ok(new { ShortUrl = shortUrl });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var requestingUser = GetUserIdFromJwt();
                var result = await _shortUrlService.DeleteShortUrlAsync(id, requestingUser);
                if (!result) return NotFound("URL not found.");

                return NoContent();
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
        }

        private Guid GetUserIdFromJwt()
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var decryptedToken = handler.ReadToken(token);

            var jwtSecurityToken = decryptedToken as JwtSecurityToken;
            return Guid.Parse(jwtSecurityToken!.Claims.First(c => c.Type == "UserId").Value);
        }
    }
}
