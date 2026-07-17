using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Pharmasy.Models.Domain;

namespace Pharmasy.Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id)
            .IsUnique();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Barcode)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(x => x.Barcode)
            .IsUnique();

        builder.Property(x => x.PurchasePrice)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Stock)
            .IsRequired();

        builder.Property(x => x.ExpiryDate)
            .IsRequired();

        builder.HasOne(x => x.Category)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}