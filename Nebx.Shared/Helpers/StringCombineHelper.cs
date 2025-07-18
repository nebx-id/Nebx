namespace Nebx.Shared.Helpers;

/// <summary>
///     Provides helper methods for combining URI or path segments.
/// </summary>
public static class StringCombineHelper
{
    /// <summary>
    ///     Combines the specified parts using '/' as the default separator.
    ///     This method is commonly used to construct well-formed URIs.
    /// </summary>
    /// <param name="parts">The URI or path segments to combine.</param>
    /// <returns>A single combined string with segments separated by '/'.</returns>
    /// <exception cref="ArgumentException">Thrown when no parts are provided.</exception>
    public static string Combine(params string[] parts)
    {
        return Combine('/', parts);
    }

    /// <summary>
    ///     Combines the specified parts using a custom separator character.
    ///     Leading and trailing separators in each part are trimmed before joining.
    /// </summary>
    /// <param name="separator">The character to use as a separator (e.g., '/', '\\', '-').</param>
    /// <param name="parts">The segments to combine.</param>
    /// <returns>A single combined string with segments separated by the specified character.</returns>
    /// <exception cref="ArgumentException">Thrown when no parts are provided.</exception>
    public static string Combine(char separator, params string[] parts)
    {
        if (parts == null || parts.Length == 0) throw new ArgumentException("At least one part is required.");

        var cleanedParts = parts
            .Where(part => !string.IsNullOrWhiteSpace(part))
            .Select(part => part.Trim(separator)); // Trim the chosen separator

        return string.Join(separator, cleanedParts);
    }
}