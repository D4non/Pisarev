using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Project.Models.DTO;
using Project.Models.Entities;
using Project.Repositories.Interfaces;
using Project.Services.Interfaces;

namespace Project.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUserRepository userRepository,
        IConfiguration configuration,
        ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<TokenResponseDto> LoginAsync(LoginDto dto)
    {
        _logger.LogInformation("Attempting login for user {Username}", dto.Username);

        var user = await _userRepository.GetByUsernameAsync(dto.Username);
        if (user == null)
        {
            _logger.LogWarning("Login failed: user {Username} not found", dto.Username);
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        if (!VerifyPassword(dto.Password, user.PasswordHash))
        {
            _logger.LogWarning("Login failed: invalid password for user {Username}", dto.Username);
            throw new UnauthorizedAccessException("Invalid username or password");
        }

        var token = GenerateJwtToken(user);
        _logger.LogInformation("User {Username} logged in successfully", dto.Username);

        return new TokenResponseDto
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "60")),
            Role = user.Role
        };
    }

    public async Task<TokenResponseDto> RegisterAsync(RegisterDto dto)
    {
        _logger.LogInformation("Attempting registration for user {Username}", dto.Username);

        var existingUser = await _userRepository.GetByUsernameAsync(dto.Username);
        if (existingUser != null)
        {
            throw new InvalidOperationException("Username already exists");
        }

        var existingEmail = await _userRepository.GetByEmailAsync(dto.Email);
        if (existingEmail != null)
        {
            throw new InvalidOperationException("Email already exists");
        }

        var passwordHash = HashPassword(dto.Password);
        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = passwordHash,
            Role = dto.Role
        };

        var created = await _userRepository.CreateAsync(user);
        _logger.LogInformation("User {Username} registered successfully", dto.Username);

        var token = GenerateJwtToken(created);
        return new TokenResponseDto
        {
            Token = token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["JwtSettings:ExpirationMinutes"] ?? "60")),
            Role = created.Role
        };
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpirationMinutes"] ?? "60")),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private static bool VerifyPassword(string password, string hash)
    {
        var passwordHash = HashPassword(password);
        return passwordHash == hash;
    }
}

