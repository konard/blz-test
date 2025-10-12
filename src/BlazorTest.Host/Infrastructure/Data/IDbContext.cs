using LiteDB;

namespace BlazorTest.Host.Infrastructure.Data;

/// <summary>
/// Database context abstraction for data access operations.
/// </summary>
public interface IDbContext : IDisposable
{
    /// <summary>
    /// Gets a collection from the database.
    /// </summary>
    /// <typeparam name="T">The entity type</typeparam>
    /// <param name="name">Optional collection name. If null, uses type name.</param>
    /// <returns>The collection</returns>
    ILiteCollection<T> GetCollection<T>(string? name = null);

    /// <summary>
    /// Begins a transaction.
    /// </summary>
    /// <returns>True if transaction started successfully</returns>
    bool BeginTrans();

    /// <summary>
    /// Commits the current transaction.
    /// </summary>
    /// <returns>True if commit was successful</returns>
    bool Commit();

    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    /// <returns>True if rollback was successful</returns>
    bool Rollback();
}
