using BlazorTest.Host.Infrastructure.Identity;
using BlazorTest.Model.Contracts.Repositories;
using BlazorTest.Model.Domain.Identity;

namespace BlazorTest.Host.Infrastructure.Data.Repositories;

/// <summary>
/// Repository for user data access operations.
/// </summary>
public class UserRepository : Repository<ApplicationUser>, IUserRepository
{
    /// <summary>
    /// Initializes a new instance of the UserRepository.
    /// </summary>
    /// <param name="context">The database context</param>
    public UserRepository(IDbContext context) : base(context, "users")
    {
        // Create indexes for efficient lookups
        _collection.EnsureIndex(x => x.Email);
        _collection.EnsureIndex(x => x.NormalizedEmail);
        _collection.EnsureIndex(x => x.UserName);
        _collection.EnsureIndex(x => x.NormalizedUserName);
    }

    // IUserRepository implementation with IUser return types
    async Task<IUser?> IUserRepository.GetByIdAsync(string id)
    {
        return await base.GetByIdAsync(id);
    }

    async Task<IUser?> IUserRepository.GetByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        var normalizedEmail = email.ToUpperInvariant();
        var results = await FindAsync(u => u.NormalizedEmail == normalizedEmail);
        return results.FirstOrDefault();
    }

    async Task<IUser?> IUserRepository.GetByUserNameAsync(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new ArgumentException("UserName cannot be empty", nameof(userName));

        var normalizedUserName = userName.ToUpperInvariant();
        var results = await FindAsync(u => u.NormalizedUserName == normalizedUserName);
        return results.FirstOrDefault();
    }

    async Task<IEnumerable<IUser>> IUserRepository.GetAllAsync()
    {
        return await base.GetAllAsync();
    }

    async Task<IUser> IUserRepository.CreateAsync(IUser user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (user is not ApplicationUser appUser)
            throw new ArgumentException("User must be of type ApplicationUser", nameof(user));

        return await base.AddAsync(appUser);
    }

    async Task<bool> IUserRepository.UpdateAsync(IUser user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        if (user is not ApplicationUser appUser)
            throw new ArgumentException("User must be of type ApplicationUser", nameof(user));

        return await base.UpdateAsync(appUser);
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        var user = await ((IUserRepository)this).GetByEmailAsync(email);
        return user != null;
    }

    /// <inheritdoc/>
    public async Task<bool> ExistsByUserNameAsync(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new ArgumentException("UserName cannot be empty", nameof(userName));

        var user = await ((IUserRepository)this).GetByUserNameAsync(userName);
        return user != null;
    }

    // Public methods with concrete ApplicationUser return types for internal use
    public async Task<ApplicationUser?> GetByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        var normalizedEmail = email.ToUpperInvariant();
        var results = await FindAsync(u => u.NormalizedEmail == normalizedEmail);
        return results.FirstOrDefault();
    }

    public async Task<ApplicationUser?> GetByUserNameAsync(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new ArgumentException("UserName cannot be empty", nameof(userName));

        var normalizedUserName = userName.ToUpperInvariant();
        var results = await FindAsync(u => u.NormalizedUserName == normalizedUserName);
        return results.FirstOrDefault();
    }
}

