namespace BlazorTest.Model.Exceptions;

/// <summary>
/// Exception for validation errors
/// </summary>
public class ValidationException : DomainException
{
    /// <summary>
    /// Collection of validation errors
    /// </summary>
    public Dictionary<string, string[]> Errors { get; }

    /// <summary>
    /// Creates a new validation exception
    /// </summary>
    public ValidationException() : base("One or more validation errors occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    /// <summary>
    /// Creates a new validation exception with a message
    /// </summary>
    /// <param name="message">Exception message</param>
    public ValidationException(string message) : base(message)
    {
        Errors = new Dictionary<string, string[]>();
    }

    /// <summary>
    /// Creates a new validation exception with validation errors
    /// </summary>
    /// <param name="errors">Dictionary of validation errors</param>
    public ValidationException(Dictionary<string, string[]> errors) : base("One or more validation errors occurred.")
    {
        Errors = errors;
    }
}

