using BlazorTest.Model.Contracts.DTOs.Authentication;
using BlazorTest.Model.Contracts.Repositories;
using BlazorTest.Model.Contracts.Services;
using BlazorTest.Model.Domain.ValueObjects;
using BlazorTest.Model.Exceptions;

namespace BlazorTest.Host.Services.Authentication;

/// <summary>
/// Service for authentication operations
/// </summary>
public class AuthenticationService : IAuthenticationService
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ILogger<AuthenticationService> _logger;

    /// <summary>
    /// Creates a new authentication service instance
    /// </summary>
    public AuthenticationService(
        IUserService userService,
        IUserRepository userRepository,
        ITokenService tokenService,
        IRefreshTokenRepository refreshTokenRepository,
        ILogger<AuthenticationService> logger)
    {
        _userService = userService;
        _userRepository = userRepository;
        _tokenService = tokenService;
        _refreshTokenRepository = refreshTokenRepository;
        _logger = logger;
    }

    /// <inheritdoc/>
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        // Validate credentials
        var user = await _userService.ValidateCredentialsAsync(request.Email, request.Password);
        if (user == null)
        {
            _logger.LogWarning("Login failed for email {Email}", request.Email);
            throw new AuthenticationException("Invalid email or password");
        }

        // Generate tokens
        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        // Store refresh token
        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(_tokenService.RefreshTokenExpirationDays),
            CreatedAt = DateTime.UtcNow
        };

        await _refreshTokenRepository.AddAsync(refreshTokenEntity);

        _logger.LogInformation("User {UserId} logged in successfully", user.Id);

        return new LoginResponse
        {
            AccessToken = accessToken,
            TokenType = "Bearer",
            ExpiresIn = _tokenService.AccessTokenExpirationMinutes * 60,
            User = new UserInfo
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Roles = user.Roles
            }
        };
    }

    /// <inheritdoc/>
    public async Task<TokenResponse> RefreshTokenAsync(string refreshToken)
    {
        // Validate refresh token
        var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);
        if (storedToken == null || !storedToken.IsValid)
        {
            _logger.LogWarning("Invalid or expired refresh token");
            throw new AuthenticationException("Invalid or expired refresh token");
        }

        // Get full user from repository (includes all properties needed for token generation)
        var user = await _userRepository.GetByIdAsync(storedToken.UserId);
        if (user == null || !user.IsActive)
        {
            _logger.LogWarning("User not found or inactive for refresh token");
            throw new AuthenticationException("User not found or inactive");
        }

        // Generate new access token
        var accessToken = _tokenService.GenerateAccessToken(user);

        _logger.LogInformation("Access token refreshed for user {UserId}", user.Id);

        return new TokenResponse
        {
            AccessToken = accessToken,
            TokenType = "Bearer",
            ExpiresIn = _tokenService.AccessTokenExpirationMinutes * 60
        };
    }

    /// <inheritdoc/>
    public async Task LogoutAsync(string refreshToken)
    {
        await _refreshTokenRepository.RevokeTokenAsync(refreshToken);
        _logger.LogInformation("User logged out");
    }

    /// <inheritdoc/>
    public async Task LogoutAllAsync(string userId)
    {
        var count = await _refreshTokenRepository.RevokeAllUserTokensAsync(userId);
        _logger.LogInformation("User {UserId} logged out from {Count} devices", userId, count);
    }
}

