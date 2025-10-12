using BlazorTest.Model.Contracts.Repositories;
using BlazorTest.Model.Domain.ValueObjects;

namespace BlazorTest.Host.Infrastructure.Data.Repositories;

/// <summary>
/// Repository for refresh token data access operations.
/// </summary>
public class RefreshTokenRepository : Repository<RefreshToken>, IRefreshTokenRepository
{
    /// <summary>
    /// Initializes a new instance of the RefreshTokenRepository.
    /// </summary>
    /// <param name="context">The database context</param>
    public RefreshTokenRepository(IDbContext context) : base(context, "refresh_tokens")
    {
        // Create indexes for efficient lookups
        _collection.EnsureIndex(x => x.Token);
        _collection.EnsureIndex(x => x.UserId);
        _collection.EnsureIndex(x => x.ExpiresAt);
    }

    /// <inheritdoc/>
    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Token cannot be empty", nameof(token));

        var results = await FindAsync(t => t.Token == token);
        return results.FirstOrDefault();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<RefreshToken>> GetByUserIdAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("UserId cannot be empty", nameof(userId));

        var results = await FindAsync(t => t.UserId == userId && !t.IsRevoked);
        return results;
    }

    /// <inheritdoc/>
    public async Task<bool> RevokeTokenAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Token cannot be empty", nameof(token));

        var refreshToken = await GetByTokenAsync(token);
        if (refreshToken == null)
            return false;

        refreshToken.IsRevoked = true;
        refreshToken.RevokedAt = DateTime.UtcNow;

        return await UpdateAsync(refreshToken);
    }

    /// <inheritdoc/>
    public async Task<int> RevokeAllUserTokensAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("UserId cannot be empty", nameof(userId));

        var tokens = await GetByUserIdAsync(userId);
        var count = 0;

        foreach (var token in tokens)
        {
            token.IsRevoked = true;
            token.RevokedAt = DateTime.UtcNow;
            if (await UpdateAsync(token))
                count++;
        }

        return count;
    }

    /// <inheritdoc/>
    public async Task<int> DeleteExpiredTokensAsync()
    {
        var expiredTokens = await FindAsync(t => t.ExpiresAt < DateTime.UtcNow);
        var count = 0;

        foreach (var token in expiredTokens)
        {
            if (await DeleteAsync(token.Id))
                count++;
        }

        return count;
    }
}

