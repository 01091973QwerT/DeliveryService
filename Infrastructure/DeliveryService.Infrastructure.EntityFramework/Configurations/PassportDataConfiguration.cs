using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeliveryService.Domain.Entities;
using DeliveryService.ValueObjects;
using DeliveryService.ValueObjects.Validators;

namespace DeliveryService.Infrastructure.EntityFramework.Configurations;

public class PassportDataConfiguration : IEntityTypeConfiguration<PassportData>
{
    public void Configure(EntityTypeBuilder<PassportData> builder)
    {
        builder.ToTable("passport_datas");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property(x => x.Series)
            .HasColumnName("series")
            .IsRequired()
            .HasConversion(
                series => series.Value,
                str => new PassportSeries(str)
            )
            .HasMaxLength(PassportSeriesValidator.EXACT_LENGTH);

        builder.Property(x => x.Number)
            .HasColumnName("number")
            .IsRequired()
            .HasConversion(
                number => number.Value,
                str => new PassportNumber(str)
            )
            .HasMaxLength(PassportNumberValidator.EXACT_LENGTH);

        builder.Property(x => x.IssuedBy)
            .HasColumnName("issued_by")
            .HasConversion(
                name => name.Value,
                str => new Name(str)
            )
            .HasMaxLength(NameValidator.MAX_LENGTH);

        builder.Property(x => x.IssuedDate)
            .HasColumnName("issued_date");

        builder.Property(x => x.BirthDate)
            .HasColumnName("birth_date");

        builder.Property(x => x.BirthPlace)
            .HasColumnName("birth_place")
            .HasConversion(
                name => name.Value,
                str => new Name(str)
            )
            .HasMaxLength(NameValidator.MAX_LENGTH);

        builder.Property(x => x.RegistrationAddress)
            .HasColumnName("registration_address")
            .HasConversion(
                addr => addr.Value,
                str => new Address(str)
            )
            .HasColumnType("text");

        builder.Property(x => x.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(x => x.UpdatedAt)
            .HasColumnName("updated_at")
            .IsRequired();
    }
}
