using System.Security.Claims;
using BCrypt.Net;
using Invoice_System.Data;
using Invoice_System.DTOs.Auth;
using Invoice_System.Helpers;
using Invoice_System.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Invoice_System.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtTokenService _jwt;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(
            ApplicationDbContext context,
            JwtTokenService jwt,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _jwt = jwt;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            // Input validation
            if (string.IsNullOrWhiteSpace(dto.Username?.Trim()))
                throw new ArgumentException("Username is required");

            if (string.IsNullOrWhiteSpace(dto.Password?.Trim()))
                throw new ArgumentException("Password is required");

            dto.Username = dto.Username.Trim();
            dto.Password = dto.Password.Trim();

            // User lookup (case-insensitive)
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username.ToLower() == dto.Username.ToLower());

            // Security: Generic error message
            if (user == null || string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                await Task.Delay(Random.Shared.Next(100, 300));
                throw new Exception("Invalid username or password");
            }

            // Password verification with enhanced entropy
            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(
                dto.Password,
                user.PasswordHash,
                enhancedEntropy: true
            );

            if (!isPasswordValid)
            {
                await Task.Delay(Random.Shared.Next(100, 300));
                throw new Exception("Invalid username or password");
            }

            return _jwt.GenerateToken(user);
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            // Input validation
            if (string.IsNullOrWhiteSpace(dto.Username?.Trim()))
                throw new ArgumentException("Username is required");

            if (string.IsNullOrWhiteSpace(dto.Password?.Trim()))
                throw new ArgumentException("Password is required");

            dto.Username = dto.Username.Trim();
            dto.Password = dto.Password.Trim();

            // Check existing user
            if (await _context.Users.AnyAsync(u => u.Username.ToLower() == dto.Username.ToLower()))
                throw new Exception("Username already exists");

            // First user becomes admin
            var isFirstUser = !await _context.Users.AnyAsync();
            var role = isFirstUser ? UserRole.Admin :
                      (string.IsNullOrWhiteSpace(dto.Role) ? UserRole.Cashier :
                       Enum.Parse<UserRole>(dto.Role));

            // Create user with hashed password
            var user = new User
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password, 11),
                Role = role
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return $"User registered successfully as {role}";
        }
    }
}