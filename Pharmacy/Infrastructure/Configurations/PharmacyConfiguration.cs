using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Pharmacy.Infrastructure.Configurations;

public class PharmacyConfiguration : IEntityTypeConfiguration<Models.Domain.Pharmacy>
{
    public void Configure(EntityTypeBuilder<Models.Domain.Pharmacy> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(x => x.PhoneNumber)
            .IsRequired()
            .HasMaxLength(100);
    }
}