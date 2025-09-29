using MimeKit;
using Nebx.BuildingBlocks.AspNetCore.Models.Emails;

namespace Nebx.BuildingBlocks.AspNetCore.Infrastructure.Interfaces;

/// <summary>
/// Defines a contract for sending email messages.
/// </summary>
public interface IMailer
{
    /// <summary>
    /// Sends an email message asynchronously.
    /// </summary>
    /// <param name="message">
    /// The <see cref="MimeMessage"/> object containing the sender, recipients, subject, body, and other email details.
    /// </param>
    /// <param name="cancellationToken">
    /// A token to monitor for cancellation requests. Optional.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous send operation.
    /// </returns>
    public Task Send(EmailMessage message, CancellationToken cancellationToken = default);

    public Task TryConnect(CancellationToken cancellationToken = default);
}