namespace BlazorTest.Model.Contracts.DTOs.Identity;

/// <summary>
/// Data transfer object for role
/// </summary>
public class RoleDto
{
    /// <summary>
    /// Role identifier
    /// </summary>
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Role name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Role description
    /// </summary>
    public string? Description { get; set; }
}

