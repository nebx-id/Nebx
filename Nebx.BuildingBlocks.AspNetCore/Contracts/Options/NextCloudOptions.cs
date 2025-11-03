using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;

namespace Nebx.BuildingBlocks.AspNetCore.Contracts.Options;

/// <summary>
/// Represents configuration options for connecting to a Nextcloud instance.
/// </summary>
public record NextCloudOptions
{
    /// <summary>
    /// The base URL or hostname of the Nextcloud server.
    /// </summary>
    [Required(ErrorMessage = "The Host is required.")]
    public string Host { get; init; } = string.Empty;

    /// <summary>
    /// The username used for authentication.
    /// </summary>
    [Required(ErrorMessage = "The Username is required.")]
    public string Username { get; init; } = string.Empty;

    /// <summary>
    /// The password used for authentication.
    /// </summary>
    [Required(ErrorMessage = "The Password is required.")]
    public string Password { get; init; } = string.Empty;

    /// <summary>
    /// Gets the HTTP <see cref="AuthenticationHeaderValue"/> for Basic authentication.
    /// </summary>
    public AuthenticationHeaderValue Authorization =>
        new("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Username}:{Password}")));

    /// <summary>
    /// Gets the default headers required by Nextcloud API requests.
    /// </summary>
    public static IReadOnlyDictionary<string, string> DefaultHeaders { get; } =
        new Dictionary<string, string>
        {
            ["OCS-APIRequest"] = "true"
        };
}