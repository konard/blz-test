using BlazorTest.Model.Domain.ValueObjects;

namespace BlazorTest.Model.Contracts.Repositories;

/// <summary>
/// Repository interface for refresh token entities
/// </summary>
public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    /// <summary>
    /// Gets a refresh token by the token string
    /// </summary>
    /// <param name="token">Token string</param>
    /// <returns>RefreshToken if found, null otherwise</returns>
    Task<RefreshToken?> GetByTokenAsync(string token);

    /// <summary>
    /// Gets all refresh tokens for a specific user
    /// </summary>
    /// <param name="userId">User identifier</param>
    /// <returns>Collection of refresh tokens</returns>
    Task<IEnumerable<RefreshToken>> GetByUserIdAsync(string userId);

    /// <summary>
    /// Revokes a refresh token
    /// </summary>
    /// <param name="token">Token string to revoke</param>
    /// <returns>True if successful, false otherwise</returns>
    Task<bool> RevokeTokenAsync(string token);

    /// <summary>
    /// Revokes all refresh tokens for a specific user
    /// </summary>
    /// <param name="userId">User identifier</param>
    /// <returns>Number of tokens revoked</returns>
    Task<int> RevokeAllUserTokensAsync(string userId);

    /// <summary>
    /// Deletes all expired refresh tokens
    /// </summary>
    /// <returns>Number of tokens deleted</returns>
    Task<int> DeleteExpiredTokensAsync();
}

