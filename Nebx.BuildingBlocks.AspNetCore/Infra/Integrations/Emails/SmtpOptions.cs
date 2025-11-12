using System.ComponentModel.DataAnnotations;

namespace Nebx.BuildingBlocks.AspNetCore.Infra.Integrations.Emails;

/// <summary>
/// Represents configuration settings for connecting to an SMTP server.
/// </summary>
public record SmtpOptions
{
    /// <summary>
    /// The hostname or IP address of the SMTP server.
    /// </summary>
    /// <example>"smtp.gmail.com"</example>
    [Required(ErrorMessage = "The SMTP Host is required.")]
    public string Host { get; init; } = string.Empty;

    /// <summary>
    /// The port used when connecting to the SMTP server.
    /// Common ports: 25 (no encryption), 465 (SSL), 587 (TLS).
    /// </summary>
    /// <example>587</example>
    public int Port { get; init; } = 25;

    /// <summary>
    /// Optional SMTP authentication credentials.
    /// If null, the client will attempt to connect without authentication.
    /// </summary>
    public SmtpCredentials? Credentials { get; init; }
}

/// <summary>
/// Represents credentials used for SMTP authentication.
/// </summary>
public record SmtpCredentials
{
    /// <summary>
    /// The username for SMTP authentication.
    /// Often the same as the sender's email address.
    /// </summary>
    /// <example>"noreply@myapp.com"</example>
    [Required(ErrorMessage = "The SMTP Username is required when authentication is used.")]
    public string Username { get; init; } = string.Empty;

    /// <summary>
    /// The password for SMTP authentication.
    /// Do not store production values in source-controlled configuration.
    /// Use environment variables or a secret store.
    /// </summary>
    /// <example>"your-secure-password"</example>
    [Required(ErrorMessage = "The SMTP Password is required when authentication is used.")]
    public string Password { get; init; } = string.Empty;

    /// <summary>
    /// Indicates whether SSL/TLS encryption should be used when connecting.
    /// </summary>
    /// <example>true</example>
    public bool UseSsl { get; init; } = true;
}
