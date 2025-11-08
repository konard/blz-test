using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BlazorTest.Host.Services.Authentication;

/// <summary>
/// Custom authentication state provider for Blazor Server that reads JWT tokens from cookies
/// </summary>
public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<CustomAuthenticationStateProvider> _logger;
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public CustomAuthenticationStateProvider(
        IHttpContextAccessor httpContextAccessor,
        ILogger<CustomAuthenticationStateProvider> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        if (httpContext == null)
        {
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        }

        // Try to get token from cookie
        var token = httpContext.Request.Cookies[".BlazorTest.AccessToken"];

        if (string.IsNullOrEmpty(token))
        {
            _logger.LogDebug("No authentication token found in cookies");
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        }

        try
        {
            // Validate and read the token
            var jwtToken = _tokenHandler.ReadJwtToken(token);

            // Check if token is expired
            if (jwtToken.ValidTo < DateTime.UtcNow)
            {
                _logger.LogDebug("Token has expired");
                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
            }

            // Create claims identity from token
            var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            _logger.LogDebug("User authenticated from token: {UserId}", jwtToken.Subject);

            return Task.FromResult(new AuthenticationState(user));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reading authentication token");
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));
        }
    }

    /// <summary>
    /// Marks a user as authenticated
    /// </summary>
    public void MarkUserAsAuthenticated(string token)
    {
        try
        {
            var jwtToken = _tokenHandler.ReadJwtToken(token);
            var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
            _logger.LogInformation("User marked as authenticated");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking user as authenticated");
        }
    }

    /// <summary>
    /// Marks a user as logged out
    /// </summary>
    public void MarkUserAsLoggedOut()
    {
        var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(anonymousUser)));
        _logger.LogInformation("User marked as logged out");
    }
}
