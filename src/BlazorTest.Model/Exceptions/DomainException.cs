namespace BlazorTest.Model.Exceptions;

/// <summary>
/// Base exception for domain-level errors
/// </summary>
public class DomainException : Exception
{
    /// <summary>
    /// Creates a new domain exception
    /// </summary>
    public DomainException() : base()
    {
    }

    /// <summary>
    /// Creates a new domain exception with a message
    /// </summary>
    /// <param name="message">Exception message</param>
    public DomainException(string message) : base(message)
    {
    }

    /// <summary>
    /// Creates a new domain exception with a message and inner exception
    /// </summary>
    /// <param name="message">Exception message</param>
    /// <param name="innerException">Inner exception</param>
    public DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

