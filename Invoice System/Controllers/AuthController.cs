using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Invoice_System.DTOs.Auth;
using Invoice_System.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Invoice_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService auth, ILogger<AuthController> logger)
        {
            _auth = auth;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            try
            {
                if (dto == null)
                {
                    _logger.LogWarning("Register request with null body");
                    return BadRequest(new { error = "Request body required" });
                }

                var result = await _auth.RegisterAsync(dto);
                return Ok(new { message = result });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation failed during registration");
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Registration failed");
                return BadRequest(new { error = ex.Message });
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                if (dto == null)
                {
                    _logger.LogWarning("Login request with null body");
                    return BadRequest(new { error = "Request body required" });
                }

                dto.Username = dto.Username?.Trim();
                dto.Password = dto.Password?.Trim();

                if (string.IsNullOrWhiteSpace(dto.Username))
                {
                    _logger.LogWarning("Login attempt with empty username");
                    return BadRequest(new { error = "Username required" });
                }

                if (string.IsNullOrWhiteSpace(dto.Password))
                {
                    _logger.LogWarning("Login attempt with empty password");
                    return BadRequest(new { error = "Password required" });
                }

                var token = await _auth.LoginAsync(dto);
                var role = GetUserRoleFromToken(token);

                _logger.LogInformation($"Successful login for user: {dto.Username}");
                return Ok(new
                {
                    token,
                    expiresIn = DateTime.UtcNow.AddHours(1),
                    role
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation failed during login");
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, $"Login failed for user: {dto?.Username}");
                return Unauthorized(new
                {
                    error = "Login failed",
                    message = ex.Message
                });
            }
        }

        private string GetUserRoleFromToken(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                return jwtToken.Claims.First(c => c.Type == ClaimTypes.Role).Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to extract role from token");
                throw new InvalidOperationException("Invalid token format");
            }
        }
    }
}