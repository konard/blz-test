using LiteDB;

namespace BlazorTest.Host.Infrastructure.Data;

/// <summary>
/// LiteDB implementation of the database context.
/// </summary>
public class LiteDbContext : IDbContext
{
    private readonly LiteDatabase _database;
    private bool _disposed = false;

    /// <summary>
    /// Initializes a new instance of the LiteDbContext.
    /// </summary>
    /// <param name="connectionString">LiteDB connection string</param>
    public LiteDbContext(string connectionString)
    {
        _database = new LiteDatabase(connectionString);
    }

    /// <inheritdoc/>
    public ILiteCollection<T> GetCollection<T>(string? name = null)
    {
        return _database.GetCollection<T>(name);
    }

    /// <inheritdoc/>
    public bool BeginTrans()
    {
        return _database.BeginTrans();
    }

    /// <inheritdoc/>
    public bool Commit()
    {
        return _database.Commit();
    }

    /// <inheritdoc/>
    public bool Rollback()
    {
        return _database.Rollback();
    }

    /// <summary>
    /// Disposes the database context.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the database context.
    /// </summary>
    /// <param name="disposing">Whether to dispose managed resources</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _database?.Dispose();
            }
            _disposed = true;
        }
    }
}
