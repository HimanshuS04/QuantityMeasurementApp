using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuantityMeasurementApp;
using QuantityMeasurementApi.Security;

namespace QuantityMeasurementApi.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IConfiguration configuration;

        public AuthController(IUserRepository userRepository, IConfiguration configuration)
        {
            this.userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
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
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Email and password are required.");
            }

            var existingUser = await userRepository.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return Conflict("A user with this email already exists.");
            }

            var (hash, salt) = PasswordHasher.HashPassword(request.Password);

            var user = new User
            {
                Email = request.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                CreatedAt = DateTime.UtcNow
            };

            await userRepository.CreateAsync(user);

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Email and password are required.");
            }

            var user = await userRepository.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            bool valid = PasswordHasher.VerifyPassword(request.Password, user.PasswordSalt, user.PasswordHash);
            if (!valid)
            {
                return Unauthorized("Invalid credentials.");
            }

            string token = GenerateJwtToken(user);
            return Ok(new LoginResponse { Token = token });
        }

        private string GenerateJwtToken(User user)
        {
            var jwtSection = configuration.GetSection("Jwt");
            string key = jwtSection["Key"] ?? throw new InvalidOperationException("Jwt:Key is missing.");
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