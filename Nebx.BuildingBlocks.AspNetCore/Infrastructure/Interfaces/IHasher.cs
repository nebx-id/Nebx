namespace Nebx.BuildingBlocks.AspNetCore.Infrastructure.Interfaces;

public interface IHasher
{
    public string Hash(string input);
    public bool VerifyHash(string input, string hashed);
}