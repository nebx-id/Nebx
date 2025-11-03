namespace Nebx.BuildingBlocks.AspNetCore.Core.Interfaces;

/// <summary>
/// Represents a non-discoverable variant of <see cref="IModule"/> 
/// used to prevent automatic reflection-based registration.
/// </summary>
/// <remarks>
/// This interface can be implemented for testing, placeholders, or 
/// partial module definitions that should not be picked up by 
/// runtime module scanners.
/// </remarks>
internal interface IFakeModule : IModule
{
    // Intentionally left blank.
    // Implement this interface in modules you want excluded from reflection-based discovery.
}