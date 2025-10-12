using BlazorTest.Host.Infrastructure.Identity;
using BlazorTest.Model.Contracts.DTOs.Identity;
using BlazorTest.Model.Contracts.Repositories;
using BlazorTest.Model.Contracts.Services;
using BlazorTest.Model.Domain.Identity;
using BlazorTest.Model.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace BlazorTest.Host.Services.Identity;

/// <summary>
/// Service for user management operations
/// </summary>
public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
    private readonly ILogger<UserService> _logger;

    /// <summary>
    /// Creates a new user service instance
    /// </summary>
    public UserService(
        IUserRepository userRepository,
        IPasswordHasher<ApplicationUser> passwordHasher,
        ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<UserDto?> GetByIdAsync(string id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        return user == null ? null : MapToDto(user);
    }

    /// <inheritdoc/>
    public async Task<UserDto?> GetByEmailAsync(string email)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        return user == null ? null : MapToDto(user);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(MapToDto);
    }

    /// <inheritdoc/>
    public async Task<UserDto> CreateAsync(string userName, string email, string password, string[] roles)
    {
        // Validate
        if (await _userRepository.ExistsByEmailAsync(email))
        {
            throw new ValidationException("A user with this email already exists");
        }

        if (await _userRepository.ExistsByUserNameAsync(userName))
        {
            throw new ValidationException("A user with this username already exists");
        }

        var user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = userName,
            Email = email,
            NormalizedEmail = email.ToUpperInvariant(),
            NormalizedUserName = userName.ToUpperInvariant(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            Roles = new List<string>(roles),
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };

        // Hash password
        user.PasswordHash = _passwordHasher.HashPassword(user, password);

        var createdUser = await _userRepository.CreateAsync(user);
        _logger.LogInformation("User {UserName} created successfully", userName);

        return MapToDto(createdUser);
    }

    /// <inheritdoc/>
    public async Task<bool> UpdateAsync(UserDto userDto)
    {
        var user = await _userRepository.GetByIdAsync(userDto.Id);
        if (user == null)
        {
            return false;
        }

        if (user is not ApplicationUser appUser)
        {
            return false;
        }

        // Update properties
        appUser.UserName = userDto.UserName;
        appUser.Email = userDto.Email;
        appUser.Roles = userDto.Roles;
        appUser.IsActive = userDto.IsActive;

        var result = await _userRepository.UpdateAsync(appUser);
        if (result)
        {
            _logger.LogInformation("User {UserId} updated successfully", userDto.Id);
        }

        return result;
    }

    /// <inheritdoc/>
    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _userRepository.DeleteAsync(id);
        if (result)
        {
            _logger.LogInformation("User {UserId} deleted successfully", id);
        }

        return result;
    }

    /// <inheritdoc/>
    public async Task<IUser?> ValidateCredentialsAsync(string email, string password)
    {
        var user = await _userRepository.GetByEmailAsync(email);
        if (user == null || !user.IsActive)
        {
            return null;
        }

        if (user is not ApplicationUser appUser)
        {
            return null;
        }

        var result = _passwordHasher.VerifyHashedPassword(appUser, appUser.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed)
        {
            _logger.LogWarning("Failed login attempt for user {Email}", email);
            return null;
        }

        // Update last login time
        appUser.LastLoginAt = DateTime.UtcNow;
        await _userRepository.UpdateAsync(appUser);

        return user;
    }

    private static UserDto MapToDto(IUser user)
    {
        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Roles = user.Roles,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            LastLoginAt = user.LastLoginAt
        };
    }
}

