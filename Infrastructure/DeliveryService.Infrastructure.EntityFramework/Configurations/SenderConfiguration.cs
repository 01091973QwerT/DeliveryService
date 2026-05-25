using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeliveryService.Domain.Entities;
using DeliveryService.ValueObjects;
using DeliveryService.ValueObjects.Validators;

namespace DeliveryService.Infrastructure.EntityFramework.Configurations;

public class SenderConfiguration : IEntityTypeConfiguration<Sender>
{
    public void Configure(EntityTypeBuilder<Sender> builder)
    {
        builder.ToTable("senders");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property(x => x.Name)
            .HasColumnName("name")
            .IsRequired()
            .HasConversion(
                name => name.Value,
                str => new Name(str)
            )
            .HasMaxLength(NameValidator.MAX_LENGTH);

        builder.Property(x => x.Phone)
            .HasColumnName("phone")
            .IsRequired()
            .HasConversion(
                phone => phone.Value,
                str => new Phone(str)
            )
            .HasMaxLength(PhoneValidator.MAX_LENGTH);

        builder.Property<Guid?>("PassportDataId")
            .HasColumnName("passport_id");

        builder.HasOne(x => x.PassportData)
            .WithMany()
            .HasForeignKey("PassportDataId")
            .IsRequired(false);

        builder.HasMany<Delivery>("_deliveries")
            .WithOne(x => x.Sender)
            .HasForeignKey("SenderId")
            .HasPrincipalKey(x => x.Id);

        builder.Ignore(x => x.Deliveries);
    }
}
