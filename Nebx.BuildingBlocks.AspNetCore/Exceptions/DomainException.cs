namespace Nebx.BuildingBlocks.AspNetCore.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }
}