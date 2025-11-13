using System.Collections.Immutable;

namespace Nebx.Labs.Integrations.Email.Emails;

/// <summary>
/// A fluent builder for constructing <see cref="EmailMessage"/> objects
/// in a provider-agnostic way.
/// </summary>
public sealed class EmailMessageBuilder
{
    private EmailMessage _message = new();

    /// <summary>
    /// Sets the sender ("From") address of the email message.
    /// </summary>
    public EmailMessageBuilder From(EmailAddress from)
    {
        _message = _message with { From = from };
        return this;
    }

    /// <summary>
    /// Sets the primary recipients (To). Duplicate addresses are removed case-insensitively.
    /// </summary>
    public EmailMessageBuilder To(IReadOnlyList<EmailAddress> recipients)
    {
        _message = _message with
        {
            To = recipients
                .GroupBy(r => r.Address, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.First())
                .ToImmutableHashSet()
        };
        return this;
    }

    /// <summary>
    /// Sets the CC recipients. Duplicate addresses are removed case-insensitively.
    /// </summary>
    public EmailMessageBuilder Cc(IReadOnlyList<EmailAddress> recipients)
    {
        _message = _message with
        {
            Cc = recipients
                .GroupBy(r => r.Address, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.First())
                .ToImmutableHashSet()
        };
        return this;
    }

    /// <summary>
    /// Sets the BCC recipients. Duplicate addresses are removed case-insensitively.
    /// </summary>
    public EmailMessageBuilder Bcc(IReadOnlyList<EmailAddress> recipients)
    {
        _message = _message with
        {
            Bcc = recipients
                .GroupBy(r => r.Address, StringComparer.OrdinalIgnoreCase)
                .Select(g => g.First())
                .ToImmutableHashSet()
        };
        return this;
    }

    /// <summary>
    /// Sets the email subject.
    /// </summary>
    public EmailMessageBuilder Subject(string subject)
    {
        _message = _message with { Subject = subject };
        return this;
    }

    /// <summary>
    /// Sets the email body and format type.
    /// </summary>
    public EmailMessageBuilder Body(string body, bool isHtml = true)
    {
        _message = _message with { Body = body, IsHtml = isHtml };
        return this;
    }

    /// <summary>
    /// Sets the attachments. Duplicate attachments (same file name and content) are removed.
    /// </summary>
    public EmailMessageBuilder Attachment(IReadOnlyList<EmailAttachment> attachments)
    {
        _message = _message with
        {
            Attachments = attachments
                .GroupBy(a => (a.FileName.ToLowerInvariant(), Convert.ToBase64String(a.Content)))
                .Select(g => g.First())
                .ToImmutableHashSet()
        };
        return this;
    }

    /// <summary>
    /// Builds and returns the configured <see cref="EmailMessage"/>.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown if no recipients or subject are specified.
    /// </exception>
    public EmailMessage Build()
    {
        if (_message.To.Count == 0)
            throw new InvalidOperationException("Email address must have at least one recipient");

        if (string.IsNullOrWhiteSpace(_message.Subject))
            throw new InvalidOperationException("Subject must have a value");

        return _message;
    }
}
