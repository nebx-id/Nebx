namespace Nebx.BuildingBlocks.AspNetCore.Core.Failures;

/// <summary>
/// Represents a generic failure result containing an error message.
/// </summary>
/// <param name="Message">
/// A descriptive message that explains the reason for the failure.
/// </param>
public record Failure(string Message);