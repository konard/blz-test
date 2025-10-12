using System.Linq.Expressions;
using BlazorTest.Model.Contracts.Repositories;
using LiteDB;

namespace BlazorTest.Host.Infrastructure.Data.Repositories;

/// <summary>
/// Base repository implementation for LiteDB.
/// </summary>
/// <typeparam name="T">The entity type</typeparam>
public class Repository<T> : IRepository<T> where T : class
{
    protected readonly IDbContext _context;
    protected readonly ILiteCollection<T> _collection;

    /// <summary>
    /// Initializes a new instance of the Repository.
    /// </summary>
    /// <param name="context">The database context</param>
    /// <param name="collectionName">Optional collection name. If null, uses type name.</param>
    public Repository(IDbContext context, string? collectionName = null)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _collection = _context.GetCollection<T>(collectionName);
    }

    /// <inheritdoc/>
    public virtual Task<T?> GetByIdAsync(string id)
    {
        var result = _collection.FindById(new BsonValue(id));
        return Task.FromResult<T?>(result);
    }

    /// <inheritdoc/>
    public virtual Task<IEnumerable<T>> GetAllAsync()
    {
        var results = _collection.FindAll();
        return Task.FromResult(results);
    }

    /// <inheritdoc/>
    public virtual Task<T> AddAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        _collection.Insert(entity);
        return Task.FromResult(entity);
    }

    /// <inheritdoc/>
    public virtual Task<bool> UpdateAsync(T entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity));

        var result = _collection.Update(entity);
        return Task.FromResult(result);
    }

    /// <inheritdoc/>
    public virtual Task<bool> DeleteAsync(string id)
    {
        var result = _collection.Delete(new BsonValue(id));
        return Task.FromResult(result);
    }

    /// <inheritdoc/>
    public virtual Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        if (predicate == null)
            throw new ArgumentNullException(nameof(predicate));

        var results = _collection.Find(predicate);
        return Task.FromResult(results);
    }
}

