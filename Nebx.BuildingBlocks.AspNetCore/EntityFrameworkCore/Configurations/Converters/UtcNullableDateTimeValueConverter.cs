using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Nebx.BuildingBlocks.AspNetCore.EntityFrameworkCore.Configurations.Converters;

/// <summary>
/// A value converter for ensuring that nullable <see cref="DateTime"/> values
/// are always treated as UTC when read from the database.
/// </summary>
/// <remarks>
/// <para>
/// Entity Framework Core does not preserve <see cref="DateTimeKind"/> when reading values
/// from the database. This converter enforces <see cref="DateTimeKind.Utc"/> for all
/// non-null <see cref="DateTime"/> values retrieved.
/// </para>
/// <para>
/// When saving values, the converter stores them as-is, assuming they are already in UTC.
/// Consumers are responsible for ensuring that any <see cref="DateTime"/> values saved
/// are properly normalized to UTC.
/// </para>
/// </remarks>
/// <example>
/// Applying the converter in a DbContext configuration:
/// <code><![CDATA[
/// protected override void OnModelCreating(ModelBuilder modelBuilder)
/// {
///     modelBuilder.Entity<User>()
///         .Property(u => u.LastLoginAt)
///         .HasConversion(new UtcNullableDateTimeValueConverter());
/// }
/// ]]></code>
/// </example>
internal class UtcNullableDateTimeValueConverter : ValueConverter<DateTime?, DateTime?>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UtcNullableDateTimeValueConverter"/> class.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item><description>When saving to the database: stores the <see cref="DateTime"/> value as-is (nullable).</description></item>
    /// <item><description>When reading from the database: enforces <see cref="DateTimeKind.Utc"/> for non-null values.</description></item>
    /// </list>
    /// </remarks>
    public UtcNullableDateTimeValueConverter()
        : base(
            v => v, // store as-is (assumes UTC)
            v => v.HasValue
                ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc)
                : (DateTime?)null)
    {
    }
}
