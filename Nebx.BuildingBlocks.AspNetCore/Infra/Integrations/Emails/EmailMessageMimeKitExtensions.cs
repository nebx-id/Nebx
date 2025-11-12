using MimeKit;
using Nebx.BuildingBlocks.AspNetCore.Contracts.Emails;

namespace Nebx.BuildingBlocks.AspNetCore.Infra.Integrations.Emails;

/// <summary>
/// Provides extensions for converting <see cref="EmailMessage"/> instances into MIME-compliant messages.
/// </summary>
public static class EmailMessageMimeKitExtensions
{
    /// <summary>
    /// Converts an <see cref="EmailMessage"/> to a <see cref="MimeMessage"/> for use with mail clients or SMTP.
    /// </summary>
    /// <param name="message">The email message to convert.</param>
    /// <returns>A constructed <see cref="MimeMessage"/> instance.</returns>
    /// <exception cref="InvalidOperationException">Thrown when an attachment has an invalid content type.</exception>
    public static MimeMessage ToMimeMessage(this EmailMessage message)
    {
        var mimeMessage = new MimeMessage();

        if (message.From is not null)
        {
            var sender = new MailboxAddress(message.From.Name, message.From.Address);
            mimeMessage.From.Add(sender);
            mimeMessage.Sender = sender;
        }

        mimeMessage.To.AddRange(message.To.Select(x => new MailboxAddress(x.Name, x.Address)));
        mimeMessage.Cc.AddRange(message.Cc.Select(x => new MailboxAddress(x.Name, x.Address)));
        mimeMessage.Bcc.AddRange(message.Bcc.Select(x => new MailboxAddress(x.Name, x.Address)));

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = message.IsHtml ? message.Body : null,
            TextBody = !message.IsHtml ? message.Body : null
        };

        foreach (var attachment in message.Attachments)
        {
            if (!ContentType.TryParse(attachment.ContentType, out var contentType))
                throw new InvalidOperationException($"Invalid attachment type: {attachment.ContentType}");

            bodyBuilder.Attachments.Add(attachment.FileName, attachment.Content, contentType);
        }

        mimeMessage.Subject = message.Subject;
        mimeMessage.Body = bodyBuilder.ToMessageBody();

        return mimeMessage;
    }
}
