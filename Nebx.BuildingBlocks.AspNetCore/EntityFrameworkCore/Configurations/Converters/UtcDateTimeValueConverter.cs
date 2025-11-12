using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Nebx.BuildingBlocks.AspNetCore.EntityFrameworkCore.Configurations.Converters;

/// <summary>
/// A value converter for ensuring that <see cref="DateTime"/> values are always treated as UTC
/// when read from the database.
/// </summary>
/// <remarks>
/// <para>
/// Entity Framework Core does not automatically track <see cref="DateTimeKind"/> when reading
/// values from the database. This converter ensures that all <see cref="DateTime"/> values
/// retrieved are explicitly marked as <see cref="DateTimeKind.Utc"/>.
/// </para>
/// <para>
/// When saving values, the converter stores the <see cref="DateTime"/> as-is,
/// assuming it is already in UTC. Consumers are responsible for ensuring that
/// <see cref="DateTime"/> values written to the database are in UTC.
/// </para>
/// </remarks>
/// <example>
/// Applying the converter in a DbContext configuration:
/// <code><![CDATA[
/// protected override void OnModelCreating(ModelBuilder modelBuilder)
/// {
///     modelBuilder.Entity<Order>()
///         .Property(o => o.CreatedAt)
///         .HasConversion(new UtcDateTimeValueConverter());
/// }
/// ]]></code>
/// </example>
internal class UtcDateTimeValueConverter : ValueConverter<DateTime, DateTime>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UtcDateTimeValueConverter"/> class.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item><description>When saving to the database: stores the <see cref="DateTime"/> value as-is.</description></item>
    /// <item><description>When reading from the database: forces <see cref="DateTimeKind.Utc"/>.</description></item>
    /// </list>
    /// </remarks>
    public UtcDateTimeValueConverter()
        : base(
            v => v, // store as-is (assumes it's already UTC)
            v => DateTime.SpecifyKind(v, DateTimeKind.Utc)) // read as UTC
    {
    }
}
