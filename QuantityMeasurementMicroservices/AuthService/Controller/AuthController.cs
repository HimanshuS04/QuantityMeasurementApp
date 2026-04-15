using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using AuthService.Data;
using AuthService.Models;
using AuthService.Security;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IConfiguration configuration;

        public AuthController(
            IUserRepository userRepository,
            IConfiguration configuration)
        {
            this.userRepository = userRepository;
            this.configuration = configuration;
        }

        public class RegisterRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class LoginRequest
        {
            public string Email { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class LoginResponse
        {
            public string Token { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Role { get; set; } = string.Empty;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Email and password are required.");

            var existing = await userRepository.GetByEmailAsync(request.Email);
            if (existing != null)
                return Conflict("A user with this email already exists.");

            var (hash, salt) = PasswordHasher.HashPassword(request.Password);

            var user = new User
            {
                Email = request.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                Role = ResolveRole(request.Email),
                CreatedAt = DateTime.UtcNow
            };

            await userRepository.CreateAsync(user);
            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Email and password are required.");

            var user = await userRepository.GetByEmailAsync(request.Email);
            if (user == null)
                return Unauthorized("Invalid credentials.");

            if (!PasswordHasher.VerifyPassword(
                request.Password, user.PasswordSalt, user.PasswordHash))
                return Unauthorized("Invalid credentials.");

            string token = GenerateJwtToken(user);

            return Ok(new LoginResponse
            {
                Token = token,
                Email = user.Email,
                Role = user.Role
            });
        }

        private string ResolveRole(string email)
        {
            string? adminEmail = configuration["AdminUser:Email"];
            if (!string.IsNullOrWhiteSpace(adminEmail) &&
                string.Equals(adminEmail, email, StringComparison.OrdinalIgnoreCase))
                return "Admin";
            return "User";
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSection = configuration.GetSection("Jwt");
            string key = jwtSection["Key"]!;
            string issuer = jwtSection["Issuer"] ?? "QuantityMeasurementApi";
            string audience = jwtSection["Audience"] ?? "QuantityMeasurementApiUsers";
            int expiresMinutes = int.TryParse(jwtSection["ExpiresMinutes"], out int m) ? m : 60;

            var keyBytes = Encoding.UTF8.GetBytes(key);
            var securityKey = new SymmetricSecurityKey(keyBytes);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("role", user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}