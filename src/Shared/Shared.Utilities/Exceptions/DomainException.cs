namespace Shared.Utilities.Exceptions;

/// <summary>
/// Base exception for domain-specific errors
/// </summary>
public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }

    public DomainException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

// TODO: Add more specific exceptions as needed:
// - NotFoundException
// - ValidationException
// - ConcurrencyException
// - BusinessRuleException
