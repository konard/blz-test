using BlazorTest.Model.Domain.Identity;

namespace BlazorTest.Model.Contracts.Repositories;

/// <summary>
/// Repository interface for user entities
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Gets a user by their identifier
    /// </summary>
    /// <param name="id">User identifier</param>
    /// <returns>User if found, null otherwise</returns>
    Task<IUser?> GetByIdAsync(string id);

    /// <summary>
    /// Gets a user by their email address
    /// </summary>
    /// <param name="email">Email address</param>
    /// <returns>User if found, null otherwise</returns>
    Task<IUser?> GetByEmailAsync(string email);

    /// <summary>
    /// Gets a user by their username
    /// </summary>
    /// <param name="userName">Username</param>
    /// <returns>User if found, null otherwise</returns>
    Task<IUser?> GetByUserNameAsync(string userName);

    /// <summary>
    /// Gets all users
    /// </summary>
    /// <returns>Collection of all users</returns>
    Task<IEnumerable<IUser>> GetAllAsync();

    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <param name="user">User to create</param>
    /// <returns>The created user</returns>
    Task<IUser> CreateAsync(IUser user);

    /// <summary>
    /// Updates an existing user
    /// </summary>
    /// <param name="user">User to update</param>
    /// <returns>True if successful, false otherwise</returns>
    Task<bool> UpdateAsync(IUser user);

    /// <summary>
    /// Deletes a user by their identifier
    /// </summary>
    /// <param name="id">User identifier</param>
    /// <returns>True if successful, false otherwise</returns>
    Task<bool> DeleteAsync(string id);

    /// <summary>
    /// Checks if a user exists with the given email
    /// </summary>
    /// <param name="email">Email address</param>
    /// <returns>True if exists, false otherwise</returns>
    Task<bool> ExistsByEmailAsync(string email);

    /// <summary>
    /// Checks if a user exists with the given username
    /// </summary>
    /// <param name="userName">Username</param>
    /// <returns>True if exists, false otherwise</returns>
    Task<bool> ExistsByUserNameAsync(string userName);
}

