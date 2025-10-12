namespace BlazorTest.Model.Exceptions;

/// <summary>
/// Exception for authentication-related errors
/// </summary>
public class AuthenticationException : DomainException
{
    /// <summary>
    /// Creates a new authentication exception
    /// </summary>
    public AuthenticationException() : base()
    {
    }

    /// <summary>
    /// Creates a new authentication exception with a message
    /// </summary>
    /// <param name="message">Exception message</param>
    public AuthenticationException(string message) : base(message)
    {
    }

    /// <summary>
    /// Creates a new authentication exception with a message and inner exception
    /// </summary>
    /// <param name="message">Exception message</param>
    /// <param name="innerException">Inner exception</param>
    public AuthenticationException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

