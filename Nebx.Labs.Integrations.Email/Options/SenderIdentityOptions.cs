using System.ComponentModel.DataAnnotations;

namespace Nebx.Labs.Integrations.Email.Options;

/// <summary>
/// Represents the email identity used in the "From" field of outgoing messages.
/// This is what recipients will see when they receive an email.
/// </summary>
public record SenderIdentityOptions
{
    /// <summary>
    /// The sender's email address displayed to recipients.
    /// </summary>
    /// <example>"noreply@myapp.com"</example>
    [Required(ErrorMessage = "The sender email Address is required.")]
    public string Address { get; init; } = string.Empty;

    /// <summary>
    /// The human-readable display name shown alongside the sender address.
    /// </summary>
    /// <example>"MyApp Notifications"</example>
    [Required(ErrorMessage = "The sender Name is required.")]
    public string Name { get; init; } = string.Empty;
}
