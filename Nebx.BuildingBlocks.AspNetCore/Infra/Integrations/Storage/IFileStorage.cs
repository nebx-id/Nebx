using WebDav;

namespace Nebx.BuildingBlocks.AspNetCore.Infra.Integrations.Storage;

/// <summary>
/// Defines a contract for interacting with a remote file storage system
/// using WebDAV or a compatible protocol.
/// </summary>
public interface IFileStorage
{
    /// <summary>
    /// Uploads a file to the specified remote path, creating or replacing the target resource if it already exists.
    /// </summary>
    /// <param name="content">The stream containing the file data to upload. Must be readable and non-empty.</param>
    /// <param name="path">The remote file path where the content should be stored.</param>
    /// <returns>
    /// A <see cref="WebDavResponse"/> representing the result of the upload operation,
    /// including the underlying HTTP status code and response details.
    /// </returns>
    Task<WebDavResponse> UploadAsync(Stream content, string path);

    /// <summary>
    /// Downloads the file located at the specified remote path and returns its content as a stream.
    /// </summary>
    /// <param name="path">The remote file path to download.</param>
    /// <returns>
    /// A <see cref="WebDavStreamResponse"/> containing the downloaded file's data stream.
    /// The caller is responsible for disposing the returned stream when finished.
    /// </returns>
    Task<WebDavStreamResponse> DownloadAsync(string path);

    /// <summary>
    /// Deletes the file located at the specified remote path.
    /// </summary>
    /// <param name="path">The remote file path to delete.</param>
    /// <returns>
    /// A <see cref="WebDavResponse"/> representing the result of the delete operation.
    /// The response indicates whether the deletion was successful or if the file did not exist.
    /// </returns>
    Task<WebDavResponse> DeleteAsync(string path);

    /// <summary>
    /// Retrieves metadata for files or directories under the specified remote path using a WebDAV <c>PROPFIND</c> request.
    /// </summary>
    /// <param name="path">The remote path to query.</param>
    /// <returns>
    /// A <see cref="PropfindResponse"/> containing information about matching resources,
    /// including their URIs and properties. The response may be empty if no resources are found.
    /// </returns>
    Task<PropfindResponse> SearchAsync(string path);
}
