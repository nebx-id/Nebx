using MimeKit;

namespace Nebx.BuildingBlocks.AspNetCore.Models.Emails;

public sealed record EmailMessage
{
    internal EmailMessage()
    {
    }

    public EmailAddress? From { get; init; }
    public HashSet<EmailAddress> To { get; init; } = [];
    public HashSet<EmailAddress> Cc { get; init; } = [];
    public HashSet<EmailAddress> Bcc { get; init; } = [];
    public string Subject { get; init; } = string.Empty;
    public string Body { get; init; } = string.Empty;
    public bool IsHtml { get; init; } = true;
    public HashSet<EmailAttachment> Attachments { get; init; } = [];
}

public static class EmailMessageExtensions
{
    public static MimeMessage ToMimeMessage(this EmailMessage message)
    {
        var mimeMessage = new MimeMessage();

        var to = message.To
            .Select(x => new MailboxAddress(x.Name, x.Address))
            .ToList();

        var cc = message.Cc
            .Select(x => new MailboxAddress(x.Name, x.Address))
            .ToList();

        var bcc = message.Bcc
            .Select(x => new MailboxAddress(x.Name, x.Address))
            .ToList();

        if (message.From is not null)
        {
            var sender = new MailboxAddress(message.From.Name, message.From.Address);
            mimeMessage.From.Add(sender);
            mimeMessage.Sender = sender;
        }

        mimeMessage.To.AddRange(to);
        mimeMessage.Cc.AddRange(cc);
        mimeMessage.Bcc.AddRange(bcc);

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = message.IsHtml ? message.Body : null,
            TextBody = !message.IsHtml ? message.Body : null
        };

        foreach (var attachment in message.Attachments)
        {
            if (!ContentType.TryParse(attachment.ContentType, out var contentType))
            {
                throw new InvalidOperationException($"Invalid attachment type: {attachment.ContentType}");
            }

            bodyBuilder.Attachments.Add(attachment.FileName, attachment.Content, contentType);
        }

        mimeMessage.Subject = message.Subject;
        mimeMessage.Body = bodyBuilder.ToMessageBody();

        return mimeMessage;
    }
}