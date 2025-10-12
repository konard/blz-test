using BlazorTest.Model.Contracts.DTOs.Authentication;

namespace BlazorTest.Model.Contracts.Services;

/// <summary>
/// Service interface for authentication operations
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// Authenticates a user with email and password
    /// </summary>
    /// <param name="request">Login request</param>
    /// <returns>Login response with access token and user information</returns>
    Task<LoginResponse> LoginAsync(LoginRequest request);

    /// <summary>
    /// Refreshes an access token using a refresh token
    /// </summary>
    /// <param name="refreshToken">Refresh token</param>
    /// <returns>New access token</returns>
    Task<TokenResponse> RefreshTokenAsync(string refreshToken);

    /// <summary>
    /// Logs out a user by revoking their refresh token
    /// </summary>
    /// <param name="refreshToken">Refresh token to revoke</param>
    Task LogoutAsync(string refreshToken);

    /// <summary>
    /// Logs out a user from all devices by revoking all refresh tokens
    /// </summary>
    /// <param name="userId">User identifier</param>
    Task LogoutAllAsync(string userId);
}

