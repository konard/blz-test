using BlazorTest.Model.Domain.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BlazorTest.Host.Services.Authentication;

/// <summary>
/// Service for JWT token generation and validation
/// </summary>
public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<TokenService> _logger;
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;
    private readonly int _accessTokenExpirationMinutes;
    private readonly int _refreshTokenExpirationDays;

    /// <summary>
    /// Creates a new token service instance
    /// </summary>
    public TokenService(IConfiguration configuration, ILogger<TokenService> logger)
    {
        _configuration = configuration;
        _logger = logger;

        _secretKey = _configuration["Authentication:Jwt:SecretKey"]
            ?? throw new InvalidOperationException("JWT Secret Key is not configured");
        _issuer = _configuration["Authentication:Jwt:Issuer"] ?? "BlazorTest";
        _audience = _configuration["Authentication:Jwt:Audience"] ?? "BlazorTest.Clients";
        _accessTokenExpirationMinutes = int.Parse(_configuration["Authentication:Jwt:AccessTokenExpirationMinutes"] ?? "15");
        _refreshTokenExpirationDays = int.Parse(_configuration["Authentication:Jwt:RefreshTokenExpirationDays"] ?? "7");

        if (_secretKey.Length < 32)
        {
            throw new InvalidOperationException("JWT Secret Key must be at least 32 characters long");
        }
    }

    /// <inheritdoc/>
    public int AccessTokenExpirationMinutes => _accessTokenExpirationMinutes;

    /// <inheritdoc/>
    public int RefreshTokenExpirationDays => _refreshTokenExpirationDays;

    /// <inheritdoc/>
    public string GenerateAccessToken(IUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(Model.Constants.ClaimTypes.UserId, user.Id),
            new Claim(Model.Constants.ClaimTypes.UserName, user.UserName)
        };

        // Add roles
        foreach (var role in user.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
            claims.Add(new Claim(Model.Constants.ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_accessTokenExpirationMinutes),
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        _logger.LogDebug("Generated access token for user {UserId}", user.Id);

        return tokenString;
    }

    /// <inheritdoc/>
    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    /// <inheritdoc/>
    public ClaimsPrincipal? ValidateAccessToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secretKey);

        try
        {
            var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _issuer,
                ValidateAudience = true,
                ValidAudience = _audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            return principal;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Token validation failed");
            return null;
        }
    }
}

