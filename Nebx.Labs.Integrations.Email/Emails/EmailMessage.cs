using System.Collections.Immutable;

namespace Nebx.Labs.Integrations.Email.Emails;

/// <summary>
/// Represents an email message with sender, recipients, content, and attachments.
/// </summary>
public record EmailMessage
{
    internal EmailMessage()
    {
    }

    /// <summary>The sender of the email.</summary>
    public EmailAddress? From { get; init; }

    /// <summary>The primary recipients of the email.</summary>
    public IImmutableSet<EmailAddress> To { get; init; } = [];

    /// <summary>The carbon copy (CC) recipients of the email.</summary>
    public IImmutableSet<EmailAddress> Cc { get; init; } = [];

    /// <summary>The blind carbon copy (BCC) recipients of the email.</summary>
    public IImmutableSet<EmailAddress> Bcc { get; init; } = [];

    /// <summary>The subject line of the email.</summary>
    public string Subject { get; init; } = string.Empty;

    /// <summary>The message body content.</summary>
    public string Body { get; init; } = string.Empty;

    /// <summary>Indicates whether the body is formatted as HTML.</summary>
    public bool IsHtml { get; init; } = true;

    /// <summary>The attachments included in the email.</summary>
    public IImmutableSet<EmailAttachment> Attachments { get; init; } = [];
}
