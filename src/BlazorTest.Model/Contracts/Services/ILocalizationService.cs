namespace BlazorTest.Model.Contracts.Services;

/// <summary>
/// Service interface for localization operations
/// </summary>
public interface ILocalizationService
{
    /// <summary>
    /// Gets a localized string by key
    /// </summary>
    /// <param name="key">Localization key</param>
    /// <returns>Localized string</returns>
    string GetString(string key);

    /// <summary>
    /// Gets a localized string by section and key
    /// </summary>
    /// <param name="section">Section name</param>
    /// <param name="key">Localization key</param>
    /// <returns>Localized string</returns>
    string GetString(string section, string key);

    /// <summary>
    /// Sets the current culture
    /// </summary>
    /// <param name="culture">Culture code (e.g., "en-US", "ru-RU")</param>
    void SetCulture(string culture);

    /// <summary>
    /// Gets the current culture code
    /// </summary>
    string CurrentCulture { get; }

    /// <summary>
    /// Gets all supported culture codes
    /// </summary>
    IEnumerable<string> SupportedCultures { get; }
}

