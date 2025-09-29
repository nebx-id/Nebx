namespace Nebx.BuildingBlocks.AspNetCore.Models.DomainDriven;

public interface ITimeAuditable
{
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}