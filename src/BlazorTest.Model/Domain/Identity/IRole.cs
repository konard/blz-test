namespace BlazorTest.Model.Domain.Identity;

/// <summary>
/// Interface for role entity
/// </summary>
public interface IRole
{
    /// <summary>
    /// Unique identifier for the role
    /// </summary>
    string Id { get; }

    /// <summary>
    /// Name of the role
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Normalized name for the role (used for lookups)
    /// </summary>
    string NormalizedName { get; }

    /// <summary>
    /// Description of the role
    /// </summary>
    string? Description { get; }
}

