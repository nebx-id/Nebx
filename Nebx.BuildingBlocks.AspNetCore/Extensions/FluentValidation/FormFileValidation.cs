using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Nebx.BuildingBlocks.AspNetCore.Extensions.FluentValidation;

public static class FormFileValidation
{
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

    public static IRuleBuilderOptions<T, IFormFile> NotEmpty<T>(this IRuleBuilder<T, IFormFile> ruleBuilder)
    {
        return ruleBuilder
            .Must(x => x.Length > 0)
            .WithMessage("File cannot be empty.")
            .Must(x => string.IsNullOrEmpty(x.ContentType) == false)
            .WithMessage("File content type is empty.");
    }
}