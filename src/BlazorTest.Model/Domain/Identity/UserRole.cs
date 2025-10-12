namespace BlazorTest.Model.Domain.Identity;

/// <summary>
/// Represents the relationship between a user and a role
/// </summary>
public class UserRole
{
    /// <summary>
    /// User identifier
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    /// Role name
    /// </summary>
    public string RoleName { get; set; } = string.Empty;

    /// <summary>
    /// UTC timestamp when the role was assigned
    /// </summary>
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
}

