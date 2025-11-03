namespace Nebx.BuildingBlocks.AspNetCore.Infra.Interfaces.Services;

/// <summary>
/// Defines methods for creating and verifying cryptographic hashes.
/// </summary>
public interface IHasher
{
    /// <summary>
    /// Generates a hash value from the specified input string.
    /// </summary>
    /// <param name="input">The plain text input to hash.</param>
    /// <returns>The resulting hashed string.</returns>
    string Hash(string input);

    /// <summary>
    /// Verifies whether a given plain text input matches a previously generated hash.
    /// </summary>
    /// <param name="input">The plain text input to verify.</param>
    /// <param name="hashed">The previously generated hash to compare against.</param>
    /// <returns><c>true</c> if the input matches the hash; otherwise, <c>false</c>.</returns>
    bool VerifyHash(string input, string hashed);
}