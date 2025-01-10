using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.Jwt;

namespace UrlShortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public ActionResult<string> Login([FromBody] LoginRequest request)
        {
            var result = _authService.Authenticate(request.Username, request.Password);

            if (result == null)
            {
                return Unauthorized("Invalid username or password");
            }

            return Ok(new { token = result });
        }

        public class LoginRequest
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
