using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Nebx.BuildingBlocks.AspNetCore.FluentValidation.Extensions;

/// <summary>
/// Provides FluentValidation extensions for validating uploaded form files.
/// </summary>
public static class FormExtension
{
    /// <summary>
    /// Validates that the uploaded file has one of the allowed file extensions.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder for the file property.</param>
    /// <param name="extensions">Allowed file extensions (including the dot, e.g. ".jpg").</param>
    /// <returns>The rule builder options for further configuration.</returns>
    public static IRuleBuilderOptions<T, IFormFile> AllowedExtensions<T>(
        this IRuleBuilder<T, IFormFile> ruleBuilder,
        params string[] extensions)
    {
        var allowedExt = string.Join(", ", extensions);

        return ruleBuilder
            .Must(file => file is not null &&
                          extensions.Any(ext =>
                              string.Equals(
                                  Path.GetExtension(file.FileName),
                                  ext,
                                  StringComparison.OrdinalIgnoreCase)))
            .WithMessage($"Invalid file type. Allowed extensions: {allowedExt}.");
    }

    /// <summary>
    /// Validates that the uploaded file has a safe and valid file name.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder for the file property.</param>
    /// <returns>The rule builder options for further configuration.</returns>
    /// <remarks>
    /// Ensures the file name is shorter than 255 characters, contains no invalid characters,
    /// and does not include directory traversal or path segments.
    /// </remarks>
    public static IRuleBuilderOptions<T, IFormFile> ValidPath<T>(this IRuleBuilder<T, IFormFile> ruleBuilder)
    {
        const int maxLength = 255;

        return ruleBuilder
            .Must(file => file is not null && file.FileName.Length <= maxLength)
            .WithMessage($"File name must be {maxLength} characters or fewer.")
            .Must(file => file is not null && !file.FileName.Any(c => Path.GetInvalidFileNameChars().Contains(c)))
            .WithMessage("File name contains invalid characters.")
            .Must(file => file is not null && Path.GetFileName(file.FileName) == file.FileName)
            .WithMessage("File name is invalid or contains directory paths.");
    }

    /// <summary>
    /// Validates that the uploaded file is not empty and includes a valid content type.
    /// </summary>
    /// <typeparam name="T">The type of the object being validated.</typeparam>
    /// <param name="ruleBuilder">The rule builder for the file property.</param>
    /// <returns>The rule builder options for further configuration.</returns>
    public static IRuleBuilderOptions<T, IFormFile> NotEmpty<T>(this IRuleBuilder<T, IFormFile> ruleBuilder)
    {
        return ruleBuilder
            .Must(file => file is not null && file.Length > 0)
            .WithMessage("File cannot be empty.")
            .Must(file => file is not null && !string.IsNullOrWhiteSpace(file.ContentType))
            .WithMessage("File content type is missing or invalid.");
    }
}
