namespace BlazorTest.Model.Domain.ValueObjects;

/// <summary>
/// Value object representing a refresh token
/// </summary>
public class RefreshToken
{
    /// <summary>
    /// Unique identifier for the refresh token
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// The actual token string
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// User ID this token belongs to
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// UTC timestamp when the token expires
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// UTC timestamp when the token was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Indicates whether the token has been revoked
    /// </summary>
    public bool IsRevoked { get; set; }

    /// <summary>
    /// UTC timestamp when the token was revoked
    /// </summary>
    public DateTime? RevokedAt { get; set; }

    /// <summary>
    /// Checks if the refresh token is valid (not expired and not revoked)
    /// </summary>
    public bool IsValid => !IsRevoked && DateTime.UtcNow < ExpiresAt;
}

