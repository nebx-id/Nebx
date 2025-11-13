namespace Nebx.Labs.Core.Domain.Exceptions;

/// <summary>
/// Represents errors that occur when a business rule or domain invariant is violated.
/// </summary>
/// <remarks>
/// Use <see cref="DomainException"/> to signal issues in the domain layer
/// that should not be treated as infrastructure or technical errors.
/// For example, throwing this exception can indicate invalid state transitions,
/// rule violations, or prohibited actions within the domain model.
/// </remarks>
public class DomainException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DomainException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public DomainException(string message) : base(message)
    {
    }
}
