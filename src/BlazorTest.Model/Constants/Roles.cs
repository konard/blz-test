namespace BlazorTest.Model.Constants;

/// <summary>
/// Application role constants
/// </summary>
public static class Roles
{
    /// <summary>
    /// Guest role - minimal permissions
    /// </summary>
    public const string Guest = "Guest";

    /// <summary>
    /// User role - standard user permissions
    /// </summary>
    public const string User = "User";

    /// <summary>
    /// Administrator role - full permissions
    /// </summary>
    public const string Administrator = "Administrator";

    /// <summary>
    /// All available roles
    /// </summary>
    public static readonly string[] All = { Guest, User, Administrator };
}

