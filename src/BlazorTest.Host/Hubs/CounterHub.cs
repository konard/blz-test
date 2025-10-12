using Microsoft.AspNetCore.SignalR;

namespace BlazorTest.Host.Hubs;

/// <summary>
/// SignalR hub for counter synchronization
/// </summary>
public class CounterHub : BaseHub
{
    private static int _currentCount = 0;

    /// <summary>
    /// Creates a new counter hub instance
    /// </summary>
    public CounterHub(ILogger<CounterHub> logger) : base(logger)
    {
    }

    /// <summary>
    /// Gets the current counter value
    /// </summary>
    public async Task<int> GetCurrentCount()
    {
        _logger.LogDebug("Client {ConnectionId} requested current count: {Count}",
            Context.ConnectionId,
            _currentCount);

        return await Task.FromResult(_currentCount);
    }

    /// <summary>
    /// Increments the counter and broadcasts to all clients
    /// </summary>
    public async Task IncrementCounter()
    {
        _currentCount++;

        _logger.LogInformation("Counter incremented to {Count} by client {ConnectionId}",
            _currentCount,
            Context.ConnectionId);

        // Broadcast the new count to all connected clients
        await Clients.All.SendAsync("CounterUpdated", _currentCount);
    }

    /// <summary>
    /// Resets the counter to zero and broadcasts to all clients
    /// </summary>
    public async Task ResetCounter()
    {
        _currentCount = 0;

        _logger.LogInformation("Counter reset to 0 by client {ConnectionId}",
            Context.ConnectionId);

        // Broadcast the reset to all connected clients
        await Clients.All.SendAsync("CounterUpdated", _currentCount);
    }

    /// <summary>
    /// Called when a client connects - sends current count
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();

        // Send current count to the newly connected client
        await Clients.Caller.SendAsync("CounterUpdated", _currentCount);
    }
}

