using System.Net.Http.Headers;
using System.Text;
using Nebx.Labs.Integrations.Storage.Options;

namespace Nebx.Labs.Integrations.Storage.Providers.NextCloud;

/// <summary>
/// Represents configuration options for connecting to a Nextcloud instance.
/// </summary>
public sealed record NextCloudOptions : WebDavOptions
{
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
