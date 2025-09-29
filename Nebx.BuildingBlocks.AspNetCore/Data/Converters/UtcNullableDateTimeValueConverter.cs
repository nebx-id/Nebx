using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Nebx.BuildingBlocks.AspNetCore.Data.Converters;

public class UtcNullableDateTimeValueConverter : ValueConverter<DateTime?, DateTime?>
{
    public UtcNullableDateTimeValueConverter()
        : base(
            v => v, // store as-is (assumes UTC)
            v => v.HasValue
                ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc)
                : (DateTime?)null)
    {
    }
}