using System.Linq.Expressions;

namespace BlazorTest.Model.Contracts.Repositories;

/// <summary>
/// Generic repository interface for data access
/// </summary>
/// <typeparam name="T">Entity type</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Gets an entity by its identifier
    /// </summary>
    /// <param name="id">Entity identifier</param>
    /// <returns>Entity if found, null otherwise</returns>
    Task<T?> GetByIdAsync(string id);

    /// <summary>
    /// Gets all entities
    /// </summary>
    /// <returns>Collection of all entities</returns>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Adds a new entity
    /// </summary>
    /// <param name="entity">Entity to add</param>
    /// <returns>The added entity</returns>
    Task<T> AddAsync(T entity);

    /// <summary>
    /// Updates an existing entity
    /// </summary>
    /// <param name="entity">Entity to update</param>
    /// <returns>True if successful, false otherwise</returns>
    Task<bool> UpdateAsync(T entity);

    /// <summary>
    /// Deletes an entity by its identifier
    /// </summary>
    /// <param name="id">Entity identifier</param>
    /// <returns>True if successful, false otherwise</returns>
    Task<bool> DeleteAsync(string id);

    /// <summary>
    /// Finds entities matching a predicate
    /// </summary>
    /// <param name="predicate">Filter predicate</param>
    /// <returns>Collection of matching entities</returns>
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
}

