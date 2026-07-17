using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pharmasy.Models.Domain;

namespace Pharmasy.Infrastructure.Configurations;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.ToTable("Carts");

        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.Customer)
            .WithOne(c => c.Cart)
            .HasForeignKey<Cart>(x => x.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.CustomerId)
            .IsUnique();
        builder.Property(x => x.TotalAmount)
            .HasColumnType("decimal(18,2)");
    }
}