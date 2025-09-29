using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Nebx.BuildingBlocks.AspNetCore.Data.Converters;

public class UtcDateTimeValueConverter : ValueConverter<DateTime, DateTime>
{
    public UtcDateTimeValueConverter()
        : base(
            v => v, // store as-is (assumes it's already UTC)
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)) // read as UTC
    {
    }
}