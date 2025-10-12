namespace BlazorTest.Model.Contracts.DTOs.Authentication;

/// <summary>
/// Response model for successful login
/// </summary>
public class LoginResponse
{
    /// <summary>
    /// JWT access token
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Token type (typically "Bearer")
    /// </summary>
    public string TokenType { get; set; } = "Bearer";

    /// <summary>
    /// Access token expiration in seconds
    /// </summary>
    public int ExpiresIn { get; set; }

    /// <summary>
    /// User information
    /// </summary>
    public UserInfo? User { get; set; }
}

/// <summary>
/// User information included in login response
/// </summary>
public class UserInfo
{
    /// <summary>
    /// User identifier
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Username
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// List of assigned roles
    /// </summary>
    public List<string> Roles { get; set; } = new();
}

