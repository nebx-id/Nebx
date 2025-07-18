namespace Nebx.BuildingBlocks.AspNetCore.DDD;

public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
}