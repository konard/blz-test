namespace BlazorTest.Model.Constants;

/// <summary>
/// Application authorization policy constants
/// </summary>
public static class Policies
{
    /// <summary>
    /// Policy requiring administrator role
    /// </summary>
    public const string RequireAdministratorRole = "RequireAdministratorRole";

    /// <summary>
    /// Policy requiring user role or higher
    /// </summary>
    public const string RequireUserRole = "RequireUserRole";

    /// <summary>
    /// Policy requiring any authenticated user
    /// </summary>
    public const string RequireAuthenticatedUser = "RequireAuthenticatedUser";
}

