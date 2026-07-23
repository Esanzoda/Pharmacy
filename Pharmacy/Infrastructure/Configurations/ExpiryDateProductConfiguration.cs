using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharmacy.Models.Domain;

namespace Pharmacy.Infrastructure.Configurations;

public class ExpiryDateProductConfiguration : IEntityTypeConfiguration<ExpiryDateProduct>
{
    public void Configure(EntityTypeBuilder<ExpiryDateProduct> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.TotalOrderPrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        
        builder.Property(x => x.TotalPurchasePrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
    }
}