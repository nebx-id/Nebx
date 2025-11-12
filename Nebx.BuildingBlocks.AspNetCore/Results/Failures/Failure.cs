namespace Nebx.BuildingBlocks.AspNetCore.Results.Failures;

/// <summary>
/// Represents a generic failure result containing an error message.
/// </summary>
/// <param name="Detail">
/// A descriptive message that explains the reason for the failure.
/// </param>
public record Failure(string Detail);