using System.ComponentModel.DataAnnotations;

namespace Nebx.Labs.Integrations.Storage.Options;

/// <summary>
/// Represents configuration options required to connect to a WebDAV-enabled
/// Nextcloud server.
/// </summary>
/// <remarks>
/// These options define the authentication credentials and server endpoint
/// used when establishing WebDAV communication.
/// </remarks>
public record WebDavOptions
{
    /// <summary>
    /// Gets the base URL or hostname of the Nextcloud server.
    /// </summary>
    /// <remarks>
    /// This value must point to the WebDAV-accessible endpoint of the server.
    /// </remarks>
    [Required(ErrorMessage = "The Host is required.")]
    public string Host { get; init; } = string.Empty;

    /// <summary>
    /// Gets the username used to authenticate against the Nextcloud server.
    /// </summary>
    [Required(ErrorMessage = "The Username is required.")]
    public string Username { get; init; } = string.Empty;

    /// <summary>
    /// Gets the password used to authenticate against the Nextcloud server.
    /// </summary>
    [Required(ErrorMessage = "The Password is required.")]
    public string Password { get; init; } = string.Empty;
}
