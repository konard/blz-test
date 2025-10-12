using BlazorTest.Model.Contracts.DTOs.Authentication;
using BlazorTest.Model.Contracts.Services;
using BlazorTest.Model.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace BlazorTest.Host.Endpoints;

/// <summary>
/// Authentication API endpoints
/// </summary>
public static class AuthenticationEndpoints
{
    /// <summary>
    /// Maps authentication endpoints to the application
    /// </summary>
    /// <param name="app">Web application</param>
    public static void MapAuthenticationEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth")
            .WithTags("Authentication");

        group.MapPost("/login", Login)
            .WithName("Login")
            .WithSummary("Authenticate user with email and password")
            .Produces<LoginResponse>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized);

        group.MapPost("/refresh", RefreshToken)
            .WithName("RefreshToken")
            .WithSummary("Refresh access token using refresh token")
            .Produces<TokenResponse>(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status401Unauthorized);

        group.MapPost("/logout", Logout)
            .WithName("Logout")
            .WithSummary("Revoke refresh token and logout")
            .Produces(StatusCodes.Status200OK)
            .Produces<ProblemDetails>(StatusCodes.Status400BadRequest);
    }

    private static async Task<IResult> Login(
        [FromBody] LoginRequest request,
        [FromServices] IAuthenticationService authService,
        HttpContext context)
    {
        try
        {
            var response = await authService.LoginAsync(request);

            // Set refresh token in HTTP-only cookie
            context.Response.Cookies.Append(".BlazorTest.RefreshToken", response.AccessToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });

            return Results.Ok(response);
        }
        catch (AuthenticationException ex)
        {
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status401Unauthorized,
                title: "Authentication Failed");
        }
        catch (Exception ex)
        {
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest,
                title: "Bad Request");
        }
    }

    private static async Task<IResult> RefreshToken(
        [FromServices] IAuthenticationService authService,
        HttpContext context)
    {
        try
        {
            // Get refresh token from cookie
            if (!context.Request.Cookies.TryGetValue(".BlazorTest.RefreshToken", out var refreshToken)
                || string.IsNullOrEmpty(refreshToken))
            {
                return Results.Problem(
                    detail: "Refresh token not found",
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: "Unauthorized");
            }

            var response = await authService.RefreshTokenAsync(refreshToken);

            return Results.Ok(response);
        }
        catch (AuthenticationException ex)
        {
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status401Unauthorized,
                title: "Authentication Failed");
        }
        catch (Exception ex)
        {
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest,
                title: "Bad Request");
        }
    }

    private static async Task<IResult> Logout(
        [FromServices] IAuthenticationService authService,
        HttpContext context)
    {
        try
        {
            // Get refresh token from cookie
            if (context.Request.Cookies.TryGetValue(".BlazorTest.RefreshToken", out var refreshToken)
                && !string.IsNullOrEmpty(refreshToken))
            {
                await authService.LogoutAsync(refreshToken);
            }

            // Clear cookie
            context.Response.Cookies.Delete(".BlazorTest.RefreshToken");

            return Results.Ok(new { message = "Logged out successfully" });
        }
        catch (Exception ex)
        {
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest,
                title: "Bad Request");
        }
    }
}

