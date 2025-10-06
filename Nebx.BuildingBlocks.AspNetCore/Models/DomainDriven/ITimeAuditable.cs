namespace Nebx.BuildingBlocks.AspNetCore.Models.DomainDriven;

/// <summary>
/// Defines a contract for entities that maintain creation and modification timestamps.
/// </summary>
/// <remarks>
/// The <see cref="ITimeAuditable"/> interface provides standard auditing properties 
/// to track when an entity was created and last modified.  
/// It is typically implemented by persistence or domain entities to enable
/// automatic timestamp management within repositories or ORM frameworks 
/// such as Entity Framework Core.
/// </remarks>
public interface ITimeAuditable
{
    /// <summary>
    /// Gets or sets the timestamp indicating when the entity was created.
    /// </summary>
    /// <remarks>
    /// This property should be set when the entity is first persisted to storage.
    /// It provides a reliable record of when the entity was initially created.
    /// </remarks>
    public DateTime CreatedOn { get; set; }

    /// <summary>
    /// Gets or sets the timestamp indicating when the entity was last modified.
    /// </summary>
    /// <remarks>
    /// This property should be updated whenever the entity is modified or saved.  
    /// If the entity has not been modified since creation, this property can remain <c>null</c>.
    /// </remarks>
    public DateTime? ModifiedOn { get; set; }
}