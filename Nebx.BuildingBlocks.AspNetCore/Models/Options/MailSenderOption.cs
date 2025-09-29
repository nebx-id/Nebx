using System.ComponentModel.DataAnnotations;

namespace Nebx.BuildingBlocks.AspNetCore.Models.Options;

/// <summary>
/// Represents configuration options for an email sender,
/// including connection settings and sender identity details.
/// </summary>
public record MailSenderOption
{
    /// <summary>
    /// Gets the SMTP host name or IP address of the mail server.
    /// </summary>
    /// <example>"smtp.gmail.com"</example>
    [Required(ErrorMessage = "The Host is required.")]
    public string Host { get; init; } = string.Empty;

    /// <summary>
    /// Gets the sender's email address used when sending messages.
    /// </summary>
    /// <example>"noreply@myapp.com"</example>
    [Required(ErrorMessage = "The Address is required.")]
    public string Address { get; init; } = string.Empty;

    /// <summary>
    /// Gets the sender's display name associated with the email address.
    /// </summary>
    /// <example>"MyApp Notifications"</example>
    [Required(ErrorMessage = "The Name is required.")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Gets the port number used to connect to the SMTP server.
    /// Typically, 25, 465 (SSL), or 587 (TLS).
    /// </summary>
    /// <example>587</example>
    public int Port { get; init; } = 0;
}
