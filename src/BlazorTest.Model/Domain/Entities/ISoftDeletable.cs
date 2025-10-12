namespace BlazorTest.Model.Domain.Entities;

/// <summary>
/// Interface for entities that support soft deletion
/// </summary>
public interface ISoftDeletable
{
    /// <summary>
    /// Indicates whether the entity is deleted
    /// </summary>
    bool IsDeleted { get; set; }

    /// <summary>
    /// UTC timestamp when the entity was deleted
    /// </summary>
    DateTime? DeletedAt { get; set; }

    /// <summary>
    /// User ID who deleted the entity
    /// </summary>
    string? DeletedBy { get; set; }
}

