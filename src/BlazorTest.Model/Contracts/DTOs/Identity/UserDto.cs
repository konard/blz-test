namespace BlazorTest.Model.Contracts.DTOs.Identity;

/// <summary>
/// Data transfer object for user
/// </summary>
public class UserDto
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

    /// <summary>
    /// Whether the user account is active
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// UTC timestamp when the user was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// UTC timestamp of last login
    /// </summary>
    public DateTime? LastLoginAt { get; set; }
}

