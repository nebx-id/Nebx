using WebDav;
using Services_IWebDavClient = Nebx.BuildingBlocks.AspNetCore.Infra.Interfaces.Services.IWebDavClient;

namespace Nebx.BuildingBlocks.AspNetCore.Infra.Implementations;

/// <summary>
/// Provides a WebDAV client implementation for interacting with a Nextcloud server.
/// </summary>
public class NextcloudClient : Services_IWebDavClient
{
    private readonly WebDavClient _client;
    private readonly string _basePath;

    /// <summary>
    /// Initializes a new instance of the <see cref="NextcloudClient"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client used for WebDAV communication.</param>
    /// <param name="username">The Nextcloud username associated with the target file path.</param>
    public NextcloudClient(HttpClient httpClient, string username)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentException.ThrowIfNullOrWhiteSpace(username);

        _client = new WebDavClient(httpClient);
        _basePath = $"remote.php/dav/files/{username.TrimEnd('/')}/";
    }

    /// <summary>
    /// Combines the base path with the specified relative path.
    /// </summary>
    private string BuildUri(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        return _basePath + path.TrimStart('/');
    }

    /// <summary>
    /// Creates or overwrites a file in Nextcloud using the specified stream.
    /// </summary>
    /// <param name="stream">The content stream to upload.</param>
    /// <param name="path">The relative path to the target file.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown when the stream is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the stream is empty or path is invalid.</exception>
    public async Task<WebDavResponse> CreateFile(Stream stream, string path)
    {
        ArgumentNullException.ThrowIfNull(stream);
        if (stream.Length == 0)
            throw new ArgumentException("The stream cannot be empty.", nameof(stream));

        return await _client.PutFile(BuildUri(path), stream).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves metadata and properties for the specified path.
    /// </summary>
    public Task<PropfindResponse> PropFind(string path) =>
        _client.Propfind(BuildUri(path));

    /// <summary>
    /// Downloads a file as a raw stream.
    /// </summary>
    public Task<WebDavStreamResponse> GetFile(string path) =>
        _client.GetRawFile(BuildUri(path));

    /// <summary>
    /// Deletes a file at the specified path.
    /// </summary>
    public Task<WebDavResponse> DeleteFile(string path) =>
        _client.Delete(BuildUri(path));
}