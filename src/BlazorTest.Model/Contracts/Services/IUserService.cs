using BlazorTest.Model.Contracts.DTOs.Identity;
using BlazorTest.Model.Domain.Identity;

namespace BlazorTest.Model.Contracts.Services;

/// <summary>
/// Service interface for user management operations
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Gets a user by their identifier
    /// </summary>
    /// <param name="id">User identifier</param>
    /// <returns>User DTO if found, null otherwise</returns>
    Task<UserDto?> GetByIdAsync(string id);

    /// <summary>
    /// Gets a user by their email address
    /// </summary>
    /// <param name="email">Email address</param>
    /// <returns>User DTO if found, null otherwise</returns>
    Task<UserDto?> GetByEmailAsync(string email);

    /// <summary>
    /// Gets all users
    /// </summary>
    /// <returns>Collection of user DTOs</returns>
    Task<IEnumerable<UserDto>> GetAllAsync();

    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <param name="userName">Username</param>
    /// <param name="email">Email address</param>
    /// <param name="password">Password</param>
    /// <param name="roles">Roles to assign</param>
    /// <returns>Created user DTO</returns>
    Task<UserDto> CreateAsync(string userName, string email, string password, string[] roles);

    /// <summary>
    /// Updates a user
    /// </summary>
    /// <param name="userDto">User DTO with updated information</param>
    /// <returns>True if successful, false otherwise</returns>
    Task<bool> UpdateAsync(UserDto userDto);

    /// <summary>
    /// Deletes a user
    /// </summary>
    /// <param name="id">User identifier</param>
    /// <returns>True if successful, false otherwise</returns>
    Task<bool> DeleteAsync(string id);

    /// <summary>
    /// Validates user credentials
    /// </summary>
    /// <param name="email">Email address</param>
    /// <param name="password">Password</param>
    /// <returns>User if credentials are valid, null otherwise</returns>
    Task<IUser?> ValidateCredentialsAsync(string email, string password);
}

