namespace BlazorTest.Model.Contracts.DTOs.Authentication;

/// <summary>
/// Request model for refreshing access token
/// </summary>
public class RefreshTokenRequest
{
    /// <summary>
    /// Refresh token (typically from cookie, but can be explicit)
    /// </summary>
    public string? RefreshToken { get; set; }
}

