using BlazorTest.Host.Components;
using BlazorTest.Host.Configuration;
using BlazorTest.Host.Endpoints;
using BlazorTest.Host.Infrastructure.Data.Migrations;
using BlazorTest.Host.Services.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add MudBlazor
builder.Services.AddMudServices();

// Add SignalR
builder.Services.AddSignalR();

// Add HttpContextAccessor (required for authentication)
builder.Services.AddHttpContextAccessor();

// Add custom authentication state provider
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

// Add framework services (database, repositories, authentication, etc.)
builder.Services.AddBlazorTestFramework(builder.Configuration);

var app = builder.Build();

// Initialize database with default data
using (var scope = app.Services.CreateScope())
{
    var dbInitializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
    await dbInitializer.InitializeAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Map authentication API endpoints
app.MapAuthenticationEndpoints();

// Map SignalR hubs
app.MapHub<BlazorTest.Host.Hubs.CounterHub>("/hubs/counter");

app.Run();

