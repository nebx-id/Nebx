using Nebx.Labs.Integrations.Email.Emails;

namespace Nebx.Labs.Integrations.Email.Abstractions;

/// <summary>
/// Defines a contract for sending email messages and managing mail server connectivity.
/// </summary>
public interface IMailer
{
    /// <summary>
    /// Sends an email message asynchronously.
    /// </summary>
    /// <param name="message">
    /// The <see cref="EmailMessage"/> instance containing sender, recipients, subject, body, and attachments.
    /// </param>
    /// <param name="cancellationToken">
    /// A token that can be used to cancel the send operation.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous send operation.
    /// </returns>
    Task Send(EmailMessage message, CancellationToken cancellationToken = default);

    /// <summary>
    /// Attempts to establish a connection to the mail server to verify configuration and connectivity.
    /// </summary>
    /// <param name="cancellationToken">
    /// A token that can be used to cancel the connection attempt.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous connection check.
    /// </returns>
    Task TryConnect(CancellationToken cancellationToken = default);
}
