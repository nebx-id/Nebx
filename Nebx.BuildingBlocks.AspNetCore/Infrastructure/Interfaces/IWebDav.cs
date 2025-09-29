using WebDav;

namespace Nebx.BuildingBlocks.AspNetCore.Infrastructure.Interfaces;

/// <summary>
/// Defines operations for interacting with a WebDAV server, such as creating, retrieving, and deleting files.
/// </summary>
public interface IWebDav
{
    /// <summary>
    /// Uploads a file to the WebDAV server at the specified path.
    /// </summary>
    /// <param name="stream">The file stream to be uploaded.</param>
    /// <param name="path">The target path on the WebDAV server, including the file name.</param>
    /// <returns>A <see cref="WebDavResponse" /> indicating the result of the operation.</returns>
    public Task<WebDavResponse> CreateFile(Stream stream, string path);

    /// <summary>
    /// Retrieves metadata for a file or directory from the WebDAV server.
    /// </summary>
    /// <param name="path">The path to the file or directory on the WebDAV server.</param>
    /// <returns>A <see cref="PropfindResponse" /> containing the file or directory properties.</returns>
    public Task<PropfindResponse> PropFind(string path);

    /// <summary>
    /// Downloads a file from the WebDAV server.
    /// </summary>
    /// <param name="path">The path to the file on the WebDAV server.</param>
    /// <returns>A <see cref="WebDavStreamResponse" /> containing the file stream and metadata.</returns>
    public Task<WebDavStreamResponse> GetFile(string path);

    /// <summary>
    /// Deletes a file from the WebDAV server.
    /// </summary>
    /// <param name="path">The path to the file on the WebDAV server to delete.</param>
    /// <returns>A <see cref="WebDavResponse" /> indicating the result of the delete operation.</returns>
    public Task<WebDavResponse> DeleteFile(string path);
}