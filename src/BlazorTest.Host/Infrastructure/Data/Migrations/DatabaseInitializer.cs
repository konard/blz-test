using BlazorTest.Host.Infrastructure.Identity;
using BlazorTest.Model.Constants;
using BlazorTest.Model.Contracts.Repositories;
using BlazorTest.Model.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace BlazorTest.Host.Infrastructure.Data.Migrations;

/// <summary>
/// Initializes the database with default data.
/// </summary>
public class DatabaseInitializer
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
    private readonly ILogger<DatabaseInitializer> _logger;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the DatabaseInitializer.
    /// </summary>
    public DatabaseInitializer(
        IUserRepository userRepository,
        IPasswordHasher<ApplicationUser> passwordHasher,
        ILogger<DatabaseInitializer> logger,
        IConfiguration configuration)
    {
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }

    /// <summary>
    /// Initializes the database with seed data.
    /// </summary>
    public async Task InitializeAsync()
    {
        try
        {
            _logger.LogInformation("Starting database initialization...");

            await SeedDefaultAdminAsync();

            _logger.LogInformation("Database initialization completed successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during database initialization.");
            throw;
        }
    }

    /// <summary>
    /// Seeds the default administrator user.
    /// </summary>
    private async Task SeedDefaultAdminAsync()
    {
    var adminEmail = _configuration["Identity:DefaultAdmin:Email"] ?? "admin@blazortest.local";
        var adminUserName = _configuration["Identity:DefaultAdmin:UserName"] ?? "admin";
        var adminPassword = _configuration["Identity:DefaultAdmin:Password"] ?? "Admin@123456";

        // Check if admin user already exists
        var existingAdmin = await _userRepository.GetByEmailAsync(adminEmail);
        if (existingAdmin != null)
        {
            _logger.LogInformation("Admin user already exists. Skipping creation.");
            return;
        }

        // Create admin user
        var adminUser = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            UserName = adminUserName,
            NormalizedUserName = adminUserName.ToUpperInvariant(),
            Email = adminEmail,
            NormalizedEmail = adminEmail.ToUpperInvariant(),
            IsActive = true,
            Roles = new List<string> { Roles.Administrator },
            CreatedAt = DateTime.UtcNow,
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };

        // Hash the password
        adminUser.PasswordHash = _passwordHasher.HashPassword(adminUser, adminPassword);

        // Save to database
        await _userRepository.CreateAsync(adminUser);

        _logger.LogInformation("Default admin user created successfully with email: {Email}", adminEmail);
    }
}

