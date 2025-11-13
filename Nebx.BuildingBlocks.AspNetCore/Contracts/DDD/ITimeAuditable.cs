namespace Nebx.BuildingBlocks.AspNetCore.Contracts.DDD;

/// <summary>
/// Defines creation and modification timestamps for an auditable entity.
/// </summary>
public interface ITimeAuditable
{
    /// <summary>
    /// The date and time the entity was created.
    /// </summary>
    DateTime CreatedOn { get; set; }

    /// <summary>
    /// The date and time the entity was last modified, if applicable.
    /// </summary>
    DateTime? ModifiedOn { get; set; }
}
