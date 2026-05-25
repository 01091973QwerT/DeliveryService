using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using DeliveryService.Domain.Entities;
using DeliveryService.ValueObjects;
using DeliveryService.ValueObjects.Validators;

namespace DeliveryService.Infrastructure.EntityFramework.Configurations;

public class ReceiverConfiguration : IEntityTypeConfiguration<Receiver>
{
    public void Configure(EntityTypeBuilder<Receiver> builder)
    {
        builder.ToTable("receivers");

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
    }
}
