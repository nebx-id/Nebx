namespace Nebx.BuildingBlocks.AspNetCore.Core.Models.Emails;

/// <summary>
/// Represents an email attachment with metadata and content.
/// </summary>
/// <param name="FileName">The name of the attached file.</param>
/// <param name="Content">The file content as a byte array.</param>
/// <param name="ContentType">The MIME type of the file.</param>
public record EmailAttachment(string FileName, byte[] Content, string ContentType);