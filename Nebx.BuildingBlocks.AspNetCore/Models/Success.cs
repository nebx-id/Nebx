namespace Nebx.BuildingBlocks.AspNetCore.Models;

/// <summary>
/// Represents a successful operation result without an associated value.
/// </summary>
/// <remarks>
/// The <see cref="Success"/> record is used to indicate that an operation completed successfully 
/// but does not need to return any data. It serves as a simple marker type in result-based APIs 
/// or command operations where the success state is sufficient.
/// </remarks>
public record Success;

/// <summary>
/// Represents a successful operation result that includes a value.
/// </summary>
/// <typeparam name="T">The type of the value returned by the successful operation.</typeparam>
/// <remarks>
/// The <see cref="Success{T}"/> record extends <see cref="Success"/> to support 
/// returning a data payload along with a success indicator. This pattern 
/// is commonly used in CQRS, functional-style result handling, or API responses 
/// where operations can succeed or fail with structured outcomes.
/// </remarks>
/// <param name="Value">The data or result associated with the successful operation.</param>
public record Success<T>(T Value) : Success;
