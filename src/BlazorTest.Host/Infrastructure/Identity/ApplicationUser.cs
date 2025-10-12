using BlazorTest.Model.Domain.Identity;

namespace BlazorTest.Host.Infrastructure.Identity;

/// <summary>
/// Application user entity implementation
/// </summary>
public class ApplicationUser : IUser
{
    /// <inheritdoc/>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <inheritdoc/>
    public string UserName { get; set; } = string.Empty;

    /// <inheritdoc/>
    public string Email { get; set; } = string.Empty;

    /// <inheritdoc/>
    public string PasswordHash { get; set; } = string.Empty;

    /// <inheritdoc/>
    public List<string> Roles { get; set; } = new();

    /// <inheritdoc/>
    public bool IsActive { get; set; } = true;

    /// <inheritdoc/>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <inheritdoc/>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// Refresh token for this user
    /// </summary>
    public string? RefreshToken { get; set; }

    /// <summary>
    /// Refresh token expiration timestamp
    /// </summary>
    public DateTime? RefreshTokenExpiry { get; set; }

    /// <summary>
    /// Normalized email for lookups
    /// </summary>
    public string NormalizedEmail { get; set; } = string.Empty;

    /// <summary>
    /// Normalized username for lookups
    /// </summary>
    public string NormalizedUserName { get; set; } = string.Empty;

    /// <summary>
    /// Security stamp for security-sensitive operations
    /// </summary>
    public string SecurityStamp { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Concurrency stamp for optimistic concurrency
    /// </summary>
    public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Number of failed login attempts
    /// </summary>
    public int AccessFailedCount { get; set; }

    /// <summary>
    /// Whether lockout is enabled for this user
    /// </summary>
    public bool LockoutEnabled { get; set; } = true;

    /// <summary>
    /// Lockout end timestamp
    /// </summary>
    public DateTimeOffset? LockoutEnd { get; set; }
}

