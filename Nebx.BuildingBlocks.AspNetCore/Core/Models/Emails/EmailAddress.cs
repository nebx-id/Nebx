namespace Nebx.BuildingBlocks.AspNetCore.Core.Models.Emails;

/// <summary>
/// Represents an email address with an optional display name.
/// </summary>
/// <param name="Name">The display name associated with the email address.</param>
/// <param name="Address">The email address.</param>
public record EmailAddress(string Name, string Address);
