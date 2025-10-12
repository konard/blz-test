namespace BlazorTest.Model.Domain.Identity;

/// <summary>
/// Interface for user entity
/// </summary>
public interface IUser
{
    /// <summary>
    /// Unique identifier for the user
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Username for the user
    /// </summary>
    string UserName { get; }

    /// <summary>
    /// Email address of the user
    /// </summary>
    string Email { get; }

    /// <summary>
    /// Hashed password of the user
    /// </summary>
    string PasswordHash { get; }

    /// <summary>
    /// List of roles assigned to the user
    /// </summary>
    List<string> Roles { get; }

    /// <summary>
    /// Indicates whether the user account is active
    /// </summary>
    bool IsActive { get; }

    /// <summary>
    /// UTC timestamp when the user was created
    /// </summary>
    DateTime CreatedAt { get; }

    /// <summary>
    /// UTC timestamp of the user's last login
    /// </summary>
    DateTime? LastLoginAt { get; }
}

