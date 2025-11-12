using WebDav;

namespace Nebx.BuildingBlocks.AspNetCore.Infra.Integrations.Storage;

/// <summary>
/// WebDAV-based <see cref="IFileStorage"/> implementation for Nextcloud.
/// </summary>
public class NextcloudStorage : IFileStorage
{
    private readonly WebDavClient _client;
    private readonly string _basePath;

    /// <summary>
    /// Creates a new storage client targeting a specific Nextcloud user root.
    /// </summary>
    /// <param name="httpClient">The HTTP client used for WebDAV requests.</param>
    /// <param name="username">The Nextcloud username whose storage area is accessed.</param>
    public NextcloudStorage(HttpClient httpClient, string username)
    {
        _client = new WebDavClient(httpClient);
        _basePath = $"remote.php/dav/files/{username.TrimEnd('/')}/";
    }

    private string ToRemotePath(string path) =>
        _basePath + path.TrimStart('/');

    /// <inheritdoc />
    public async Task<WebDavResponse> UploadAsync(Stream content, string path)
    {
        ArgumentNullException.ThrowIfNull(content);
        if (content.Length == 0) throw new ArgumentException("Stream is empty.", nameof(content));

        var response = await _client.PutFile(ToRemotePath(path), content);
        return response;
    }

    /// <inheritdoc />
    public async Task<WebDavStreamResponse> DownloadAsync(string path)
    {
        var response = await _client.GetRawFile(ToRemotePath(path));
        return response;
    }

    /// <inheritdoc />
    public async Task<WebDavResponse> DeleteAsync(string path)
    {
        var response = await _client.Delete(ToRemotePath(path));
        return response;
    }

    /// <inheritdoc />
    public async Task<PropfindResponse> SearchAsync(string path)
    {
        var response = await _client.Propfind(ToRemotePath(path));
        return response;
    }
}
