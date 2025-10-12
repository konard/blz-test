using Microsoft.AspNetCore.SignalR;

namespace BlazorTest.Host.Hubs;

/// <summary>
/// Base hub class with common functionality
/// </summary>
public abstract class BaseHub : Hub
{
    protected readonly ILogger _logger;

    /// <summary>
    /// Creates a new base hub instance
    /// </summary>
    protected BaseHub(ILogger logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Called when a client connects
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("Client {ConnectionId} connected to {HubName}",
            Context.ConnectionId,
            GetType().Name);
        await base.OnConnectedAsync();
    }

    /// <summary>
    /// Called when a client disconnects
    /// </summary>
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (exception != null)
        {
            _logger.LogWarning(exception, "Client {ConnectionId} disconnected with error from {HubName}",
                Context.ConnectionId,
                GetType().Name);
        }
        else
        {
            _logger.LogInformation("Client {ConnectionId} disconnected from {HubName}",
                Context.ConnectionId,
                GetType().Name);
        }

        await base.OnDisconnectedAsync(exception);
    }

    /// <summary>
    /// Gets the current user ID from the context
    /// </summary>
    protected string? GetUserId()
    {
        return Context.User?.FindFirst(Model.Constants.ClaimTypes.UserId)?.Value;
    }

    /// <summary>
    /// Gets the current username from the context
    /// </summary>
    protected string? GetUserName()
    {
        return Context.User?.FindFirst(Model.Constants.ClaimTypes.UserName)?.Value
            ?? Context.User?.Identity?.Name;
    }
}

