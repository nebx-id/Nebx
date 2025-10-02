using WebDav;
using IWebDavClient = Nebx.BuildingBlocks.AspNetCore.Infrastructure.Interfaces.IWebDavClient;

namespace Nebx.BuildingBlocks.AspNetCore.Infrastructure.Implementations;

public class NextcloudClient : IWebDavClient
{
    private readonly WebDavClient _client;
    private readonly string _basePath;

    public NextcloudClient(HttpClient httpClient, string username)
    {
        _client = new WebDavClient(httpClient);
        _basePath = $"remote.php/dav/files/{username}/";
    }

    private string BuildUri(string path) => _basePath + path.Trim();

    public Task<WebDavResponse> CreateFile(Stream stream, string path)
    {
        return stream.Length == 0
            ? throw new NullReferenceException("The stream length is zero.")
            : _client.PutFile(BuildUri(path), stream);
    }

    public Task<PropfindResponse> PropFind(string path) =>
        _client.Propfind(BuildUri(path));

    public Task<WebDavStreamResponse> GetFile(string path) =>
        _client.GetRawFile(BuildUri(path));

    public Task<WebDavResponse> DeleteFile(string path) =>
        _client.Delete(BuildUri(path));
}