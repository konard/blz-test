using BlazorTest.Model.Domain.Identity;
using System.Security.Claims;

namespace BlazorTest.Host.Services.Authentication;

/// <summary>
/// Service interface for JWT token operations
/// </summary>
public interface ITokenService
{
    /// <summary>
    /// Generates a JWT access token for the specified user
    /// </summary>
    /// <param name="user">User to generate token for</param>
    /// <returns>JWT access token string</returns>
    string GenerateAccessToken(IUser user);

    /// <summary>
    /// Generates a refresh token
    /// </summary>
    /// <returns>Refresh token string</returns>
    string GenerateRefreshToken();

    /// <summary>
    /// Validates a JWT access token and returns the claims principal
    /// </summary>
    /// <param name="token">JWT access token</param>
    /// <returns>Claims principal if valid, null otherwise</returns>
    ClaimsPrincipal? ValidateAccessToken(string token);

    /// <summary>
    /// Gets the access token expiration time in minutes
    /// </summary>
    int AccessTokenExpirationMinutes { get; }

    /// <summary>
    /// Gets the refresh token expiration time in days
    /// </summary>
    int RefreshTokenExpirationDays { get; }
}

