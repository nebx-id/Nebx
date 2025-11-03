using Nebx.BuildingBlocks.AspNetCore.Core.Contracts.Emails;

namespace Nebx.BuildingBlocks.AspNetCore.Infra.Implementations;

/// <summary>
/// A fluent builder for constructing <see cref="EmailMessage"/> objects
/// in a provider-agnostic way.
/// </summary>
public sealed class EmailMessageBuilder
{
    private EmailMessage _message = new();

    /// <summary>
    /// Sets the sender ("From") address of the email message.
    /// Replaces any existing sender with the provided <see cref="EmailAddress"/>.
    /// </summary>
    /// <param name="from">
    /// The <see cref="EmailAddress"/> representing the sender, including
    /// both display name and email address.
    /// </param>
    /// <returns>The current <see cref="EmailMessageBuilder"/> instance.</returns>
    public EmailMessageBuilder From(EmailAddress from)
    {
        _message = _message with { From = from };
        return this;
    }

    /// <summary>
    /// Sets the recipients of the email message.
    /// Replaces any existing recipients with the provided list.
    /// Duplicate addresses (case-insensitive) are automatically removed.
    /// </summary>
    /// <param name="recipients">A read-only list of recipients.</param>
    /// <returns>The current <see cref="EmailMessageBuilder"/> instance.</returns>
    public EmailMessageBuilder To(IReadOnlyList<EmailAddress> recipients)
    {
        _message = _message with { To = recipients.ToHashSet() };
        return this;
    }

    /// <summary>
    /// Sets the CC (carbon copy) recipients of the email message.
    /// Replaces any existing CC list with the provided recipients.
    /// Duplicate addresses (case-insensitive) are automatically removed.
    /// </summary>
    /// <param name="recipients">A read-only list of CC recipients.</param>
    /// <returns>The current <see cref="EmailMessageBuilder"/> instance.</returns>
    public EmailMessageBuilder Cc(IReadOnlyList<EmailAddress> recipients)
    {
        _message = _message with { Cc = recipients.ToHashSet() };
        return this;
    }

    /// <summary>
    /// Sets the BCC (blind carbon copy) recipients of the email message.
    /// Replaces any existing BCC list with the provided recipients.
    /// Duplicate addresses (case-insensitive) are automatically removed.
    /// </summary>
    /// <param name="recipients">A read-only list of BCC recipients.</param>
    /// <returns>The current <see cref="EmailMessageBuilder"/> instance.</returns>
    public EmailMessageBuilder Bcc(IReadOnlyList<EmailAddress> recipients)
    {
        _message = _message with { Bcc = recipients.ToHashSet() };
        return this;
    }

    /// <summary>
    /// Sets the subject of the email message.
    /// </summary>
    /// <param name="subject">The subject text.</param>
    /// <returns>The current <see cref="EmailMessageBuilder"/> instance.</returns>
    public EmailMessageBuilder Subject(string subject)
    {
        _message = _message with { Subject = subject };
        return this;
    }

    /// <summary>
    /// Sets the body of the email message.
    /// </summary>
    /// <param name="body">The body content of the email.</param>
    /// <param name="isHtml">
    /// Indicates whether the body should be treated as HTML (<c>true</c>) or plain text (<c>false</c>).
    /// </param>
    /// <returns>The current <see cref="EmailMessageBuilder"/> instance.</returns>
    public EmailMessageBuilder Body(string body, bool isHtml = true)
    {
        _message = _message with { Body = body, IsHtml = isHtml };
        return this;
    }

    /// <summary>
    /// Sets the attachments of the email message.
    /// Replaces any existing attachments with the provided list.
    /// Duplicate attachments (same file name and content) are automatically removed.
    /// Each attachment’s <see cref="EmailAttachment.ContentType"/> is validated.
    /// </summary>
    /// <param name="attachments">A read-only list of attachments to include.</param>
    /// <returns>The current <see cref="EmailMessageBuilder"/> instance.</returns>
    /// <exception cref="FormatException">
    /// Thrown if any attachment’s content type is invalid.
    /// </exception>
    public EmailMessageBuilder Attachment(IReadOnlyList<EmailAttachment> attachments)
    {
        _message = _message with { Attachments = attachments.ToHashSet() };
        return this;
    }

    /// <summary>
    /// Builds and returns the configured <see cref="EmailMessage"/>.
    /// </summary>
    /// <returns>A fully constructed <see cref="EmailMessage"/> instance.</returns>
    public EmailMessage Build()
    {
        if (_message.To.Count == 0)
        {
            throw new InvalidOperationException("Email address must have at least one recipient");
        }

        if (string.IsNullOrWhiteSpace(_message.Subject))
        {
            throw new InvalidOperationException("Subject must have a value");
        }

        return _message;
    }
}