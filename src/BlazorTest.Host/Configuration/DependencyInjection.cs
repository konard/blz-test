using BlazorTest.Host.Infrastructure.Data;
using BlazorTest.Host.Infrastructure.Data.Migrations;
using BlazorTest.Host.Infrastructure.Data.Repositories;
using BlazorTest.Host.Infrastructure.Identity;
using BlazorTest.Host.Services.Authentication;
using BlazorTest.Host.Services.Identity;
using BlazorTest.Model.Contracts.Repositories;
using BlazorTest.Model.Contracts.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BlazorTest.Host.Configuration;

/// <summary>
/// Dependency injection configuration
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds framework services to the service collection
    /// </summary>
    public static IServiceCollection AddBlazorTestFramework(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Database
        services.AddDatabase(configuration);

        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

        // Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAuthenticationService, AuthenticationService>();

        // Identity
        services.AddScoped<IPasswordHasher<ApplicationUser>, PasswordHasher<ApplicationUser>>();

        // Database initialization
        services.AddScoped<DatabaseInitializer>();

        // Authentication
        services.AddAuthentication(configuration);

        return services;
    }

    private static IServiceCollection AddDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration["Database:LiteDB:ConnectionString"]
            ?? throw new InvalidOperationException("Database connection string is not configured");

        services.AddSingleton<IDbContext>(sp => new LiteDbContext(connectionString));

        return services;
    }

    private static IServiceCollection AddAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var secretKey = configuration["Authentication:Jwt:SecretKey"]
            ?? throw new InvalidOperationException("JWT Secret Key is not configured");
        var issuer = configuration["Authentication:Jwt:Issuer"] ?? "BlazorTest";
        var audience = configuration["Authentication:Jwt:Audience"] ?? "BlazorTest.Clients";

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            // Support tokens from cookies for Blazor Server
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    // Try to get token from Authorization header first
                    var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                    // If not in header, try cookie
                    if (string.IsNullOrEmpty(token))
                    {
                        token = context.Request.Cookies[".BlazorTest.AccessToken"];
                    }

                    if (!string.IsNullOrEmpty(token))
                    {
                        context.Token = token;
                    }

                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorization();

        return services;
    }
}

