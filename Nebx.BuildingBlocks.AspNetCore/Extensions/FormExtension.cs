using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Nebx.BuildingBlocks.AspNetCore.Extensions;

public static class FormExtension
{
    /// <summary>
    /// Adds a validation rule that ensures the uploaded file has one of the specified allowed extensions.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <param name="extensions">The list of allowed file extensions (including the dot, e.g., ".jpg").</param>
    /// <returns>
    /// The rule builder options for further configuration.
    /// </returns>
    public static IRuleBuilderOptions<T, IFormFile> AllowedExtensions<T>(
        this IRuleBuilder<T, IFormFile> ruleBuilder,
        params string[] extensions)
    {
        var allowedExt = string.Join(", ", extensions);

        return ruleBuilder.Must(x => extensions
                .Any(ext => Path
                    .GetExtension(x.FileName)
                    .Equals(ext, StringComparison.CurrentCultureIgnoreCase)
                )
            )
            .WithMessage($"Only {allowedExt} are allowed.");
    }

    /// <summary>
    /// Adds a validation rule that ensures the uploaded file has a valid file name and path.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>
    /// The rule builder options for further configuration.
    /// </returns>
    /// <remarks>
    /// This rule checks that the file name length is within 255 characters, does not contain invalid characters,
    /// and does not include any path traversal or directory segments.
    /// </remarks>
    public static IRuleBuilderOptions<T, IFormFile> ValidPath<T>(this IRuleBuilder<T, IFormFile> ruleBuilder)
    {
        const int length = 255;

        return ruleBuilder
            .Must(x => x.FileName.Length <= length)
            .WithMessage($"File name must be less than {length} characters.")
            .Must(x => !x.FileName.Any(c => Path.GetInvalidFileNameChars().Contains(c)))
            .WithMessage("File name contains invalid characters.")
            .Must(x => Path.GetFileName(x.FileName) == x.FileName)
            .WithMessage("File name is invalid.");
    }

    /// <summary>
    /// Adds a validation rule that ensures the uploaded file is not empty and has a valid content type.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder.</param>
    /// <returns>
    /// The rule builder options for further configuration.
    /// </returns>
    public static IRuleBuilderOptions<T, IFormFile> NotEmpty<T>(this IRuleBuilder<T, IFormFile> ruleBuilder)
    {
        return ruleBuilder
            .Must(x => x.Length > 0)
            .WithMessage("File cannot be empty.")
            .Must(x => !string.IsNullOrEmpty(x.ContentType))
            .WithMessage("File content type is empty.");
    }
}