namespace Nebx.BuildingBlocks.AspNetCore.DDD;

public interface ITimeAuditable
{
    public DateTime CreatedOn { get; set; }
    public DateTime? ModifiedOn { get; set; }
}