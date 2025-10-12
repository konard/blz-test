namespace BlazorTest.Model.Domain.Entities;

/// <summary>
/// Base entity with audit tracking capabilities
/// </summary>
public abstract class AuditableEntity : BaseEntity
{
    /// <summary>
    /// User ID who created the entity
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// User ID who last modified the entity
    /// </summary>
    public string? ModifiedBy { get; set; }
}

