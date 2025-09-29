namespace Nebx.BuildingBlocks.AspNetCore.Models.Emails;

public sealed record EmailAttachment(string FileName, byte[] Content, string ContentType);