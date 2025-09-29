using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;

namespace Nebx.BuildingBlocks.AspNetCore.Models.Options;

public record NextcloudClientOption
{
    [Required(ErrorMessage = "The Host is required.")]
    public string Host { get; init; } = string.Empty;

    [Required(ErrorMessage = "The Username is required.")]
    public string Username { get; init; } = string.Empty;

    [Required(ErrorMessage = "The Password is required.")]
    public string Password { get; init; } = string.Empty;

    private byte[] AuthBytes => Encoding.UTF8.GetBytes($"{Username}:{Password}");
    public AuthenticationHeaderValue Authorization => new("Basic", Convert.ToBase64String(AuthBytes));

    public IReadOnlyDictionary<string, string> DefaultHeaders => new Dictionary<string, string>
    {
        ["OCS-APIRequest"] = "true"
    };
}